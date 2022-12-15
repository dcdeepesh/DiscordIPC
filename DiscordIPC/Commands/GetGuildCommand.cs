using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands; 

public class GetGuildCommand : ICommand<GetGuildCommand.Args, GetGuildCommand.Data> {
        
    public string Name => "GET_GUILD";
    public Args Arguments { get; set; }
        
    public static GetGuildCommand Create(Action<Args> argsBuilder) {
        GetGuildCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string guild_id { get; set; }
        public int? timeout { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string id { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
        public List<Member> members { get; set; }
    }
}