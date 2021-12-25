using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// </summary>
    public class User {
        /// <summary>The user's id</summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary>The user's username, not unique across the platform</summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        /// <summary>The user's 4-digit discord-tag</summary>
        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }
        
        /// <summary>The user's avatar hash</summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        
        /// <summary>Whether the user belongs to an OAuth2 application</summary>
        [JsonPropertyName("bot")]
        public bool? Bot { get; set; }
        
        /// <summary>Whether the user is an Official Discord System user (part of the urgent message system)</summary>
        [JsonPropertyName("system")]
        public bool? System { get; set; }
        
        /// <summary>Whether the user has two factor enabled on their account</summary>
        [JsonPropertyName("mfa_enabled")]
        public bool? MfaEnabled { get; set; }
        
        /// <summary>The user's banner hash</summary>
        [JsonPropertyName("banner")]
        public string Banner { get; set; }
        
        /// <summary>The user's banner color encoded as an integer representation of hexadecimal color code</summary>
        [JsonPropertyName("accent_color")]
        public int? AccentColor { get; set; }
        
        /// <summary>The user's chosen language option</summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
        
        /// <summary>Whether the email on this account has been verified</summary>
        [JsonPropertyName("verified")]
        public bool? Verified { get; set; }
        
        /// <summary>The user's email</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        /// <summary>The flags on a user's account</summary>
        [JsonPropertyName("flags")]
        public int? Flags { get; set; }
        
        /// <summary>The type of Nitro subscription on a user's account</summary>
        [JsonPropertyName("premium_type")]
        public int? PremiumType { get; set; }
        
        /// <summary>The public flags on a user's account</summary>
        [JsonPropertyName("public_flags")]
        public int? PublicFlags { get; set; }
    }
}
