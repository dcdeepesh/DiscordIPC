using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to set the client's voice settings
    /// </summary>
    public class SetVoiceSettings {
        [DiscordRPC("SET_VOICE_SETTINGS")]
        public class Args : VoiceSettings, IPayloadResponse<Data> { }
        
        public class Data : VoiceSettings { }
    }
}
