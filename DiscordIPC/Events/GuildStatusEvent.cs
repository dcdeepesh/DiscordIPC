﻿using System;
using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class GuildStatusEvent : IEvent<GuildStatusEvent.Args, GuildStatusEvent.Data> {
    public string Name => "GUILD_STATUS";
    public Args Arguments { get; set; }

    public bool IsMatchingData(Data data) =>
        data.guild.id == Arguments.guild_id;

    public static GuildStatusEvent Create(Action<Args> argsBuilder) {
        GuildStatusEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public string guild_id { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public Guild guild { get; set; }
        // [Deprecated] online
    }
}