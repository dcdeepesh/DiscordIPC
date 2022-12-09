using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetActivityCommand {
        public class Args {
            public int? pid { get; set; }
            public Presence.Activity activity { get; set; }
        }

        // No data
    }
}
