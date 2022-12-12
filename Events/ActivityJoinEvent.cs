namespace Dec.DiscordIPC.Events; 

public class ActivityJoinEvent : IEvent<ActivityJoinEvent.Args, ActivityJoinEvent.Data> {
    public string Name => "ACTIVITY_JOIN";
    public Args Arguments { get; set; }

    public static ActivityJoinEvent Create() => new();
        
    // No arguments; dummy
    public class Args { }

    public class Data {
        public string secret { get; set; }
    }
}