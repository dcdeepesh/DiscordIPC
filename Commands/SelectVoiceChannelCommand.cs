using System;

namespace Dec.DiscordIPC.Commands {
    public class SelectVoiceChannelCommand : ICommand<SelectVoiceChannelCommand.Args, SelectVoiceChannelCommand.Data> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
            public bool? force { get; set; }
        }

        public class Data : GetChannelCommand.Data { }
    }
}
