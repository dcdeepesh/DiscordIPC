namespace Dec.DiscordIPC.Events {
    public class MessageUpdateEvent : IEvent<MessageUpdateEvent.Args> {
        public string Name => "MESSAGE_UPDATE";
        public Args Arguments { get; set; }
        
        public class Args : MessageCreateEvent.Args { }

        public class Data : MessageCreateEvent.Data { }
    }
}
