using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve the client's voice settings
    /// </summary>
    public class GetVoiceSettings {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data : VoiceSettings { }
    }
}
