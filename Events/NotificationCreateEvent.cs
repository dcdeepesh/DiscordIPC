using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class NotificationCreateEvent : IEvent<NotificationCreateEvent.Args> {
        public string Name => "NOTIFICATION_CREATE";
        public Args Arguments { get; set; }
        
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string channel_id { get; set; }
            public Message message { get; set; }
            public string icon_url { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }
    }
}
