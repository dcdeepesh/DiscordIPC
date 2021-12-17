using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public static class Thread {
        public class Metadata {
            /// <summary>Whether the thread is archived</summary>
            [JsonPropertyName("archived")]
            public bool Archived { get; set; }
            
            /// <summary>Duration in minutes to automatically archive the thread after recent activity, can be set to: `60`, `1440`, `4320`, `10080`</summary>
            [JsonPropertyName("auto_archive_duration")]
            public int AutoArchiveDuration { get; set; }
            
            /// <summary>Timestamp when the thread's archive status was last changed, used for calculating recent activity</summary>
            [JsonPropertyName("archive_timestamp")]
            public string ArchivedTimestamp { get; set; }
            
            /// <summary>Whether the thread is locked; when a thread is locked, only users with `MANAGE_THREADS` can unarchive it</summary>
            [JsonPropertyName("locked")]
            public bool Locked { get; set; }
            
            /// <summary>Whether non-moderators can add other non-moderators to a thread; only available on private threads</summary>
            [JsonPropertyName("invitable")]
            public bool? Invitable { get; set; }
        }
        public class Member {
            /// <summary>The id of the thread</summary>
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            /// <summary>The ID of the user</summary>
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
            
            /// <summary>The time the current user last joined the thread</summary>
            [JsonPropertyName("join_timestamp")]
            public string JoinTimestamp { get; set; }
            
            /// <summary>Any user-thread settings, currently only used for notifications</summary>
            [JsonPropertyName("flags")]
            public int Flags { get; set; }
        }
    }
}