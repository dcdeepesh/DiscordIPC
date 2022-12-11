namespace Dec.DiscordIPC.Events {
    public class SpeakingStartEvent {
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string user_id { get; set; }
        }
    }
}
