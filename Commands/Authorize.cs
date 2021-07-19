using System;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class Authorize : ICommand {
        public string nonce { get; set; } = Guid.NewGuid().ToString();
        public string cmd { get; } = "AUTHORIZE";
        public Args args { get; set; }

        public class Args : ICommandArgs {
            public string client_id { get; set; }
            public List<string> scopes { get; set; }
        }

        public class Response : IResponse {
            public string cmd { get; set; }
            public string nonce { get; set; }
            public Data data { get; set; }

            public class Data : IResponseData {
                public string code { get; set; }
            }
        }
    }
}
