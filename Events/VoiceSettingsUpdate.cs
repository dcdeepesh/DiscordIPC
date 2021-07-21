using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class VoiceSettingsUpdate {
        // No arguments; dummy
        public class Args { }

        public class Data : VoiceSettings { }
    }
}
