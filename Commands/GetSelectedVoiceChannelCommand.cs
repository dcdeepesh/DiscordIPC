namespace Dec.DiscordIPC.Commands; 

public class GetSelectedVoiceChannelCommand :
    ICommand<object, GetSelectedVoiceChannelCommand.Data> {
        
    public string Name => "GET_SELECTED_VOICE_CHANNEL";
    // No arguments; dummy
    public object Arguments { get; set; }
        
    public static GetSelectedVoiceChannelCommand Create() => new();

    public class Data : GetChannelCommand.Data { }
}