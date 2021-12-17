using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Mention {
        [JsonPropertyName("id")]
        public string ID { get; set; }
            
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
            
        [JsonPropertyName("type")]
        public int? Type { get; set; }
            
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}