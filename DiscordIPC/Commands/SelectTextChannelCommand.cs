using System;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public class SelectTextChannelCommand : ICommand<SelectTextChannelCommand.Args, SelectTextChannelCommand.Data> {
        
    public string Name => "SELECT_TEXT_CHANNEL";
    public Args Arguments { get; set; }
        
    public static SelectTextChannelCommand Create(Action<Args> argsBuilder) {
        SelectTextChannelCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string channel_id { get; set; }
        public int? timeout { get; set; }
    }

    public class Data : GetChannelCommand.Data { }
}