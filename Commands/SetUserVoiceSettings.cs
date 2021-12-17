using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Events.DataObjects;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to change voice settings of users in voice channels
    /// </summary>
    public class SetUserVoiceSettings {
        public class Args : ICommandArgs {
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
