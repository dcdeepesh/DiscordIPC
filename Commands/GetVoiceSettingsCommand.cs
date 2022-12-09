using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetVoiceSettingsCommand {
        // No arguments; dummy
        public class Args { }

        public class Data : VoiceSettings { }
    }
}
