using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Commands.Payloads {
    public class CommandPayloadArgs : CommandPayload, IPayloadArgs {
        [JsonPropertyName("args")]
        public object Args { get; set; }
    }
}