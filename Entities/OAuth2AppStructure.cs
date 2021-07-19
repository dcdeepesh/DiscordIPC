using System.Collections.Generic;

namespace Dec.DiscordIPC.Entities {
    public class OAuth2AppStructure {
        public string description { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public List<string> rpc_origins { get; set; }
        public string name { get; set; }
    }
}
