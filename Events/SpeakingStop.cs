using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a user in a subscribed voice channel stops speaking
    /// </summary>
    public class SpeakingStop {
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
