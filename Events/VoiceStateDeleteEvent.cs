using System;

namespace Dec.DiscordIPC.Events; 

public class VoiceStateDeleteEvent : IEvent<VoiceStateDeleteEvent.Args, VoiceStateDeleteEvent.Data> {
    public string Name => "VOICE_STATE_DELETE";
    public Args Arguments { get; set; }

    public bool IsMatchingData(Data _) => true;

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