using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands; 

public class GetChannelsCommand : ICommand<GetChannelsCommand.Args, GetChannelsCommand.Data> {
        
    public string Name => "GET_CHANNELS";
    public Args Arguments { get; set; }
        
    public static GetChannelsCommand Create(Action<Args> argsBuilder) {
        GetChannelsCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string guild_id { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public List<Channel> channels { get; set; }
    }
}