namespace Dec.DiscordIPC.Events {
    public class VoiceChannelSelectEvent : IEvent<VoiceChannelSelectEvent.Args> {
        public string Name => "VOICE_CHANNEL_SELECT";
        public Args Arguments { get; set; }
        
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string channel_id { get; set; }
            public string guild_id { get; set; }
        }
    }
}
