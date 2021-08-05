using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class Authenticate {
        public class Args {
            public string access_token { get; set; }
        }

        public class Data {
            public User user { get; set; }
            public List<string> scopes { get; set; }
            public string expires { get; set; }
            public OAuth2AppStructure application { get; set; }

            public class OAuth2AppStructure {
                public string description { get; set; }
                public string icon { get; set; }
                public string id { get; set; }
                public List<string> rpc_origins { get; set; }
                public string name { get; set; }
            }
        }
    }
}
