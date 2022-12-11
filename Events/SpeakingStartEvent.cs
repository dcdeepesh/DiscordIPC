using System;

namespace Dec.DiscordIPC.Events; 

public class SpeakingStartEvent : IEvent<SpeakingStartEvent.Args> {
    public string Name => "SPEAKING_START";
    public Args Arguments { get; set; }
        
    public SpeakingStartEvent Create(Action<Args> argsBuilder) {
        SpeakingStartEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    public class Args {
        public string channel_id { get; set; }
    }

    public class Data {
        public string user_id { get; set; }
    }
}