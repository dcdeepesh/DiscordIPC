using System.Text;

namespace Dec.DiscordIPC.Core; 

internal class IpcRawPacket {
    public OpCode OpCode;
    public byte[] Data;
    
    public int Length => Data.Length;
    public string Json => Encoding.UTF8.GetString(Data);

    public IpcRawPacket(OpCode opCode, object data)
        : this(opCode, Dec.DiscordIPC.Core.Json.SerializeToBytes(data)) {
    }
        
    public IpcRawPacket(OpCode opCode, byte[] data) {
        OpCode = opCode;
        Data = data;
    }
}

internal enum OpCode {
     Handshake, Frame, Close, Ping, Pong
}