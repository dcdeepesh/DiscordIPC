using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Events.DataObjects;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to change voice settings of users in voice channels
    /// </summary>
    public class SetUserVoiceSettings {
        [DiscordRPC("SET_USER_VOICE_SETTINGS")]
        public class Args : IPayloadResponse<Data> {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
            
            [JsonPropertyName("pan")]
            public Pan Pan { get; set; }
            
            [JsonPropertyName("volume")]
            public int? Volume { get; set; }
            
            [JsonPropertyName("mute")]
            public bool? Mute { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
            
            [JsonPropertyName("pan")]
            public Pan Pan { get; set; }
            
            [JsonPropertyName("volume")]
            public int? Volume { get; set; }
            
            [JsonPropertyName("mute")]
            public bool? Mute { get; set; }
        }
    }
}
