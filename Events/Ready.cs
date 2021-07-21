using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class Ready {
        // No args because this isn't a subscription event

        public class Data {
            public int v { get; set; }
            public RPCServerConfig config { get; set; }
            public User user { get; set; }

            public class RPCServerConfig {
                public string cdn_host { get; set; }
                public string api_endpoint { get; set; }
                public string environment { get; set; }
            }
        }
    }
}
