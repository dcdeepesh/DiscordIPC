using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Events.DataObjects {
    public class Pan {
        [JsonPropertyName("left")]
        public float? Left { get; set; }
        
        [JsonPropertyName("right")]
        public float? Right { get; set; }
    }
}