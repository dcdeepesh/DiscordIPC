using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to join or leave a text channel, group dm, or dm
    /// </summary>
    public class SelectTextChannel {
        public class Args : ICommandArgs {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("timeout")]
            public int? Timeout { get; set; }
        }

        public class Data : Channel { }
    }
}
