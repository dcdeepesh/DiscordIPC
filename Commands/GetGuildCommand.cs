using System;
using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class GetGuildCommand : ICommand<GetGuildCommand.Args, GetGuildCommand.Data> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string guild_id { get; set; }
            public int? timeout { get; set; }
        }

        public class Data {
            public string id { get; set; }
            public string name { get; set; }
            public string icon_url { get; set; }
            public List<Member> members { get; set; }
        }
    }
}
