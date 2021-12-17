using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to consent to a Rich Presence Ask to Join request
    /// </summary>
    public class SendActivityJoinInvite {
        public class Args : ICommandArgs {
            [JsonPropertyName("user_id")]
            public string UserID { get; set; }
        }
        
        // No data
    }
}
