using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the client's voice settings update
    /// </summary>
    public class VoiceSettingsUpdate {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data : VoiceSettings { }
    }
}
