using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;
using Dec.DiscordIPC.Events.DataObjects;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a user joins a subscribed voice channel
    /// </summary>
    public class VoiceStateCreate {
        public class Args : ICommandArgs {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("voice_state")]
            public VoiceState VoiceState { get; set; }
            
            [JsonPropertyName("user")]
            public User User { get; set; }
            
            [JsonPropertyName("nick")]
            public string Nick { get; set; }
            
            [JsonPropertyName("volume")]
            public float? Volume { get; set; }
            
            [JsonPropertyName("mute")]
            public bool? Mute { get; set; }
            
            [JsonPropertyName("pan")]
            public Pan Pan { get; set; }
        }
    }
}
