using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a guild is created/joined on the client
    /// </summary>
    public class GuildCreate {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }
}
