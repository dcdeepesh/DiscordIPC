using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetActivityCommand : ICommand<SetActivityCommand.Args> {
        
        public string Name => "SET_ACTIVITY";
        public Args Arguments { get; set; }
        
        public static SetActivityCommand Create(Action<Args> argsBuilder) {
            SetActivityCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args {
            public int? pid { get; set; }
            public Presence.Activity activity { get; set; }
        }

        // No data
    }
}
