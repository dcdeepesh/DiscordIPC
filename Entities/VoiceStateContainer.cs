using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class VoiceStateContainer {
        [JsonPropertyName("nick")]
        public string Nick { get; set; }
        
        [JsonPropertyName("mute")]
        public bool? Mute { get; set; }
        
        [JsonPropertyName("voice_state")]
        public VoiceState VoiceState { get; set; }
        
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}