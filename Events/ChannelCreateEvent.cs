namespace Dec.DiscordIPC.Events; 

public class ChannelCreateEvent : IEvent<ChannelCreateEvent.Args> {
    public string Name => "CHANNEL_CREATE";
    public Args Arguments { get; set; }
        
    public static ChannelCreateEvent Create() => new();

    // No arguments; dummy
    public class Args { }

    public class Data {
        public string id { get; set; }
        public string name { get; set; }
        public int? type { get; set; }
    }
}