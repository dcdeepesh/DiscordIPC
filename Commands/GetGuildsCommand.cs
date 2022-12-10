using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetGuildsCommand : ICommand<object, GetGuildsCommand.Data>{
        
        public string Name => "GET_GUILDS";
        // No arguments; dummy
        public object Arguments { get; set; }
        
        public static GetGuildsCommand Create() => new();

        public class Data {
            public List<Guild> guilds { get; set; }
        }
    }
}
