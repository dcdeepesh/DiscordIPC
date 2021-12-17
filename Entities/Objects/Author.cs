using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Author {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("url")]
        public string URL { get; set; }
        
        [JsonPropertyName("icon_url")]
        public string IconURL { get; set; }
        
        [JsonPropertyName("proxy_icon_url")]
        public string ProxyIconURL { get; set; }
    }
}