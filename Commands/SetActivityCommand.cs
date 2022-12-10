using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetActivityCommand : ICommand<SetActivityCommand.Args> {
        public class Args {
            public int? pid { get; set; }
            public Presence.Activity activity { get; set; }
        }

        // No data
    }
}
