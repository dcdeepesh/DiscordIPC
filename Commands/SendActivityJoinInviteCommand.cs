namespace Dec.DiscordIPC.Commands {
    public class SendActivityJoinInviteCommand : ICommand<SendActivityJoinInviteCommand.Args> {
        public class Args {
            public string user_id { get; set; }
        }

        // No data
    }
}
