namespace Dec.DiscordIPC.Commands {
    public class CloseActivityRequestCommand : ICommand<CloseActivityRequestCommand.Args> {
        public class Args {
            public string user_id { get; set; }
        }

        // No data
    }
}
