using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core.Ipc;

public class Packet {

    public OpCode OpCode { get; }
    public byte[] Payload { get; }

    public int PayloadLength => Payload.Length;
    public string PayloadJson => Encoding.UTF8.GetString(Payload);

    public Packet(OpCode opCode, object data) {
        OpCode = opCode;
        Payload = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }

    public Packet(OpCode opCode, byte[] payloadData) {
        OpCode = opCode;
        Payload = payloadData;
    }
}