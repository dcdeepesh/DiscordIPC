using System;

namespace Dec.DiscordIPC.Events {
    public class VoiceStateDeleteEvent : IEvent<VoiceStateDeleteEvent.Args> {
        public string Name => "VOICE_STATE_DELETE";
        public Args Arguments { get; set; }

        public VoiceStateDeleteEvent Create(Action<Args> argsBuilder) {
            VoiceStateDeleteEvent theEvent = new() {
                Arguments = new Args()
            };
            argsBuilder(theEvent.Arguments);
            return theEvent;
        }
        
        public class Args : VoiceStateCreateEvent.Args { }

        public class Data : VoiceStateCreateEvent.Data { }
    }
}
