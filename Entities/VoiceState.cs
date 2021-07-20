// Done
namespace Dec.DiscordIPC.Entities {
    public class VoiceState {
        public string guild_id { get; set; }
        public string channel_id { get; set; }
        public string user_id { get; set; }
        public Member member { get; set; }
        public string session_id { get; set; }
        public bool deaf { get; set; }
        public bool mute { get; set; }
        public bool self_deaf { get; set; }
        public bool self_mute { get; set; }
        public bool self_stream { get; set; }
        public bool self_video { get; set; }
        public bool suppress { get; set; }
        public string request_to_speak_timestamp { get; set; }
    }
}
