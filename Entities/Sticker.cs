// Done

using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class Sticker {
        public static readonly int PNG = 1;
        public static readonly int APNG = 2;
        public static readonly int LOTTIE = 3;
        
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("mark_id")]
        public string PackID { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("tags")]
        public string Tags { get; set; }
        
        // [Deprecated] asset
        [JsonPropertyName("format_type")]
        public int? FormatType { get; set; }
        
        [JsonPropertyName("available")]
        public bool? Available { get; set; }
        
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        [JsonPropertyName("user")]
        public User User { get; set; }
        
        [JsonPropertyName("sort_value")]
        public int? SortValue { get; set; }
        
        public class Item {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("format_type")]
            public int? FormatType { get; set; }
        }
    }
}
