using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetVoiceSettingsCommand : ICommand<SetVoiceSettingsCommand.Args, SetVoiceSettingsCommand.Data> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args : VoiceSettings { }

        public class Data : VoiceSettings { }
    }
}
