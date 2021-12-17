using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to update a user's Rich Presence
    /// </summary>
    public class SetActivity {
        [DiscordRPC("SET_ACTIVITY")]
        public class Args : IPayloadResponse {
            [JsonPropertyName("pid")]
            public int? PID { get; set; }
            
            [JsonPropertyName("activity")]
            public Presence.Activity Activity { get; set; }
        }
        
        // No data
    }
}
