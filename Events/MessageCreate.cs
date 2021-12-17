using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a message is created in a subscribed text channel
    /// </summary>
    public class MessageCreate {
        [DiscordRPC("MESSAGE_CREATE")]
        public class Args : ICommandArgs {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("message")]
            public Message Message { get; set; }
        }
    }
}
