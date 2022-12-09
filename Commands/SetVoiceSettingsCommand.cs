using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetVoiceSettingsCommand : ICommand<SetVoiceSettingsCommand.Args, SetVoiceSettingsCommand.Data> {
        public class Args : VoiceSettings { }

        public class Data : VoiceSettings { }
    }
}
