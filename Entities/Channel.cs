using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// Represents a guild or DM channel within Discord.
    /// </summary>
    public class Channel {
        /// <summary>The ID of this channel</summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary>The <see cref="Types">type of channel</see></summary>
        [JsonPropertyName("type")]
        public int? Type { get; set; }
        
        /// <summary>The id of the guild (May be `null` for some channel objects received over gateway guild dispatches)</summary>
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        /// <summary>Sorting position of the channel</summary>
        [JsonPropertyName("position")]
        public int? Position { get; set; }
        
        /// <summary>The name of the channel (1-100 characters)</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>The channel topic (0-1024 characters)</summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        
        /// <summary>Whether the channel is nsfw</summary>
        [JsonPropertyName("nsfw")]
        public bool Nsfw { get; set; }
        
        /// <summary>The ID of the last message sent in this channel (may not point to an existing or valid message)</summary>
        [JsonPropertyName("last_message_id")]
        public string LastMessageID { get; set; }
        
        /// <summary>The bitrate (in bits) of the voice channel</summary>
        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; set; }
        
        /// <summary>The user limit of the voice channel</summary>
        [JsonPropertyName("user_limit")]
        public int? UserLimit { get; set; }
        
        /// <summary>Amount of seconds a user has to wait before sending another message (0-21600); bots, as well as users with the permission manage_messages or manage_channel, are unaffected</summary>
        [JsonPropertyName("rate_limit_per_user")]
        public int RateLimitPerUser { get; set; }
        
        /// <summary>The recipients of the DM</summary>
        [JsonPropertyName("recipients")]
        public List<User> Recipients { get; set; }
        
        /// <summary>Icon Hash</summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        
        /// <summary>ID of the creator of the group DM or thread</summary>
        [JsonPropertyName("owner_id")]
        public string OwnerID { get; set; }
        
        /// <summary>Application id of the group DM creator if it is bot-created</summary>
        [JsonPropertyName("application_id")]
        public string ApplicationID { get; set; }
        
        /// <summary>For guild channels: id of the parent category for a channel (each parent category can contain up to 50 channels), for threads: id of the text channel this thread was created</summary>
        [JsonPropertyName("parent_id")]
        public string ParentID { get; set; }
        
        /// <summary>When the last pinned message was pinned. This may be `null` in events such as `GUILD_CREATE` when a message is not pinned.</summary>
        [JsonPropertyName("last_pin_timestamp")]
        public string LastPinTimestamp { get; set; }
        
        /// <summary><see>Voice region</see> ID for the voice channel, `automatic` when set to `null`</summary>
        [JsonPropertyName("rtc_region")]
        public string RTCRegion { get; set; }
        
        /// <summary>The camera video quality mode of the voice channel, `1` when not present</summary>
        [JsonPropertyName("video_quality_mode")]
        public int? VideoQualityMode { get; set; }
        
        /// <summary>An approximate count of messages in a thread, stops counting at 50</summary>
        [JsonPropertyName("message_count")]
        public int? MessageCount { get; set; }
        
        /// <summary>An approximate count of users in a thread, stops counting at 50</summary>
        [JsonPropertyName("member_count")]
        public int? MemberCount { get; set; }
        
        /// <summary>Thread-specific fields not needed by other channels</summary>
        [JsonPropertyName("thread_metadata")]
        public Thread.Metadata ThreadMetadata { get; set; }
        
        /// <summary>Thread member object for the current user, if they have joined the thread, only included on certain API endpoints</summary>
        [JsonPropertyName("member")]
        public Thread.Member Member { get; set; }
        
        /// <summary>Default duration that the clients (not the API) will use for newly created threads, in minutes, to automatically archive the thread after recent activity, can be set to: 60, 1440, 4320, 10080</summary>
        [JsonPropertyName("default_auto_archive_duration")]
        public int? DefaultAutoArchiveDuration { get; set; }
        
        /// <summary>Computed permissions for the invoking user in the channel, including overwrites, only included when part of the resolved data received on a slash command interaction</summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }
        
        public static class Types {
            public const int
                GUILD_TEXT = 0,
                DM = 1,
                GUILD_VOICE = 2,
                GROUP_DM = 3,
                GUILD_CATEGORY = 4,
                GUILD_NEWS = 5,
                GUILD_STORE = 6,
                GUILD_NEWS_THREAD = 10,
                GUILD_PUBLIC_THREAD = 11,
                GUILD_PRIVATE_THREAD = 12,
                GUILD_STAGE_VOICE = 13;
        }
    }
}
