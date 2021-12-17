using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class ScheduledEvent {
        /// <summary>The id of the scheduled event</summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        /// <summary>The guild id which the scheduled event belongs to</summary>
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        /// <summary>The channel id in which the scheduled event will be hosted, or `null` if `EntityType` is `EXTERNAL`</summary>
        [JsonPropertyName("channel_id")]
        public string ChannelID { get; set; }
        
        /// <summary>
        /// The id of the user that created the scheduled event
        /// Will be `null` for events created before `October 25th, 2021`, when `CreatorID` was introduced and tracked.
        /// </summary>
        [JsonPropertyName("creator_id")]
        public string CreatorID { get; set; }
        
        /// <summary>The name of the scheduled event (1-100 characters)</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>The description of the scheduled event (1-1000 characters)</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        /// <summary>The time the scheduled event will start</summary>
        [JsonPropertyName("scheduled_start_time")]
        public string ScheduledStartTime { get; set; }
        
        /// <summary>The time the scheduled event will end, required if `EntityType` is `EXTERNAL`</summary>
        [JsonPropertyName("scheduled_end_time")]
        public string ScheduledEndTime { get; set; }
        
        /// <summary>The <see cref="PrivacyLevels">privacy level</see> of the scheduled event</summary>
        [JsonPropertyName("privacy_level")]
        public int? PrivacyLevel { get; set; }
        
        /// <summary>
        /// The <see>status</see> of the scheduled event
        /// Once `Status` is set to `COMPLETED` or `CANCELED`, the `Status` can no longer be updated
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }
        
        /// <summary>The <see cref="EntityTypes">type</see> of the scheduled event</summary>
        [JsonPropertyName("entity_type")]
        public int? EntityType { get; set; }
        
        /// <summary>The ID of an entity associated with a guild scheduled event</summary>
        [JsonPropertyName("entity_id")]
        public string EntityId { get; set; }
        
        /// <summary>Additional metadata for the guild scheduled event</summary>
        [JsonPropertyName("entity_metadata")]
        public object EntityMetadata { get; set; }
        
        /// <summary>
        /// The user that created the scheduled event
        /// Will be `null` for events created before `October 25th, 2021`, when `Creator` was introduced and tracked.
        /// </summary>
        [JsonPropertyName("creator")]
        public User Creator { get; set; }
        
        /// <summary>The number of users subscribed to the scheduled event</summary>
        [JsonPropertyName("user_count")]
        public int? UserCount { get; set; }
        
        public static class PrivacyLevels {
            public const int
                GUILD_ONLY = 2;
        }
        public static class Statuses {
            public const int
                SCHEDULED = 1,
                ACTIVE = 2,
                COMPLETED = 3,
                CANCELLED = 4;
        }
        public static class EntityTypes {
            public const int
                STAGE_INSTANCE = 1,
                VOICE = 2,
                EXTERNAL = 3;
        }
    }
}