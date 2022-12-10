using System;
using System.Collections.Generic;

using Dec.DiscordIPC.Entities;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Commands {
    public class GetChannelCommand : ICommand<GetChannelCommand.Args, GetChannelCommand.Data> {
        
        public Args Arguments { get; set; }
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string id { get; set; }
            public string guild_id { get; set; }
            public string name { get; set; }
            public int? type { get; set; }
            public string topic { get; set; }
            public int? bitrate { get; set; }
            public int? user_limit { get; set; }
            public int? position { get; set; }
            public List<VoiceStateCreate.Data> voice_states { get; set; }
            public List<Message> messages { get; set; }
        }
    }
}
