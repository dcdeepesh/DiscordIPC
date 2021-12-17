using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Commands.Payloads {
    public interface IPayloadArgs {
        [JsonPropertyName("args")]
        object Args { get; }
    }
}