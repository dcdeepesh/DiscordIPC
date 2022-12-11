using System;

namespace Dec.DiscordIPC.Commands; 

public class SetUserVoiceSettingsCommand : ICommand<SetUserVoiceSettingsCommand.Args, SetUserVoiceSettingsCommand.Data> {
        
    public string Name => "SET_USER_VOICE_SETTINGS";
    public Args Arguments { get; set; }
        
    public static SetUserVoiceSettingsCommand Create(Action<Args> argsBuilder) {
        SetUserVoiceSettingsCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    public class Args {
        public string user_id { get; set; }
        public Pan pan { get; set; }
        public int? volume { get; set; }
        public bool? mute { get; set; }
    }

    public class Data {
        public string user_id { get; set; }
        public Pan pan { get; set; }
        public int? volume { get; set; }
        public bool? mute { get; set; }
    }

    public class Pan {
        public float? left { get; set; }
        public float? right { get; set; }
    }
}