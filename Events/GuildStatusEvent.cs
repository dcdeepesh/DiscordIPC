using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class GuildStatusEvent {
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public Guild guild { get; set; }
            // [Deprecated] online
        }
    }
}
