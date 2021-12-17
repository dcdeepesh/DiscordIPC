using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class Role {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("color")]
        public int? Color { get; set; }
        
        [JsonPropertyName("host")]
        public bool? Hoist { get; set; }
        
        [JsonPropertyName("position")]
        public int? Position { get; set; }
        
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }
        
        [JsonPropertyName("managed")]
        public bool? Managed { get; set; }
        
        [JsonPropertyName("mentionable")]
        public bool? Mentionable { get; set; }
        
        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }

        public class Tag {
            [JsonPropertyName("bot_id")]
            public string BotID { get; set; }
            
            [JsonPropertyName("integration_id")]
            public string IntegrationID { get; set; }
            
            [JsonPropertyName("premium_subscriber")]
            public object PremiumSubscriber { get; set; }
        }
    }
}
