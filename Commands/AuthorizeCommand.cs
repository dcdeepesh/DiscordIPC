using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class AuthorizeCommand {
        public class Args {
            public List<string> scopes { get; set; }
            public string client_id { get; set; }
            public string rpc_token { get; set; }
            public string username { get; set; }
        }

        public class Data {
            public string code { get; set; }
        }
    }
}
