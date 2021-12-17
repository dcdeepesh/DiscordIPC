using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Sent when the client joins a voice channel
    /// </summary>
    public class VoiceChannelSelect {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
        }
    }
}
