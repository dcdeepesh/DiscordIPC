using System;
using System.Text.Json;
using System.IO.Pipes;
using System.Threading.Tasks;

using Dec.DiscordIPC.Events;
using System.Threading;

namespace Dec.DiscordIPC.Core {
    public class LowLevelDiscordIPC {
        private NamedPipeClientStream pipe;
        internal MessageReadLoop messageReadLoop;
        protected readonly string clientId;

        public LowLevelDiscordIPC(string clientId) {
            this.clientId = clientId;
        }

        public async Task InitAsync() {
            pipe = new NamedPipeClientStream(".", "discord-ipc-0",
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipe.ConnectAsync();

            messageReadLoop = new MessageReadLoop(pipe, this);
            messageReadLoop.Start();

            EventWaitHandle readyWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            EventHandler<Ready.Data> readyListener = (sender, data) => readyWaitHandle.Set();
            OnReady += readyListener;

            await SendMessageAsync(IPCMessage.Handshake(JsonSerializer.SerializeToUtf8Bytes(new {
                client_id = clientId,
                v = "1",
                nonce = Guid.NewGuid().ToString()
            })));

            await Task.Run(() => {
                readyWaitHandle.WaitOne();
                OnReady -= readyListener;
            });
        }

        public async Task<dynamic> SendCommandWeakTypeAsync(dynamic payload) {
            await SendMessageAsync(new IPCMessage(OpCode.FRAME, JsonSerializer.SerializeToUtf8Bytes<dynamic>(payload)));
            return await messageReadLoop.WaitForResponse(payload.nonce);
        }

        #region Events

        public event EventHandler<Ready.Data> OnReady;
        public event EventHandler<MessageCreate.Data> OnMessageCreate;
        // More events on their way

        internal void FireEvent(string evt, IPCMessage message) {
            switch (evt) {
                case "READY":
                    JsonElement elementObj = JsonSerializer.Deserialize<dynamic>(message.Json);
                    OnReady?.Invoke(this, elementObj.ToObject<Ready.Data>());
                    break;
            }
        }

        #endregion

        public void Dispose() {
            messageReadLoop.Stop();
            pipe.Dispose();
        }

        #region Private methods

        private async Task SendMessageAsync(IPCMessage message) {
            byte[] bOpCode = BitConverter.GetBytes((int) message.opCode);
            byte[] bLen = BitConverter.GetBytes(message.Length);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bOpCode);
                Array.Reverse(bLen);
            }

            byte[] buffer = new byte[4 + 4 + message.Length];
            Array.Copy(bOpCode, buffer, 4);
            Array.Copy(bLen, 0, buffer, 4, 4);
            Array.Copy(message.data, 0, buffer, 8, message.Length);
            Console.WriteLine("\nSENDING:\n{0}", message.Json);
            await pipe.WriteAsync(buffer, 0, buffer.Length);
        }

        #endregion
    }
}
