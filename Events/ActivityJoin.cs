using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the user clicks a Rich Presence join invite in chat to join a game
    /// </summary>
    public class ActivityJoin {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("secret")]
            public string Secret { get; set; }
        }
    }
}
