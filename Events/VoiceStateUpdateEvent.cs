namespace Dec.DiscordIPC.Events {
    public class VoiceStateUpdateEvent : IEvent<VoiceStateUpdateEvent.Args> {
        public string Name => "VOICE_STATE_UPDATE";
        public Args Arguments { get; set; }

        public class Args : VoiceStateCreateEvent.Args { }

        public class Data : VoiceStateCreateEvent.Data { }
    }
}
