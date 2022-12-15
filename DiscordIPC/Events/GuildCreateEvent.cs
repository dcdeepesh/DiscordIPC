using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class GuildCreateEvent : IEvent<object, GuildCreateEvent.Data> {
    public string Name => "GUILD_CREATE";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static GuildCreateEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string id { get; set; }
        public string name { get; set; }
    }
}