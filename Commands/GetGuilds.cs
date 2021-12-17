using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve a list of guilds from the client
    /// </summary>
    public class GetGuilds {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("guilds")]
            public List<Guild> Guilds { get; set; }
        }
    }
}
