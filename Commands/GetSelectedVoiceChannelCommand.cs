namespace Dec.DiscordIPC.Commands {
    public class GetSelectedVoiceChannelCommand :
        ICommand<GetSelectedVoiceChannelCommand.Args, GetSelectedVoiceChannelCommand.Data> {
        // No arguments; dummy
        public class Args { }

        public class Data : GetChannelCommand.Data { }
    }
}
