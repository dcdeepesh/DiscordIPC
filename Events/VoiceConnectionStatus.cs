using System.Collections.Generic;

namespace Dec.DiscordIPC.Events {
    public class VoiceConnectionStatus {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string state { get; set; }
            public string hostname { get; set; }
            public List<int> pings { get; set; }
            public int average_ping { get; set; }
            public int last_ping { get; set; }
        }
    }
}
