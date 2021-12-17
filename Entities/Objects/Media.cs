using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Media {
        [JsonPropertyName("url")]
        public string URL { get; set; }
        
        [JsonPropertyName("proxy_url")]
        public string ProxyURL { get; set; }
        
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        
        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }
}