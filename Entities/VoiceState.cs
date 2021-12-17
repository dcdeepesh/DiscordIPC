using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// Used to represent a user's voice connection status.
    /// </summary>
    public class VoiceState {
        /// <summary>Whether this user is deafened by the server</summary>
        [JsonPropertyName("deaf")]
        public bool? Deaf { get; set; }
        
        /// <summary>Whether this user is muted by the server</summary>
        [JsonPropertyName("mute")]
        public bool? Mute { get; set; }
        
        /// <summary>Whether this user is locally deafened</summary>
        [JsonPropertyName("self_deaf")]
        public bool? SelfDeaf { get; set; }
        
        /// <summary>Whether this user is locally muted</summary>
        [JsonPropertyName("self_mute")]
        public bool? SelfMute { get; set; }
        
        /// <summary>Whether this user is streaming using "Go Live"</summary>
        [JsonPropertyName("self_stream")]
        public bool? SelfStream { get; set; }
        
        /// <summary>Whether this user's camera is enabled</summary>
        [JsonPropertyName("self_video")]
        public bool? SelfVideo { get; set; }
        
        /// <summary>Whether this user is muted by the current user</summary>
        [JsonPropertyName("suppress")]
        public bool? Suppress { get; set; }
        
        /// <summary>The time at which the user requested to speak</summary>
        [JsonPropertyName("request_to_speak_timestamp")]
        public string RequestToSpeakTimestamp { get; set; }
    }
}
