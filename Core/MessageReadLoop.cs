using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core {
    internal class MessageReadLoop {
        private readonly LowLevelDiscordIPC IPCInstance;
        private readonly NamedPipeClientStream Pipe;
        private readonly Thread Thread;
        private readonly LinkedList<Waiter> Waiters = new LinkedList<Waiter>();
        private readonly LinkedList<JsonElement> Responses = new LinkedList<JsonElement>();
        
        public MessageReadLoop(NamedPipeClientStream pipe, LowLevelDiscordIPC ipcInstance) {
            this.Pipe = pipe;
            this.IPCInstance = ipcInstance;
            this.Thread = new Thread(this.Loop) {
                IsBackground = true,
                Name = "Message loop"
            };
        }
        
        public void Start() => this.Thread.Start();
        
        public Task<JsonElement> WaitForResponse(string nonce, CancellationToken token) {
            return Task.Run(() => {
                Waiter waiter;
                lock (this.Responses) {
                    JsonElement? result = null;
                    foreach (var response in this.Responses)
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
                else return waiter.Response;
            }, token);
        }
        
        // Private methods
        
        private void Loop() {
            try {
                byte[] bOpCode = new byte[4];
                byte[] bLen = new byte[4];
                
                while (true) {
                    this.Pipe.Read(bOpCode, 0, 4);
                    OpCode opCode = (OpCode) BitConverter.ToInt32(bOpCode, 0);
                    this.Pipe.Read(bLen, 0, 4);
                    int len = BitConverter.ToInt32(bLen, 0);
                    byte[] data = new byte[len];
                    this.Pipe.Read(data, 0, len);
                    IPCMessage message = new IPCMessage(opCode, data);
                    
                    Task.Run(() => {
                        Util.Log("RECEIVED: {0}", message.Json);
                        JsonElement jsonRoot = JsonDocument.Parse(message.Json).RootElement;
                        string cmd = jsonRoot.GetProperty("cmd").GetString();
                        string evt = "";
                        if (jsonRoot.TryGetProperty("evt", out JsonElement elem))
                            evt = elem.GetString();
                        
                        if (cmd == "DISPATCH")
                            this.IPCInstance.FireEvent(evt, message);
                        else
                            this.SignalNewResponse(message);
                    });
                }
            } catch (ObjectDisposedException) {
                // can be thrown at exit time, no need to handle
            }
        }
        
        private void SignalNewResponse(IPCMessage message) {
            JsonElement response = Json.Deserialize<dynamic>(message.Json);
            lock (this.Responses) {
                Waiter waiterToResume = null;
                foreach (var waiter in this.Waiters)
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
    }
    
    internal class Waiter {
        public readonly string Nonce;
        public readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);
        public JsonElement Response;
        
        public Waiter(string nonce) => this.Nonce = nonce;
    }
}
