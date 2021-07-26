using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Core {
    internal class MessageReadLoop {
        private readonly LowLevelDiscordIPC ipcInstance;
        private readonly NamedPipeClientStream pipe;
        private readonly Thread thread;
        private LinkedList<Waiter> waiters = new LinkedList<Waiter>();
        private LinkedList<JsonElement> responses = new LinkedList<JsonElement>();

        public MessageReadLoop(NamedPipeClientStream pipe, LowLevelDiscordIPC ipcInstance) {
            this.pipe = pipe;
            this.ipcInstance = ipcInstance;
            thread = new Thread(Loop);
            thread.IsBackground = true;
            thread.Name = "Message loop";
        }

        public void Start() => thread.Start();

        public Task<JsonElement> WaitForResponse(string nonce) {
            return Task.Run(() => {
                Waiter waiter;
                lock (responses) {
                    JsonElement? result = null;
                    foreach (var response in responses)
                        if (response.GetProperty("nonce").GetString() == nonce)
                            result = response;
                    if (result.HasValue) {
                        responses.Remove(result.Value);
                        if (result.Value.IsErrorResponse())
                            throw new ErrorResponseException(result.Value);
                        else return result.Value;
                    }

                    waiter = new Waiter(nonce);
                    waiters.AddLast(waiter);
                }

                waiter.resetEvent.WaitOne();
                if (waiter.response.IsErrorResponse())
                    throw new ErrorResponseException(waiter.response);
                else return waiter.response;
            });
        }

        // Private methods

        private void Loop() {
            try {
                byte[] bOpCode = new byte[4];
                byte[] bLen = new byte[4];
                IPCMessage message;

                while (true) {
                    pipe.Read(bOpCode, 0, 4);
                    OpCode opCode = (OpCode) BitConverter.ToInt32(bOpCode, 0);
                    pipe.Read(bLen, 0, 4);
                    int len = BitConverter.ToInt32(bLen, 0);
                    byte[] data = new byte[len];
                    pipe.Read(data, 0, len);
                    message = new IPCMessage(opCode, data);

                    Task.Run(() => {
                        Util.Log("\nRECEIVIED:\n{0}", message.Json);
                        var jsonRoot = JsonDocument.Parse(message.Json).RootElement;
                        string cmd = jsonRoot.GetProperty("cmd").GetString();
                        string evt = "";
                        if (jsonRoot.TryGetProperty("evt", out JsonElement elem))
                            evt = elem.GetString();

                        if (cmd == "DISPATCH")
                            ipcInstance.FireEvent(evt, message);
                        else
                            SignalNewResponse(message);
                    });
                }
            } catch (ObjectDisposedException) {
                // can be thrown at exit time, no need to handle
            }
        }

        private void SignalNewResponse(IPCMessage message) {
            JsonElement response = Json.Deserialize<dynamic>(message.Json);
            lock (responses) {
                Waiter waiterToResume = null;
                foreach (var waiter in waiters)
                    if (waiter.nonce == response.GetProperty("nonce").GetString())
                        waiterToResume = waiter;

                if (waiterToResume is null is false) {
                    waiters.Remove(waiterToResume);
                    waiterToResume.response = response;
                    waiterToResume.resetEvent.Set();
                } else {
                    responses.AddLast(response);
                }                
            }
        }
    }

    internal class Waiter {
        public string nonce;
        public AutoResetEvent resetEvent = new AutoResetEvent(false);
        public JsonElement response;

        public Waiter(string nonce) => this.nonce = nonce;
    }
}
