using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to reject a Rich Presence Ask to Join request
    /// </summary>
    public class CloseActivityRequest {
        [DiscordRPC("CLOSE_ACTIVITY_REQUEST")]
        public class Args : IPayloadResponse {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
        }
        
        // No data
    }
}
