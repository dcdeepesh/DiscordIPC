using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class Stage {
        /// <summary></summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("channel_id")]
        public string ChannelID { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("privacy_level")]
        public int? PrivacyLevel { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("discoverable_disabled")]
        public bool? DiscoverableDisabled { get; set; }
        
        public static class PrivacyLevels {
            public const int
                PUBLIC = 1,
                GUILD_ONLY = 2;
        }
    }
}