using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class VoiceSettingsUpdateEvent : IEvent<VoiceSettingsUpdateEvent.Args, VoiceSettingsUpdateEvent.Data> {
    public string Name => "VOICE_SETTINGS_UPDATE";
    public Args Arguments { get; set; }
        
    public static VoiceSettingsUpdateEvent Create() => new();

    // No arguments; dummy
    public class Args { }

    public class Data : VoiceSettings { }
}