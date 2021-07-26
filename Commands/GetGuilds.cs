using Dec.DiscordIPC.Entities;
using System;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class GetGuilds {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public List<Guild> guilds { get; set; }
        }
    }
}
