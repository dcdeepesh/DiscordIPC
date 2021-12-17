using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve guild information from the client
    /// </summary>
    public class GetGuild {
        public class Args : ICommandArgs {
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
            
            [JsonPropertyName("timeout")]
            public int? Timeout { get; set; }
        }

        public class Data {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("icon_url")]
            public string IconURL { get; set; }
            
            [JsonPropertyName("members")]
            public List<Member> Members { get; set; }
        }
    }
}
