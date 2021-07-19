using Dec.DiscordIPC.Entities;
using System;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class Authenticate : ICommand {
        public string cmd { get; } = "AUTHENTICATE";
        public string nonce { get; set; } = Guid.NewGuid().ToString();
        public Args args { get; set; }

        public class Args : ICommandArgs {
            public string access_token { get; set; }
        }

        public class Response : IResponse {
            public string cmd { get; set; }
            public string nonce { get; set; }
            public Data data { get; set; }

            public class Data : IResponseData {
                public User user { get; set; }
                public List<string> scopes { get; set; }
                public string expires { get; set; }
                public OAuth2AppStructure application { get; set; }
            }
         }
    }
}
