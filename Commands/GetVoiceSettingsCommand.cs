﻿using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetVoiceSettingsCommand : ICommand<GetVoiceSettingsCommand.Args, GetVoiceSettingsCommand.Data> {
        
        public string Name => "GET_VOICE_SETTINGS";
        public Args Arguments { get; set; }
        
        public static Args Create() => new();

        // No arguments; dummy type
        public class Args { }

        public class Data : VoiceSettings { }
    }
}
