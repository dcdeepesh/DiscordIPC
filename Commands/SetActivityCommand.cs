using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetActivityCommand : ICommand<SetActivityCommand.Args> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public int? pid { get; set; }
            public Presence.Activity activity { get; set; }
        }

        // No data
    }
}
