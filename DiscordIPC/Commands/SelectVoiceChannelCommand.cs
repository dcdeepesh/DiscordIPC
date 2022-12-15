using System;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public class SelectVoiceChannelCommand : ICommand<SelectVoiceChannelCommand.Args, SelectVoiceChannelCommand.Data> {
        
    public string Name => "SELECT_VOICE_CHANNEL";
    public Args Arguments { get; set; }
        
    public static SelectVoiceChannelCommand Create(Action<Args> argsBuilder) {
        SelectVoiceChannelCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string channel_id { get; set; }
        public int? timeout { get; set; }
        public bool? force { get; set; }
    }

    public class Data : GetChannelCommand.Data { }
}