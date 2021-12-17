using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to reject a Rich Presence Ask to Join request
    /// </summary>
    public class CloseActivityRequest {
        public class Args : ICommandArgs {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
        }
        
        // No data
    }
}
