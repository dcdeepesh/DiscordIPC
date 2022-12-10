using System;

namespace Dec.DiscordIPC.Commands {
    public class SelectTextChannelCommand : ICommand<SelectTextChannelCommand.Args, SelectTextChannelCommand.Data> {
        
        public Args Arguments { get; set; }
        
        public static SelectTextChannelCommand Create(Action<Args> argsBuilder) {
            SelectTextChannelCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
        }

        public class Data : GetChannelCommand.Data { }
    }
}
