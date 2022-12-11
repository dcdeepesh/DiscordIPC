using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class GuildStatusEvent : IEvent<GuildStatusEvent.Args> {
        public string Name => "GUILD_STATUS";
        public Args Arguments { get; set; }
        
        public class Args {
            public string guild_id { get; set; }
        }

        public class Data {
            public Guild guild { get; set; }
            // [Deprecated] online
        }
    }
}
