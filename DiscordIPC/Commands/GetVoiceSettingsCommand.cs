using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands; 

public class GetVoiceSettingsCommand : ICommand<object, GetVoiceSettingsCommand.Data> {
        
    public string Name => "GET_VOICE_SETTINGS";
    // No arguments; dummy type
    public object Arguments { get; set; }
        
    public static GetVoiceSettingsCommand Create() => new();

    public class Data : VoiceSettings { }
}