using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class Application {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("rpc_origins")]
        public List<string> RPCOrigins { get; set; }
        
        [JsonPropertyName("bot_public")]
        public bool? BotPublic { get; set; }
        
        [JsonPropertyName("bot_require_code_grant")]
        public bool? BotRequireCodeGrant { get; set; }
        
        [JsonPropertyName("terms_of_service_url")]
        public string TermsOfServiceURL { get; set; }
        
        [JsonPropertyName("privacy_policy_url")]
        public string PrivacyPolicyURL { get; set; }
        
        [JsonPropertyName("owner")]
        public User Owner { get; set; }
        
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        
        [JsonPropertyName("verify_key")]
        public string VerifyKey { get; set; }
        
        [JsonPropertyName("team")]
        public Team Team { get; set; }
        
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        [JsonPropertyName("primary_sku_id")]
        public string PrimarySkuID { get; set; }
        
        [JsonPropertyName("slug")]
        public string Slug { get; set; }
        
        [JsonPropertyName("cover_image")]
        public string CoverImage { get; set; }
        
        [JsonPropertyName("flags")]
        public int? Flags { get; set; }
        
        public class FlagTypes {
            public static readonly int
                GATEWAY_PRESENCE = 1 << 12,
                GATEWAY_PRESENCE_LIMITED = 1 << 13,
                GATEWAY_GUILD_MEMBERS = 1 << 14,
                GATEWAY_GUILD_MEMBERS_LIMITED = 1 << 15,
                VERIFICATION_PENDING_GUILD_LIMIT = 1 << 16,
                EMBEDDED = 1 << 17;
        }
    }
}
