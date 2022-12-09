using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetVoiceSettingsCommand : ICommand<GetVoiceSettingsCommand.Args, GetVoiceSettingsCommand.Data> {
        // No arguments; dummy
        public class Args { }

        public class Data : VoiceSettings { }
    }
}
