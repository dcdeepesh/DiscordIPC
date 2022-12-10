namespace Dec.DiscordIPC.Commands {
    public class GetSelectedVoiceChannelCommand :
        ICommand<GetSelectedVoiceChannelCommand.Args, GetSelectedVoiceChannelCommand.Data> {
        
        public Args Arguments { get; set; }
        
        public static Args Create() => new();

        // No arguments; dummy
        public class Args { }

        public class Data : GetChannelCommand.Data { }
    }
}
