﻿namespace Dec.DiscordIPC.Events; 

public class GuildCreateEvent : IEvent<object, GuildCreateEvent.Data> {
    public string Name => "GUILD_CREATE";
    // No arguments; dummy
    public object Arguments { get; set; }
        
    public static GuildCreateEvent Create() => new();

    public class Data {
        public string id { get; set; }
        public string name { get; set; }
    }
}