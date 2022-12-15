using System;
using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class VoiceStateCreateEvent : IEvent<VoiceStateCreateEvent.Args, VoiceStateCreateEvent.Data> {
    public string Name => "VOICE_STATE_CREATE";
    public Args Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static VoiceStateCreateEvent Create(Action<Args> argsBuilder) {
        VoiceStateCreateEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string channel_id { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public VoiceState voice_state { get; set; }
        public User user { get; set; }
        public string nick { get; set; }
        public float? volume { get; set; }
        public bool? mute { get; set; }
        public Pan pan { get; set; }

        public class Pan {
            public float? left { get; set; }
            public float? right { get; set; }
        }
    }
}