﻿namespace Dec.DiscordIPC.Events; 

public class VoiceChannelSelectEvent : IEvent<object, VoiceChannelSelectEvent.Data> {
    public string Name => "VOICE_CHANNEL_SELECT";
    // No arguments; dummy
    public object Arguments { get; set; }
        
    public static VoiceChannelSelectEvent Create() => new();

    public class Data {
        public string channel_id { get; set; }
        public string guild_id { get; set; }
    }
}