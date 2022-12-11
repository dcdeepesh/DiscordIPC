using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class ActivityJoinRequestEvent : IEvent<ActivityJoinRequestEvent.Args> {
        public string Name => "ACTIVITY_JOIN_REQUEST";
        public Args Arguments { get; set; }
        
        // No arguments; dummy
        public class Args { }

        public class Data {
            public User user { get; set; }
        }
    }
}
