using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a guild is created/joined on the client
    /// </summary>
    public class GuildCreate {
        [DiscordRPC("GUILD_CREATE")]
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }
}
