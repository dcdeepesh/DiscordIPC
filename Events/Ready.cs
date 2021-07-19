using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class Ready {
        // No args because this isn't a subscription event

        public class Data {
            public int v { get; set; }
            public RPCServerConfig config { get; set; }
            public User user { get; set; }
        }
    }
}
