using System;

namespace Dec.DiscordIPC.Events; 

public class VoiceStateUpdateEvent : IEvent<VoiceStateUpdateEvent.Args> {
    public string Name => "VOICE_STATE_UPDATE";
    public Args Arguments { get; set; }
        
    public VoiceStateUpdateEvent Create(Action<Args> argsBuilder) {
        VoiceStateUpdateEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }

    public class Args : VoiceStateCreateEvent.Args { }

    public class Data : VoiceStateCreateEvent.Data { }
}