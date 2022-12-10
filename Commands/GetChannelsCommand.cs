using System;
using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetChannelsCommand : ICommand<GetChannelsCommand.Args, GetChannelsCommand.Data> {
        
        public Args Arguments { get; set; }
        
        public static GetChannelsCommand Create(Action<Args> argsBuilder) {
            GetChannelsCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public List<Channel> channels { get; set; }
        }
    }
}
