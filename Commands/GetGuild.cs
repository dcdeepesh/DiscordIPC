using Dec.DiscordIPC.Entities;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class GetGuild {
        public class Args {
            public string guild_id { get; set; }
            public int timeout { get; set; }
        }

        public class Data {
            public string id { get; set; }
            public string name { get; set; }
            public string icon_url { get; set; }
            public List<Member> members { get; set; }
        }
    }
}
