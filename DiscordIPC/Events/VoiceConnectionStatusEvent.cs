using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class VoiceConnectionStatusEvent : IEvent<object, VoiceConnectionStatusEvent.Data> {
    public string Name => "VOICE_CONNECTION_STATUS";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static VoiceConnectionStatusEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string state { get; set; }
        public string hostname { get; set; }
        // "unreleased" problem (see docs)
        public List<float?> pings { get; set; }
        public float? average_ping { get; set; }
        public float? last_ping { get; set; }
    }
}