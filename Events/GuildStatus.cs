using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when a subscribed server's state changes
    /// </summary>
    public class GuildStatus {
        [DiscordRPC("GUILD_STATUS")]
        public class Args : ICommandArgs {
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("guild")]
            public Guild Guild { get; set; }
            // [Deprecated] online
        }
    }
}
