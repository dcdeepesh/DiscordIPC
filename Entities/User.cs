using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// </summary>
    public class User {
        /// <summary></summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("user")]
        public string Username { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("bot")]
        public bool? Bot { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("system")]
        public bool? System { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("mfa_enabled")]
        public bool? MfaEnabled { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("verified")]
        public bool? Verified { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("flags")]
        public int? Flags { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("premium_type")]
        public int? PremiumType { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("public_flags")]
        public int? PublicFlags { get; set; }
    }
}
