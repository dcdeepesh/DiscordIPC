namespace Dec.DiscordIPC.Commands {
    public class SelectTextChannelCommand : ICommand<SelectTextChannelCommand.Args, SelectTextChannelCommand.Data> {
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
        }

        public class Data : GetChannelCommand.Data { }
    }
}
