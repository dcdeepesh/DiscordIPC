using Dec.DiscordIPC.Entities;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class GetChannels {
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public List<Channel> channels { get; set; }
        }
    }
}
