namespace Dec.DiscordIPC.Events; 

public class ActivityJoinEvent : IEvent<object, ActivityJoinEvent.Data> {
    public string Name => "ACTIVITY_JOIN";
    // No arguments; dummy
    public object Arguments { get; set; }

    public static ActivityJoinEvent Create() => new();

    public class Data {
        public string secret { get; set; }
    }
}