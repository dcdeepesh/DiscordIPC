using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to set the client's voice settings
    /// </summary>
    public class SetVoiceSettings {
        public class Args : VoiceSettings, ICommandArgs { }
        
        public class Data : VoiceSettings { }
    }
}
