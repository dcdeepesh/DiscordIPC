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
        private EventWaitHandle newResponseEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private CountdownEvent countdownLatch = new CountdownEvent(0);
        private LinkedList<dynamic> unhandledResponses = new LinkedList<dynamic>();
        private LinkedList<ErrorResponse> errorResponses = new LinkedList<ErrorResponse>();
        private int waiterCount = 0;

        public MessageReadLoop(NamedPipeClientStream pipe, LowLevelDiscordIPC ipcInstance) {
            this.pipe = pipe;
            this.ipcInstance = ipcInstance;
            thread = new Thread(Loop);
            thread.Name = "Message loop";
        }

        public void Start() => thread.Start();
        public void Stop() => thread.Abort();

        public Task<dynamic> WaitForResponse(string nonce) {
            return Task.Run(() => {
                bool firstRun = true;
                while (true) {
                    lock (errorResponses) {
                        foreach (var response in errorResponses)
                            if (response.nonce == nonce)
                                throw new ErrorResponseException(response);
                    }

                    dynamic result = null;
                    lock (unhandledResponses) {
                        foreach (var response in unhandledResponses) {
                            if (response.GetProperty("nonce").GetString() == nonce) {
                                result = response;
                                break;
                            }
                        }

                        if (!(result is null)) {
                            unhandledResponses.Remove(result);
                            if (!firstRun) {
                                waiterCount--;
                                countdownLatch.Signal();
                            }
                            return result;
                        }

                        if (firstRun) {
                            waiterCount++;
                            firstRun = false;
                        } else {
                            countdownLatch.Signal();
                        }
                    }

                    newResponseEvent.WaitOne();
                }
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
                            SignalNewResponse(message, evt == "ERROR");
                    });
                }
            } catch (ThreadAbortException) {
            }
        }

        private void SignalNewResponse(IPCMessage message, bool error) {
            countdownLatch.Wait();
            if (error) {
                lock (errorResponses) {
                    if (countdownLatch.Wait(1)) {
                        countdownLatch.Reset(waiterCount);
                        errorResponses.AddLast(JsonSerializer.Deserialize<ErrorResponse>(message.Json));
                        newResponseEvent.Set();
                    }
                }
            } else {
                lock (unhandledResponses) {
                    if (countdownLatch.Wait(1)) {
                        countdownLatch.Reset(waiterCount);
                        unhandledResponses.AddLast(JsonSerializer.Deserialize<dynamic>(message.Json));
                        newResponseEvent.Set();
                    }
                }
            }
        }
    }
}
