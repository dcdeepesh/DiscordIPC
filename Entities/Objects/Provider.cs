using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Provider {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("url")]
        public string URL { get; set; }
    }
}