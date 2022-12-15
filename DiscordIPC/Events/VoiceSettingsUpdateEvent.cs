using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class VoiceSettingsUpdateEvent : IEvent<object, VoiceSettingsUpdateEvent.Data> {
    public string Name => "VOICE_SETTINGS_UPDATE";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static VoiceSettingsUpdateEvent Create() => new();

    public class Data : VoiceSettings { }
}