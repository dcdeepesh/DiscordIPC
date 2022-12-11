using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class ActivityJoinRequestEvent {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public User user { get; set; }
        }
    }
}
