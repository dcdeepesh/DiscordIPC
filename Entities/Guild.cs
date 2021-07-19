namespace Dec.DiscordIPC.Entities {
    public class Guild {
        public string id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string icon_hash { get; set; }
        public string splash { get; set; }
        public string discovery_splash { get; set; }
        public bool owner { get; set; }
        public string owner_id { get; set; }
        public string permissions { get; set; }
        public string region { get; set; }
        public string afk_channel_id { get; set; }
        public int afk_timeout { get; set; }
        public bool widget_enabled { get; set; }
        public string widget_channel_id { get; set; }
        public int verification_level { get; set; }
        public int default_message_notifications { get; set; }
        public int explicit_content_filter { get; set; }
        // roles
        // emojis
        // features
        public int mfa_level { get; set; }
        public string application_id { get; set; }
        public string system_channel_id { get; set; }
        public int system_channel_flags { get; set; }
        public string rules_channel_id { get; set; }
        // joined at
        public bool large { get; set; }
        public bool unavailable { get; set; }
        public int member_count { get; set; }
        // voice_stated
        // members
        // channels
        // threads
        // presences
        public int max_presences { get; set; }
        public int max_members { get; set; }
        public string vanity_url_code { get; set; }
        public string description { get; set; }
        public string banner { get; set; }
        public int premium_tier { get; set; }
        public int premium_subscription_count { get; set; }
        public string preferred_locale { get; set; }
        public string public_updates_channel_id { get; set; }
        public int max_video_channel_users { get; set; }
        public int approximate_member_count { get; set; }
        public int approximate_presence_count { get; set; }
        // welcome screen
        public int nsfw_level { get; set; }
        // state_instances
    }
}
