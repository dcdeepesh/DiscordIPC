using System;

namespace Dec.DiscordIPC.Events; 

public class SpeakingStopEvent : IEvent<SpeakingStopEvent.Args, SpeakingStopEvent.Data> {
    public string Name => "SPEAKING_STOP";
    public Args Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static SpeakingStopEvent Create(Action<Args> argsBuilder) {
        SpeakingStopEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    public class Args : SpeakingStartEvent.Args { }

    public class Data : SpeakingStartEvent.Data { }
}