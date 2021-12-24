using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

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
        
        #region Writers
        
        /// <summary>
        /// Await the response from the pipe for a nonce
        /// </summary>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException"></exception>
        public Task<JsonElement> WaitForResponseAsync(string nonce, CancellationToken token = default) {
            return Task.Run(async () => {
                Waiter waiter;
                lock (this.Responses) {
                    JsonElement? result = this.Responses.Where(response => response.GetProperty("nonce").GetString() == nonce)
                        .Cast<JsonElement?>()
                        .FirstOrDefault();
                    
                    if (result is JsonElement element) {
                        this.Responses.Remove(element);
                        
                        if (element.IsErrorResponse())
                            throw new ErrorResponseException(element);
                        
                        return element;
                    }
                    
                    // Create a new Waiter
                    waiter = new Waiter(nonce);
                    this.Waiters.AddLast(waiter);
                }
                
                // Await the waiter response
                await waiter.ResetEvent.WaitAsync(token);
                
                // Throw if the response is an error
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
        
        #endregion
        
        #region Awaiters
        
        /// <summary>
        /// Wait for the Stream to Connect
        /// </summary>
        public Task AwaitConnectedAsync(CancellationToken cancellationToken = default) => this.Factory.AwaitConnectedAsync(cancellationToken);
        
        /// <summary>
        /// Wait for the HELLO Event (After connecting) to have been sent
        /// </summary>
        public Task AwaitHelloAsync(CancellationToken cancellationToken = default) => this.Factory.AwaitHelloAsync(cancellationToken);
        
        #endregion
        
        #region Thread Loop Methods
        
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
                        this.Factory.Close();
                    }
                }
            } catch (ObjectDisposedException e) {
                // can be thrown at exit time, no need to handle
                Console.WriteLine(e);
            } catch (Exception e) {
                Console.WriteLine(e);
            } finally {
                this.Factory.Close();
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
            lock (this.Responses) {
                // Get the first matching Waiter (or null)
                if (this.Waiters.FirstOrDefault(waiter => waiter.Nonce == message.Json.GetProperty("nonce").GetString()) is {} waiterToResume) {
                    // Remove the waiter from the list
                    this.Waiters.Remove(waiterToResume);
                    
                    // Pass the waiters Response
                    waiterToResume.Response = message.Json;
                    
                    // Trigger the waiting await
                    waiterToResume.ResetEvent.Set();
                } else {
                    // Add the response as received before triggering a waiter
                    this.Responses.AddLast(message.Json);
                }
            }
        }
        
        #endregion
        
        public void Dispose() {
            this.TokenSource.Cancel();
            this.TokenSource.Dispose();
            this.Thread.Abort();
        }
    }
    
    internal class Waiter {
        public readonly string Nonce;
        public readonly AsyncAutoResetEvent ResetEvent = new AsyncAutoResetEvent(false);
        public JsonElement Response;
        
        public Waiter(string nonce) => this.Nonce = nonce;
    }
    internal class EndOfPipeException : Exception {}
}