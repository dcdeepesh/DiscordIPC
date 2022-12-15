using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events;

public class ActivityJoinRequestEvent : IEvent<object, ActivityJoinRequestEvent.Data> {
    public string Name => "ACTIVITY_JOIN_REQUEST";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static ActivityJoinRequestEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public User user { get; set; }
    }
}