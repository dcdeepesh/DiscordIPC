using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Events; 

public class VoiceChannelSelectEvent : IEvent<object, VoiceChannelSelectEvent.Data> {
    public string Name => "VOICE_CHANNEL_SELECT";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static VoiceChannelSelectEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string channel_id { get; set; }
        public string guild_id { get; set; }
    }
}