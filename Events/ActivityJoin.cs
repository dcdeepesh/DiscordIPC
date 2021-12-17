using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the user clicks a Rich Presence join invite in chat to join a game
    /// </summary>
    public class ActivityJoin {
        [DiscordRPC("ACTIVITY_JOIN")]
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("secret")]
            public string Secret { get; set; }
        }
    }
}
