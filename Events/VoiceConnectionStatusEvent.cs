using System.Collections.Generic;

namespace Dec.DiscordIPC.Events; 

public class VoiceConnectionStatusEvent : IEvent<VoiceConnectionStatusEvent.Args> {
    public string Name => "VOICE_CONNECTION_STATUS";
    public Args Arguments { get; set; }
        
    public static VoiceConnectionStatusEvent Create() => new();

    // No arguments; dummy
    public class Args { }

    public class Data {
        public string state { get; set; }
        public string hostname { get; set; }
        // "unreleased" problem (see docs)
        public List<float?> pings { get; set; }
        public float? average_ping { get; set; }
        public float? last_ping { get; set; }
    }
}