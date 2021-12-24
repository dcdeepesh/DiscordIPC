using System.Text;
using System.Text.Json;

namespace Dec.DiscordIPC.Core {
    internal class IPCMessage {
        public readonly OpCode OpCode;
        public readonly byte[] Data;
        public readonly JsonElement Json;
        
        public int Length => this.Data.Length;
        public string RawData => Encoding.UTF8.GetString(this.Data);
        
        public IPCMessage(OpCode opCode, byte[] data) {
            this.OpCode = opCode;
            this.Data = data;
            this.Json = JsonDocument.Parse(this.RawData).RootElement;
        }
        
        public bool TryGetError(out int code, out string message) {
            if (this.Json.TryGetProperty("code", out JsonElement jCode) && jCode.TryGetInt32(out code)) {
                message = this.Json.GetProperty("message").GetString();
                return true;
            }
            
            code = 0;
            message = null;
            
            return false;
        }
        
        public static IPCMessage Handshake(byte[] data) => new IPCMessage(OpCode.HANDSHAKE, data);
    }
    
    internal enum OpCode {
        HANDSHAKE, FRAME, CLOSE, PING, PONG
    }
}
