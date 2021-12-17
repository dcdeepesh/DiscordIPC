using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Commands.Payloads {
    public class EventPayload : CommandPayload {
        [JsonPropertyName("evt")]
        public string Event { get; set; }
    }
}