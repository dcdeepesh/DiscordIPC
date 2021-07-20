using System.Text;

namespace Dec.DiscordIPC.Core {
    internal class IPCMessage {
        public OpCode opCode;
        public byte[] data;
        public int Length => data.Length;
        public string Json => Encoding.UTF8.GetString(data);

        public IPCMessage(OpCode opCode, byte[] data) {
            this.opCode = opCode;
            this.data = data;
        }

        public static IPCMessage Handshake(byte[] data) => new IPCMessage(OpCode.HANDSHAKE, data);
        public static IPCMessage Frame(byte[] data) => new IPCMessage(OpCode.FRAME, data);
        public static IPCMessage Close(byte[] data) => new IPCMessage(OpCode.CLOSE, data);
    }

    internal enum OpCode {
        HANDSHAKE, FRAME, CLOSE, PING, PONG
    }
}
