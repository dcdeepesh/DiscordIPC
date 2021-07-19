using System;
using System.Text.Json;
using System.IO.Pipes;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;

namespace Dec.DiscordIPC {
    public class DiscordIPC {
        private NamedPipeClientStream pipe;
        private MessageReadLoop messageReadLoop;

        public async Task InitAsync() {
            pipe = new NamedPipeClientStream(".", "discord-ipc-0",
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipe.ConnectAsync();

            messageReadLoop = new MessageReadLoop(pipe);
            messageReadLoop.Start();

            await SendMessageAsync(Message.Handshake(JsonSerializer.SerializeToUtf8Bytes(new {
                client_id = "850433922746810368",
                v = "1",
                nonce = Guid.NewGuid().ToString()
            })));
        }

        public async Task<dynamic> SendCommandAsync(ICommand payload) {
            await SendMessageAsync(new Message(OpCode.FRAME, JsonSerializer.SerializeToUtf8Bytes(payload)));
            var response = await messageReadLoop.WaitForResponse(payload.nonce);
            return response.data;
        }

        ~DiscordIPC() {
            messageReadLoop.Stop();
            pipe.Dispose();
        }

        #region Private methods
        private async Task SendMessageAsync(Message message) {
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
