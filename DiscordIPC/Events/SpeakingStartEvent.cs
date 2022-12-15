using System;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class SpeakingStartEvent : IEvent<SpeakingStartEvent.Args, SpeakingStartEvent.Data> {
    public string Name => "SPEAKING_START";
    public Args Arguments { get; set; }

    public bool IsMatchingData(Data _) => true;
    
    public static SpeakingStartEvent Create(Action<Args> argsBuilder) {
        SpeakingStartEvent theEvent = new() {
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
        public string user_id { get; set; }
    }
}