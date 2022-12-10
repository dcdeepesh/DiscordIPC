using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetGuildsCommand : ICommand<GetGuildsCommand.Args, GetGuildsCommand.Data>{
        
        public static Args Create() => new();
        
        // No arguments; dummy
        public class Args { }

        public class Data {
            public List<Guild> guilds { get; set; }
        }
    }
}
