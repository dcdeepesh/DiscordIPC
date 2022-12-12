namespace Dec.DiscordIPC.Events; 

public class ChannelCreateEvent : IEvent<object, ChannelCreateEvent.Data> {
    public string Name => "CHANNEL_CREATE";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static ChannelCreateEvent Create() => new();

    public class Data {
        public string id { get; set; }
        public string name { get; set; }
        public int? type { get; set; }
    }
}