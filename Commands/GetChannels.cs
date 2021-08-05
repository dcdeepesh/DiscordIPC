using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

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
