using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core {
    internal sealed class LeakyPipeConnection : IDisposable {
        private readonly LeakyPipeFactory Factory;
        private readonly Action<string, IPCMessage> MessageReceiveEvent;
        
        private readonly Thread Thread;
        private readonly LinkedList<Waiter> Waiters = new LinkedList<Waiter>();
        private readonly LinkedList<JsonElement> Responses = new LinkedList<JsonElement>();
        
        private readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
        
        internal LeakyPipeConnection(
            string name,
            IPCHello<NamedPipeClientStream> streamHello,
            Func<Task> afterHello,
            Action<string, IPCMessage> messageReceiveEvent
        ) {
            this.MessageReceiveEvent = messageReceiveEvent;
            this.Factory = new LeakyPipeFactory(name, streamHello, afterHello);
            
            this.Thread = new Thread(this.Loop) {
                Name = "IPCReader",
                IsBackground = true
            };
        }
        
        /// <summary>
        /// Start the connection loop
        /// </summary>
        public void Start() => this.Thread.Start();
        
        public Task<JsonElement> WaitForResponse(string nonce, CancellationToken token) {
            return Task.Run(() => {
                Waiter waiter;
                lock (this.Responses) {
                    JsonElement? result = null;
                    foreach (JsonElement response in this.Responses)
                        if (response.GetProperty("nonce").GetString() == nonce)
                            result = response;
                    if (result.HasValue) {
                        this.Responses.Remove(result.Value);
                        if (result.Value.IsErrorResponse())
                            throw new ErrorResponseException(result.Value);
                        else return result.Value;
                    }

                    waiter = new Waiter(nonce);
                    this.Waiters.AddLast(waiter);
                }
                
                waiter.ResetEvent.WaitOne();
                if (waiter.Response.IsErrorResponse())
                    throw new ErrorResponseException(waiter.Response);
                return waiter.Response;
            }, token);
        }
        
        /// <summary>
        /// Write to the pipe asynchronously, wait if the client is not connected
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="authorized">If the connect must have finished its HELLO event before sending</param>
        /// <param name="cancellationToken"></param>
        public async Task WriteAsync(byte[] buffer, int offset, int count, bool authorized, CancellationToken cancellationToken = default) {
            // Wait for the connection to open
            NamedPipeClientStream clientStream = await this.Factory.AwaitOrCreateConnectionAsync(authorized, cancellationToken);
            
            // Write to the open connection
            await clientStream.WriteAsync(buffer, offset, count, cancellationToken);
        }
        
        /// <summary>
        /// Wait for the Stream to Connect
        /// </summary>
        public Task AwaitConnectedAsync(CancellationToken cancellationToken = default) => this.Factory.AwaitConnectedAsync(cancellationToken);
        
        /// <summary>
        /// Wait for the HELLO Event (After connecting) to have been sent
        /// </summary>
        public Task AwaitHelloAsync(CancellationToken cancellationToken = default) => this.Factory.AwaitHelloAsync(cancellationToken);
        
        /// <summary>
        /// Loop on the thread forever to listen for messages
        /// </summary>
        private void Loop() {
            try {
                // Create one cancellation token from the source
                CancellationToken cancellationToken = this.TokenSource.Token;
                while (true) {
                    try {
                        this.Run(cancellationToken);
                    } catch (EndOfPipeException) {
                        // Throw when the client is no longer useful
                        this.Factory.Dispose();
                    }
                }
            } catch (ObjectDisposedException e) {
                // can be thrown at exit time, no need to handle
                Console.WriteLine(e);
            } catch (Exception e) {
                Console.WriteLine(e);
            } finally {
                this.Factory.Dispose();
            }
        }
        
        /// <summary>
        /// Keep the connection open and then listen for events
        /// </summary>
        private void Run(CancellationToken cancellationToken = default) {
            cancellationToken.ThrowIfCancellationRequested();
            NamedPipeClientStream clientStream = this.Factory.AwaitConnection(cancellationToken);
            
            byte[] bOpCode = new byte[4];
            byte[] bLen = new byte[4];
            
            while (clientStream.IsConnected && clientStream.CanRead) {
                // Try reading the OpCode
                if (clientStream.Read(bOpCode, 0, 4) == 0)
                    throw new EndOfPipeException();
                OpCode opCode = (OpCode) BitConverter.ToInt32(bOpCode, 0);
                
                // Try reading the Payload Length
                if (clientStream.Read(bLen, 0, 4) == 0)
                    throw new EndOfPipeException();
                int len = BitConverter.ToInt32(bLen, 0);
                byte[] data = new byte[len];
                
                // Try reading the Payload
                if (clientStream.Read(data, 0, len) == 0)
                    throw new EndOfPipeException();
                
                IPCMessage message = new IPCMessage(opCode, data);
                if (message.TryGetError(out int errCode, out string errMsg)) {
                    throw new EndOfPipeException();
                } else {
                    Task.Run(() => {
                        string raw = message.RawData;
                        
                        Util.Log("RECEIVED: {0}", raw);
                        JsonElement jsonRoot = JsonDocument.Parse(raw).RootElement;
                        string cmd = jsonRoot.GetProperty("cmd").GetString();
                        string evt = "";
                        if (jsonRoot.TryGetProperty("evt", out JsonElement elem))
                            evt = elem.GetString();
                        
                        if (cmd == "DISPATCH")
                            this.MessageReceiveEvent(evt, message);
                        else
                            this.SignalNewResponse(message);
                    }, cancellationToken);
                }
            }
        }
        
        /// <summary>
        /// Signal responses to messages
        /// </summary>
        private void SignalNewResponse(IPCMessage message) {
            JsonElement response = Json.Deserialize<dynamic>(message.RawData);
            lock (this.Responses) {
                Waiter waiterToResume = null;
                foreach (Waiter waiter in this.Waiters)
                    if (waiter.Nonce == response.GetProperty("nonce").GetString())
                        waiterToResume = waiter;
                
                if (waiterToResume is null is false) {
                    this.Waiters.Remove(waiterToResume);
                    waiterToResume.Response = response;
                    waiterToResume.ResetEvent.Set();
                } else {
                    this.Responses.AddLast(response);
                }
            }
        }
        
        public void Dispose() {
            this.TokenSource.Cancel();
            this.TokenSource.Dispose();
        }
    }
    
    internal class Waiter {
        public readonly string Nonce;
        public readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);
        public JsonElement Response;
        
        public Waiter(string nonce) => this.Nonce = nonce;
    }
    internal class EndOfPipeException : Exception {}
}