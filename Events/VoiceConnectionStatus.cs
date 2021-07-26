using System.Collections.Generic;

namespace Dec.DiscordIPC.Events {
    public class VoiceConnectionStatus {
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
}
