using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class Authorize {
        public class Args {
            public string client_id { get; set; }
            public List<string> scopes { get; set; }
        }

        public class Data {
            public string code { get; set; }
        }
    }
}
