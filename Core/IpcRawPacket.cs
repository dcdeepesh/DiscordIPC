using System.Text;

namespace Dec.DiscordIPC.Core; 

internal class IpcRawPacket {
    public OpCode opCode;
    public byte[] data;
    public int Length => data.Length;
    public string Json => Encoding.UTF8.GetString(data);

    public IpcRawPacket(OpCode opCode, object data)
        : this(opCode, Dec.DiscordIPC.Core.Json.SerializeToBytes(data)) {
    }
        
    public IpcRawPacket(OpCode opCode, byte[] data) {
        this.opCode = opCode;
        this.data = data;
    }
}

internal enum OpCode {
    HANDSHAKE, FRAME, CLOSE, PING, PONG
}