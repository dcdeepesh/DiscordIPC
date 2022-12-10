using System;

namespace Dec.DiscordIPC.Commands {
    public class SendActivityJoinInviteCommand : ICommand<SendActivityJoinInviteCommand.Args> {
        
        public string Name => "SEND_ACTIVITY_JOIN_INVITE";
        public Args Arguments { get; set; }
        
        public static SendActivityJoinInviteCommand Create(Action<Args> argsBuilder) {
            SendActivityJoinInviteCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args {
            public string user_id { get; set; }
        }

        // No data
    }
}
