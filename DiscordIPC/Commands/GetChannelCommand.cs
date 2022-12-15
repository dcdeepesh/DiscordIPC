using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Commands; 

public class GetChannelCommand : ICommand<GetChannelCommand.Args, GetChannelCommand.Data> {
        
    public string Name => "GET_CHANNEL";
    public Args Arguments { get; set; }
        
    public static GetChannelCommand Create(Action<Args> argsBuilder) {
        GetChannelCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string channel_id { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string id { get; set; }
        public string guild_id { get; set; }
        public string name { get; set; }
        public int? type { get; set; }
        public string topic { get; set; }
        public int? bitrate { get; set; }
        public int? user_limit { get; set; }
        public int? position { get; set; }
        public List<VoiceStateCreateEvent.Data> voice_states { get; set; }
        public List<Message> messages { get; set; }
    }
}