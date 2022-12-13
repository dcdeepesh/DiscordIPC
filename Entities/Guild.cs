using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Entities; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Guild {
    public string id { get; set; }
    public string name { get; set; }
    public string icon { get; set; }
    public string icon_hash { get; set; }
    public string splash { get; set; }
    public string discovery_splash { get; set; }
    public bool? owner { get; set; }
    public string owner_id { get; set; }
    public string permissions { get; set; }
    public string region { get; set; }
    public string afk_channel_id { get; set; }
    public int? afk_timeout { get; set; }
    public bool? widget_enabled { get; set; }
    public string widget_channel_id { get; set; }
    public int? verification_level { get; set; }
    public int? default_message_notifications { get; set; }
    public int? explicit_content_filter { get; set; }
    public List<Role> roles { get; set; }
    public List<Emoji> emojis { get; set; }
    public List<string> features { get; set; }
    public int? mfa_level { get; set; }
    public string application_id { get; set; }
    public string system_channel_id { get; set; }
    public int? system_channel_flags { get; set; }
    public string rules_channel_id { get; set; }
    public string joined_at { get; set; }
    public bool? large { get; set; }
    public bool? unavailable { get; set; }
    public int? member_count { get; set; }
    public List<VoiceState> voice_states { get; set; }
    public List<Member> members { get; set; }
    public List<Channel> channels { get; set; }
    public List<Channel> threads { get; set; }
    public List<Presence> presences { get; set; }
    public int? max_presences { get; set; }
    public int? max_members { get; set; }
    public string vanity_url_code { get; set; }
    public string description { get; set; }
    public string banner { get; set; }
    public int? premium_tier { get; set; }
    public int? premium_subscription_count { get; set; }
    public string preferred_locale { get; set; }
    public string public_updates_channel_id { get; set; }
    public int? max_video_channel_users { get; set; }
    public int? approximate_member_count { get; set; }
    public int? approximate_presence_count { get; set; }
    public WelcomeScreen welcome_screen { get; set; }
    public int? nsfw_level { get; set; }
    public List<StageInstance> stage_instances { get; set; }
    public List<Sticker> stickers { get; set; }

    public class WelcomeScreen {
        public string description { get; set; }
        public List<Channel> welcome_channels { get; set; }

        public class Channel {
            public string channel_id { get; set; }
            public string description { get; set; }
            public string emoji_id { get; set; }
            public string emoji_name { get; set; }
        }
    }

    public class StageInstance {
        public string id { get; set; }
        public string guild_id { get; set; }
        public string channel_id { get; set; }
        public string topic { get; set; }
        public int? privacy_level { get; set; }
        public bool? discoverable_disabled { get; set; }

        public class PrivacyLevel {
            public static readonly int
                PUBLIC = 1,
                GUILD_ONLY = 2;
        }
    }
}