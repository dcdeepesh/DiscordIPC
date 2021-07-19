using Dec.DiscordIPC.Entities;
using System.Collections.Generic;

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
        }
    }
}
