namespace Dec.DiscordIPC.Events {
    public class SpeakingStopEvent : IEvent<SpeakingStopEvent.Args> {
        public string Name => "SPEAKING_STOP";
        public Args Arguments { get; set; }
        
        public class Args : SpeakingStartEvent.Args { }

        public class Data : SpeakingStartEvent.Data { }
    }
}
