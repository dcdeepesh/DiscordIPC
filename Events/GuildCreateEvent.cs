namespace Dec.DiscordIPC.Events; 

public class GuildCreateEvent : IEvent<GuildCreateEvent.Args> {
    public string Name => "GUILD_CREATE";
    public Args Arguments { get; set; }
        
    public static GuildCreateEvent Create() => new();

    // No arguments; dummy
    public class Args { }

    public class Data {
        public string id { get; set; }
        public string name { get; set; }
    }
}