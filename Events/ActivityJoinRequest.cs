using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the user receives a Rich Presence Ask to Join request
    /// </summary>
    public class ActivityJoinRequest {
        [DiscordRPC("ACTIVITY_JOIN_REQUEST")]
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("user")]
            public User User { get; set; }
        }
    }
}
