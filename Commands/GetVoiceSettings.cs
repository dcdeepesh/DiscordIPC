using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve the client's voice settings
    /// </summary>
    public class GetVoiceSettings {
        [DiscordRPC("GET_VOICE_SETTINGS")]
        public class Args : IPayloadResponse<Data>, IDummyCommandArgs { }
        
        public class Data : VoiceSettings { }
    }
}
