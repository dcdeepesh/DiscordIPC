using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to consent to a Rich Presence Ask to Join request
    /// </summary>
    public class SendActivityJoinInvite {
        [DiscordRPC("SET_ACTIVITY_JOIN_INVITE")]
        public class Args : IPayloadResponse {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
        }
        
        // No data
    }
}
