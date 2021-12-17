using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a user in a subscribed voice channel speaks
    /// </summary>
    public class SpeakingStart {
        [DiscordRPC("SPEAKING_START")]
        public class Args : ICommandArgs {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
        }
    }
}
