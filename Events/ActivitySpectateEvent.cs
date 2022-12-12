namespace Dec.DiscordIPC.Events; 

public class ActivitySpectateEvent : IEvent<object, ActivitySpectateEvent.Data> {
    public string Name => "ACTIVITY_SPECTATE";
    // No arguments; dummy
    public object Arguments { get; set; }
        
    public static ActivitySpectateEvent Create() => new();

    public class Data {
        public string secret { get; set; }
    }
}