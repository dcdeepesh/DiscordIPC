using System;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public class CloseActivityRequestCommand : ICommand<CloseActivityRequestCommand.Args> {
        
    public string Name => "CLOSE_ACTIVITY_REQUEST";
    public Args Arguments { get; set; }

    public static CloseActivityRequestCommand Create(Action<Args> argsBuilder) {
        CloseActivityRequestCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string user_id { get; set; }
    }

    // No data
}