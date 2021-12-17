using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the client receives a notification (mention or new message in eligible channels)
    /// </summary>
    public class NotificationCreate {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("message")]
            public Message Message { get; set; }
            
            [JsonPropertyName("icon_url")]
            public string IconURL { get; set; }
            
            [JsonPropertyName("title")]
            public string Title { get; set; }
            
            [JsonPropertyName("body")]
            public string Body { get; set; }
        }
    }
}
