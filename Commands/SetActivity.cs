using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to update a user's Rich Presence
    /// </summary>
    public class SetActivity {
        public class Args : ICommandArgs {
            [JsonPropertyName("pid")]
            public int? PID { get; set; }
            
            [JsonPropertyName("activity")]
            public Presence.Activity Activity { get; set; }
        }
        
        // No data
    }
}
