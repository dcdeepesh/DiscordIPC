using System;
using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetChannelsCommand : ICommand<GetChannelsCommand.Args, GetChannelsCommand.Data> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public List<Channel> channels { get; set; }
        }
    }
}
