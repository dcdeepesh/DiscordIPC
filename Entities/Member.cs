using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class Member {
        /// <summary></summary>
        [JsonPropertyName("user")]
        public User User { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("nick")]
        public string Nick { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("joined_at")]
        public string JoinedAt { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("premium_since")]
        public string PremiumSince { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("deaf")]
        public bool? Deaf { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("mute")]
        public bool? Mute { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("pending")]
        public bool? Pending { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }
    }
}
