using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve a list of channels for a guild from the client
    /// </summary>
    public class GetChannels {
        [DiscordRPC("GET_CHANNELS")]
        public class Args : IPayloadResponse<Data> {
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("channels")]
            public List<Channel> Channels { get; set; }
        }
    }
}
