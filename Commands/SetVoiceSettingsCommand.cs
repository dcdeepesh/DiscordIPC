using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SetVoiceSettingsCommand : ICommand<SetVoiceSettingsCommand.Args, SetVoiceSettingsCommand.Data> {
        
        public Args Arguments { get; set; }
        
        public static SetVoiceSettingsCommand Create(Action<Args> argsBuilder) {
            SetVoiceSettingsCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args : VoiceSettings { }

        public class Data : VoiceSettings { }
    }
}
