using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to join or leave a text channel, group dm, or dm
    /// </summary>
    public class SelectTextChannel {
        [DiscordRPC("SELECT_TEXT_CHANNEL")]
        public class Args : IPayloadResponse<Data> {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("timeout")]
            public int? Timeout { get; set; }
        }

        public class Data : Channel { }
    }
}
