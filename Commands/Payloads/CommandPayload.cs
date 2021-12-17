using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Commands.Payloads {
    public class CommandPayload {
        [JsonPropertyName("cmd")]
        public string Command { get; set; }
        
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }
    }
}