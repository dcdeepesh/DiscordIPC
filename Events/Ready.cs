using System.Text.Json.Serialization;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    /// <summary>
    /// Non-subscription event sent immediately after connecting, contains server information
    /// </summary>
    public class Ready {
        // No args because this isn't a subscription event
        
        public class Data {
            [JsonPropertyName("v")]
            public int? Version { get; set; }
            
            [JsonPropertyName("config")]
            public RPCServerConfig Config { get; set; }
            
            [JsonPropertyName("user")]
            public User User { get; set; }
            
            public class RPCServerConfig {
                [JsonPropertyName("cdn_host")]
                public string CDNHost { get; set; }
                
                [JsonPropertyName("api_endpoint")]
                public string APIEndpoint { get; set; }
                
                [JsonPropertyName("environment")]
                public string Environment { get; set; }
            }
        }
    }
}
