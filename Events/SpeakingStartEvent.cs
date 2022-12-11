namespace Dec.DiscordIPC.Events {
    public class SpeakingStartEvent : IEvent<SpeakingStartEvent.Args> {
        public string Name => "SPEAKING_START";
        public Args Arguments { get; set; }
        
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string user_id { get; set; }
        }
    }
}
