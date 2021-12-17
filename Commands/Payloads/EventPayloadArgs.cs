using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Commands.Payloads {
    public class EventPayloadArgs : EventPayload, IPayloadArgs {
        [JsonPropertyName("args")]
        public object Args { get; set; }
    }
}