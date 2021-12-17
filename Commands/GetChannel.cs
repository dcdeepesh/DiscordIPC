using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to retrieve channel information from the client
    /// </summary>
    public class GetChannel {
        public class Args : ICommandArgs {
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("type")]
            public int? Type { get; set; }
            
            [JsonPropertyName("topic")]
            public string Topic { get; set; }
            
            [JsonPropertyName("bitrate")]
            public int? Bitrate { get; set; }
            
            [JsonPropertyName("user_limit")]
            public int? UserLimit { get; set; }
            
            [JsonPropertyName("position")]
            public int? Position { get; set; }
            
            [JsonPropertyName("voice_states")]
            public List<VoiceStateContainer> VoiceStates { get; set; }
            
            [JsonPropertyName("messages")]
            public List<Message> Messages { get; set; }
        }
    }
}
