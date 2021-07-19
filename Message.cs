using System.Text;

namespace Dec.DiscordIPC {
    internal class Message {
        public OpCode opCode;
        public byte[] data;
        public int Length => data.Length;
        public string Json => Encoding.UTF8.GetString(data);

        public Message(OpCode opCode, byte[] data) {
            this.opCode = opCode;
            this.data = data;
        }

        public static Message Handshake(byte[] data) => new Message(OpCode.HANDSHAKE, data);
        public static Message Frame(byte[] data) => new Message(OpCode.FRAME, data);
        public static Message Close(byte[] data) => new Message(OpCode.CLOSE, data);
    }

    internal enum OpCode {
        HANDSHAKE, FRAME, CLOSE, PING, PONG
    }
}
