using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a channel is created/joined on the client
    /// </summary>
    public class ChannelCreate {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }

        public class Data {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("type")]
            public int? Type { get; set; }
        }
    }
}
