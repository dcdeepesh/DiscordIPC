using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Entities.Objects;

namespace Dec.DiscordIPC.Entities {
    public class Presence {
        [JsonPropertyName("user")]
        public User User { get; set; }
        
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("activities")]
        public List<Activity> Activities { get; set; }
        
        [JsonPropertyName("client_status")]
        public ClientStatus ClientStatus { get; set; }
        
        public class Activity {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("type")]
            public int? Type { get; set; }
            
            [JsonPropertyName("url")]
            public string URL { get; set; }
            
            [JsonPropertyName("created_at")]
            public int? CreatedAt { get; set; }
            
            [JsonPropertyName("timestamps")]
            public Timestamps Timestamps { get; set; }
            
            [JsonPropertyName("application_id")]
            public string ApplicationID { get; set; }
            
            [JsonPropertyName("details")]
            public string Details { get; set; }
            
            [JsonPropertyName("state")]
            public string State { get; set; }
            
            [JsonPropertyName("emoji")]
            public Emoji Emoji { get; set; }
            
            [JsonPropertyName("party")]
            public Party Party { get; set; }
            
            [JsonPropertyName("assets")]
            public Assets Assets { get; set; }
            
            [JsonPropertyName("secrets")]
            public Secrets Secrets { get; set; }
            
            [JsonPropertyName("instance")]
            public bool? Instance { get; set; }
            
            [JsonPropertyName("flags")]
            public int? Flags { get; set; }
            
            [JsonPropertyName("buttons")]
            public List<Button> Buttons { get; set; }
        }
        public class Timestamps {
            [JsonPropertyName("start")]
            public int? Start { get; set; }
            
            [JsonPropertyName("end")]
            public int? End { get; set; }
        }
        public class Emoji {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("animated")]
            public bool? Animated { get; set; }
        }
        public class Party {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("size")]
            public List<int?> Size { get; set; }
            
            [JsonPropertyName("current_size")]
            public int? CurrentSize => this.Size[0];
            
            [JsonPropertyName("max_size")]
            public int? MaxSize => this.Size[1];
        }
        public class Assets {
            [JsonPropertyName("large_image")]
            public string LargeImage { get; set; }
            
            [JsonPropertyName("large_text")]
            public string LargeText { get; set; }
            
            [JsonPropertyName("small_image")]
            public string SmallImage { get; set; }
            
            [JsonPropertyName("small_text")]
            public string SmallText { get; set; }
        }
        public class Secrets {
            [JsonPropertyName("join")]
            public string Join { get; set; }
            
            [JsonPropertyName("spectate")]
            public string Spectate { get; set; }
            
            [JsonPropertyName("match")]
            public string Match { get; set; }
        }
        public class Button {
            [JsonPropertyName("label")]
            public string Label { get; set; }
            
            [JsonPropertyName("url")]
            public string URL { get; set; }
        }
    }
}
