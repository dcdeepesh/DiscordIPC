using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Entities.Objects;

namespace Dec.DiscordIPC.Entities {
    public class Embed {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("url")]
        public string URL { get; set; }
        
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        
        [JsonPropertyName("color")]
        public int? Color { get; set; }
        
        [JsonPropertyName("footer")]
        public Footer Footer { get; set; }
        
        [JsonPropertyName("image")]
        public Media Image { get; set; }
        
        [JsonPropertyName("thumbnail")]
        public Media Thumbnail { get; set; }
        
        [JsonPropertyName("video")]
        public Media Video { get; set; }
        
        [JsonPropertyName("provider")]
        public Provider Provider { get; set; }
        
        [JsonPropertyName("author")]
        public Author Author { get; set; }
        
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }
}
