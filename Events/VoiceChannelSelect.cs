namespace Dec.DiscordIPC.Events {
    public class VoiceChannelSelect {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string channel_id { get; set; }
            public string guild_id { get; set; }
        }
    }
}
