using System;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class GetGuilds {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public List<Guid> guilds { get; set; }
        }
    }
}
