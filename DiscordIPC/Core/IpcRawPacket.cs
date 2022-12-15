using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core; 

public class IpcRawPacket {
    public OpCode OpCode { get; }
    public byte[] Data { get; }
    
    public int Length => Data.Length;
    public string Json => Encoding.UTF8.GetString(Data);

    public IpcRawPacket(OpCode opCode, object data) {
        OpCode = opCode;
        Data = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
        
    public IpcRawPacket(OpCode opCode, byte[] data) {
        OpCode = opCode;
        Data = data;
    }
}

public enum OpCode {
     Handshake, Frame, Close, Ping, Pong
}