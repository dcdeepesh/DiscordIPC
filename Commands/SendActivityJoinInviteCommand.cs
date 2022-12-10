using System;

namespace Dec.DiscordIPC.Commands {
    public class SendActivityJoinInviteCommand : ICommand<SendActivityJoinInviteCommand.Args> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string user_id { get; set; }
        }

        // No data
    }
}
