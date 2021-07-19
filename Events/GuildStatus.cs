using Dec.DiscordIPC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Events {
    public class GuildStatus {
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public Guild guild { get; set; }
            public int online { get; set; }
        }
    }
}
