using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities.Objects {
    public class WelcomeScreen {
        /// <summary></summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        /// <summary></summary>
        [JsonPropertyName("welcome_channels")]
        public List<Channel> WelcomeChannels { get; set; }
        
        public class Channel {
            /// <summary></summary>
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            /// <summary></summary>
            [JsonPropertyName("description")]
            public string Description { get; set; }
            
            /// <summary></summary>
            [JsonPropertyName("emoji_id")]
            public string EmojiID { get; set; }
            
            /// <summary></summary>
            [JsonPropertyName("emoji_name")]
            public string EmojiName { get; set; }
        }
    }
}