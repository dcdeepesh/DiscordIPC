using System.Text;

namespace Dec.DiscordIPC.Core {
    internal class IPCMessage {
        public readonly OpCode OpCode;
        public readonly byte[] Data;
        public int Length => this.Data.Length;
        public string Json => Encoding.UTF8.GetString(this.Data);
        
        public IPCMessage(OpCode opCode, byte[] data) {
            this.OpCode = opCode;
            this.Data = data;
        }
        
        public static IPCMessage Handshake(byte[] data) => new IPCMessage(OpCode.HANDSHAKE, data);
    }
    
    internal enum OpCode {
        HANDSHAKE, FRAME, CLOSE, PING, PONG
    }
}
