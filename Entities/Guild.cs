using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Entities.Objects;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// Guilds in Discord represent an isolated collection of users and channels, and are often referred to as "servers" in the UI.
    /// </summary>
    public class Guild {
        /// <summary>Guild ID</summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary>Guild name (2-100 characters, excluding trailing and leading whitespace)</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>Icon Hash</summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        
        /// <summary>Icon hash, returned when in the template object</summary>
        [JsonPropertyName("icon_hash")]
        public string IconHash { get; set; }
        
        /// <summary>Splash hash</summary>
        [JsonPropertyName("splash")]
        public string Splash { get; set; }
        
        /// <summary>Discovery splash hash; only present for guilds with the "DISCOVERABLE" feature</summary>
        [JsonPropertyName("discovery_splash")]
        public string DiscoverySplash { get; set; }
        
        /// <summary>True if the user is the owner of the guild</summary>
        [JsonPropertyName("owner")]
        public bool? Owner { get; set; }
        
        /// <summary>ID of owner</summary>
        [JsonPropertyName("owner_id")]
        public string OwnerID { get; set; }
        
        /// <summary>Total permissions for the user in the guild (excludes overwrites)</summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }
        
        /// <summary>Voice region id for the guild (deprecated)</summary>
        [JsonPropertyName("region"), Obsolete]
        public string Region { get; set; }
        
        /// <summary>ID of AFK channel</summary>
        [JsonPropertyName("afk_channel_id")]
        public string AfkChannelID { get; set; }
        
        /// <summary>AFK timeout in seconds</summary>
        [JsonPropertyName("afk_timeout")]
        public int? AfkTimeout { get; set; }
        
        /// <summary>True if thte server widget is enabled</summary>
        [JsonPropertyName("widget_enabled")]
        public bool? WidgetEnabled { get; set; }
        
        /// <summary>The channel ID that the widget will generate an invite to, or `null` if set to no invite</summary>
        [JsonPropertyName("widget_channel_id")]
        public string WidgetChannelID { get; set; }
        
        /// <summary><see cref="VerificationLevels">Verification level</see> required for the guild</summary>
        [JsonPropertyName("verification_level")]
        public int? VerificationLevel { get; set; }
        
        /// <summary>Default <see cref="MessageNotificationLevels">message notifications level</see></summary>
        [JsonPropertyName("default_message_notifications")]
        public int? DefaultMessageNotifications { get; set; }
        
        /// <summary><see cref="ExplicitContentFilterLevels">Explicit content filter level</see></summary>
        [JsonPropertyName("explicit_content_filter")]
        public int? ExplicitContentFilter { get; set; }
        
        /// <summary>Roles in the guild</summary>
        [JsonPropertyName("roles")]
        public List<Role> Roles { get; set; }
        
        /// <summary>Custom guild emojis</summary>
        [JsonPropertyName("emojis")]
        public List<Emoji> Emojis { get; set; }
        
        /// <summary>Enabled <see cref="GuildFeatures">guild features</see></summary>
        [JsonPropertyName("features")]
        public List<string> Features { get; set; }
        
        /// <summary>Required <see cref="MultiFactorAuthenticationLevels">Multi-Factor authentication level</see> for the guild</summary>
        [JsonPropertyName("mfa_level")]
        public int? MfaLevel { get; set; }
        
        /// <summary>Application ID of the guild creator if it is bot-created</summary>
        [JsonPropertyName("application_id")]
        public string ApplicationID { get; set; }
        
        /// <summary>The ID of the channel where guild notices such as welcome messages and boost events are posted</summary>
        [JsonPropertyName("system_channel_id")]
        public string SystemChannelID { get; set; }
        
        /// <summary><see cref="SystemChannelFlags">System channel flags</see></summary>
        [JsonPropertyName("system_channel_flags")]
        public int? SystemChannelFlags { get; set; }
        
        /// <summary>The ID of the channel where Community Guilds can display rules and/or guidelines</summary>
        [JsonPropertyName("rules_channel_id")]
        public string RulesChannelID { get; set; }
        
        /// <summary>When this guild was joined at</summary>
        [JsonPropertyName("joined_at")]
        public string JoinedAt { get; set; }
        
        /// <summary>True if this is considered a large guild</summary>
        [JsonPropertyName("large")]
        public bool? Large { get; set; }
        
        /// <summary>True if this guild is unavailable due to an outage</summary>
        [JsonPropertyName("unavailable")]
        public bool? Unavailable { get; set; }
        
        /// <summary>Total number of members in this guild</summary>
        [JsonPropertyName("member_count")]
        public int? MemberCount { get; set; }
        
        /// <summary>States of members currently in voice channels; lacks the `guild_id` key</summary>
        [JsonPropertyName("voice_states")]
        public List<VoiceState> VoiceStates { get; set; }
        
        /// <summary>Users in the guild</summary>
        [JsonPropertyName("members")]
        public List<Member> Members { get; set; }
        
        /// <summary>Channels in the guild</summary>
        [JsonPropertyName("channels")]
        public List<Channel> Channels { get; set; }
        
        /// <summary>All active threads in the guild that current user has permission to view</summary>
        [JsonPropertyName("threads")]
        public List<Channel> Threads { get; set; }
        
        /// <summary>Presences of the members in the guild, will only include non-offline members if the size is greater than `large threshold`</summary>
        [JsonPropertyName("presences")]
        public List<Presence> Presences { get; set; }
        
        /// <summary>The maxmimum number of presences for the guild (`null` is always returned, apart from the largest of guilds)</summary>
        [JsonPropertyName("max_presences")]
        public int? MaxPresences { get; set; }
        
        /// <summary>The maximum number of members for the guild</summary>
        [JsonPropertyName("max_members")]
        public int? MaxMembers { get; set; }
        
        /// <summary>The vanity URL code for the guild</summary>
        [JsonPropertyName("vanity_url_code")]
        public string VanityURLCode { get; set; }
        
        /// <summary>The description of a Community guild</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        /// <summary>Banner hash</summary>
        [JsonPropertyName("banner")]
        public string Banner { get; set; }
        
        /// <summary><see cref="PremiumTiers">Premium tier</see> (Server Boost Level)</summary>
        [JsonPropertyName("premium_tier")]
        public int? PremiumTier { get; set; }
        
        /// <summary>The number of boosts this guild currently has</summary>
        [JsonPropertyName("premium_subscription_count")]
        public int? PremiumSubscriptionCount { get; set; }
        
        /// <summary>The preferred locale of a Community guild; used in server discovery and notices from Discord; defaults to "en-US"</summary>
        [JsonPropertyName("preferred_locale")]
        public string PreferredLocale { get; set; }
        
        /// <summary>The id of the channel where admins and moderators of Community guilds receive notices from Discord</summary>
        [JsonPropertyName("public_updates_channel_id")]
        public string PublicUpdatesChannelID { get; set; }
        
        /// <summary>The maximum amount of users in a video channel</summary>
        [JsonPropertyName("max_video_channel_users")]
        public int? MaxVideoChannelUsers { get; set; }
        
        /// <summary>Approximate number of members in this guild, returned from the `GET /guilds/&lt;id&gt;` endpoint when `with_counts` is `true`</summary>
        [JsonPropertyName("approximate_member_count")]
        public int? ApproximateMemberCount { get; set; }
        
        /// <summary>Approximate number of non-offline members in this guild, returned from the `GET /guilds/&lt;id&gt;` endpoint when `with_counts` is `true`</summary>
        [JsonPropertyName("approximate_presence_count")]
        public int? ApproximatePresenceCount { get; set; }
        
        /// <summary>The welcome screen of a Community guild, shown to new members, returned in an Invite's guild object</summary>
        [JsonPropertyName("welcome_screen")]
        public WelcomeScreen WelcomeScreen { get; set; }
        
        /// <summary><see cref="NsfwLevels">Guild NSFW level</see></summary>
        [JsonPropertyName("nsfw_level")]
        public int? NsfwLevel { get; set; }
        
        /// <summary>Stage instances in the guild</summary>
        [JsonPropertyName("stage_instances")]
        public List<Stage> StageInstances { get; set; }
        
        /// <summary>Custom guild stickers</summary>
        [JsonPropertyName("stickers")]
        public List<Sticker> Stickers { get; set; }
        
        /// <summary>The scheduled events in the guild</summary>
        [JsonPropertyName("guild_scheduled_events")]
        public List<ScheduledEvent> ScheduledEvents { get; set; }
        
        /// <summary>Whether the guild has the boost progress bar enabled</summary>
        [JsonPropertyName("premium_progress_bar_enabled")]
        public bool? PremiumProgressBarEnabled { get; set; }
        
        public static class MessageNotificationLevels {
            public const int
                ALL_MESSAGES = 0,
                ONLY_MENTIONS = 1;
        }
        public static class ExplicitContentFilterLevels {
            public const int
                DISABLED = 0,
                MEMBERS_WITHOUT_ROLES = 1,
                ALL_MEMBERS = 2;
        }
        public static class MultiFactorAuthenticationLevels {
            public const int
                NONE = 0,
                ELEVATED = 1;
        }
        public static class VerificationLevels {
            public const int
                NONE = 0,
                LOW = 1,
                MEDIUM = 2,
                HIGH = 3,
                VERY_HIGH = 4;
        }
        public static class NsfwLevels {
            public const int
                DEFAULT = 0,
                EXPLICIT = 1,
                SAFE = 2,
                AGE_RESTRICTED = 3;
        }
        public static class PremiumTiers {
            public const int
                NONE = 0,
                TIER_1 = 1,
                TIER_2 = 2,
                TIER_3 = 3;
        }
        public enum GuildFeatures {
            /// <summary>Has access to set an animated guild icon</summary>
            ANIMATED_ICON,
            /// <summary>Has access to set a guild banner image</summary>
            BANNER,
            /// <summary>Has access to use commerce features (i.e. create store channels)</summary>
            COMMERCE,
            /// <summary>Can enable welcome screen, Membership Screening, stage channels and discovery, and receives community updates</summary>
            COMMUNITY,
            /// <summary>Is able to be discovered in the directory</summary>
            DISCOVERABLE,
            /// <summary>Is able to be featured in the directory</summary>
            FEATURABLE,
            /// <summary>Has access to set an invite splash background</summary>
            INVITE_SPLASH,
            /// <summary>Has enabled Membership Screening</summary>
            MEMBER_VERIFICATION_GATE_ENABLED,
            /// <summary>Has enabled monetization</summary>
            MONETIZATION_ENABLED,
            /// <summary>Has increased custom sticker slots</summary>
            MORE_STICKERS,
            /// <summary>Has access to create news channels</summary>
            NEWS,
            /// <summary>Is partnered</summary>
            PARTNERED,
            /// <summary>Can be previewed before joining via Membership Screening or the directory</summary>
            PREVIEW_ENABLED,
            /// <summary>Has access to create private threads</summary>
            PRIVATE_THREADS,
            /// <summary>Is able to set role icons</summary>
            ROLE_ICONS,
            /// <summary>Has access to the seven day archive time for threads</summary>
            SEVEN_DAY_THREAD_ARCHIVE,
            /// <summary>Has access to the three day archive time for threads</summary>
            THREE_DAY_THREAD_ARCHIVE,
            /// <summary>Has enabled ticketed events</summary>
            TICKETED_EVENTS_ENABLED,
            /// <summary>Has access to set a vanity URL</summary>
            VANITY_URL,
            /// <summary>Is verified</summary>
            VERIFIED,
            /// <summary>Has access to set 384kbps bitrate in voice (previously VIP voice servers)</summary>
            VIP_REGIONS,
            /// <summary>Has enabled the welcome screen</summary>
            WELCOME_SCREEN_ENABLED
        }
    }
}
