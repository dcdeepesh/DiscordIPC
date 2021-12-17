using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the client's voice settings update
    /// </summary>
    public class VoiceSettingsUpdate {
        [DiscordRPC("VOICE_SETTINGS_UPDATE")]
        public class Args : IDummyCommandArgs { }
        
        public class Data : VoiceSettings { }
    }
}
