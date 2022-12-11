namespace Dec.DiscordIPC.Events {
    public class MessageDeleteEvent : IEvent<MessageDeleteEvent.Args> {
        public string Name => "MESSAGE_DELETE";
        public Args Arguments { get; set; }
        
        public class Args : MessageCreateEvent.Args { }

        public class Data : MessageCreateEvent.Data { }
    }
}
