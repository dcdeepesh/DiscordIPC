namespace Dec.DiscordIPC.Events {
    public class ActivitySpectateEvent : IEvent<ActivitySpectateEvent.Args> {
        public string Name => "ACTIVITY_SPECTATE";
        public Args Arguments { get; set; }
        
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string secret { get; set; }
        }
    }
}
