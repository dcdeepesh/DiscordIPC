using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve a list of channels for a guild from the client
    /// </summary>
    public class GetChannels {
        public class Args : ICommandArgs {
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("channels")]
            public List<Channel> Channels { get; set; }
        }
    }
}
