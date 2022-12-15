using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class ActivityJoinEvent : IEvent<object, ActivityJoinEvent.Data> {
    public string Name => "ACTIVITY_JOIN";
    // No arguments; dummy
    public object Arguments { get; set; }

    public bool IsMatchingData(Data _) => true;

    public static ActivityJoinEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string secret { get; set; }
    }
}