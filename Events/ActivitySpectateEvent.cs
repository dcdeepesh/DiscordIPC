using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class ActivitySpectateEvent : IEvent<object, ActivitySpectateEvent.Data> {
    public string Name => "ACTIVITY_SPECTATE";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static ActivitySpectateEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string secret { get; set; }
    }
}