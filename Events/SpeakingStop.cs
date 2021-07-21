namespace Dec.DiscordIPC.Events {
    public class SpeakingStop {
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string user_id { get; set; }
        }
    }
}
