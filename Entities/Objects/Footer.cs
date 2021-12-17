using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Footer {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("icon_url")]
        public string IconURL { get; set; }
        
        [JsonPropertyName("proxy_icon_url")]
        public string ProxyIconURL { get; set; }
    }
}