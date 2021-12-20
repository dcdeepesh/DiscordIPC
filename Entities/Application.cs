using System.Collections.Generic;

namespace Dec.DiscordIPC.Entities {
    public class Application {
        public string id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public List<string> rpc_origins { get; set; }
        public bool? bot_public { get; set; }
        public bool? bot_require_code_grant { get; set; }
        public string terms_of_service_url { get; set; }
        public string privacy_policy_url { get; set; }
        public User owner { get; set; }
        public string summary { get; set; }
        public string verify_key { get; set; }
        public Team team { get; set; }
        public string guild_id { get; set; }
        public string primary_sku_id { get; set; }
        public string slug { get; set; }
        public string cover_image { get; set; }
        public int? flags { get; set; }

        public class Flags {
            public static readonly int
                GATEWAY_PRESENCE         = 1 << 12,
                GATEWAY_PRESENCE_LIMITED = 1 << 13,
                GATEWAY_GUILD_MEMBERS         = 1 << 14,
                GATEWAY_GUILD_MEMBERS_LIMITED = 1 << 15,
                VERIFICATION_PENDING_GUILD_LIMIT = 1 << 16,
                EMBEDDED = 1 << 17,
                GATEWAY_MESSAGE_CONTENT         = 1 << 18,
                GATEWAY_MESSAGE_CONTENT_LIMITED = 1 << 19;
        }
    }
}
