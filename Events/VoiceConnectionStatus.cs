using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the client's voice connection status changes
    /// </summary>
    public class VoiceConnectionStatus {
        [DiscordRPC("VOICE_CONNECTION_STATUS")]
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("state")]
            public string State { get; set; }
            
            [JsonPropertyName("hostname")]
            public string HostName { get; set; }
            
            // "unreleased" problem (see docs)
            [JsonPropertyName("pings")]
            public List<float?> Pings { get; set; }
            
            [JsonPropertyName("average_ping")]
            public float? AveragePing { get; set; }
            
            [JsonPropertyName("last_ping")]
            public float? LastPing { get; set; }
        }
    }
}
