using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to authenticate an existing client with your app
    /// </summary>
    public class Authenticate {
        [DiscordRPC("AUTHENTICATE", false)]
        public class Args : IPayloadResponse<Data> {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
        }
        
        public class Data {
            [JsonPropertyName("user")]
            public User User { get; set; }
            
            [JsonPropertyName("scopes")]
            public List<string> Scopes { get; set; }
            
            [JsonPropertyName("expires")]
            public string Expires { get; set; }
            
            [JsonPropertyName("application")]
            public OAuth2AppStructure Application { get; set; }
            
            public class OAuth2AppStructure {
                [JsonPropertyName("description")]
                public string Description { get; set; }
                
                [JsonPropertyName("icon")]
                public string Icon { get; set; }
                
                [JsonPropertyName("id")]
                public string ID { get; set; }
                
                [JsonPropertyName("rpc_origins")]
                public List<string> RPCOrigins { get; set; }
                
                [JsonPropertyName("name")]
                public string Name { get; set; }
            }
        }
    }
}
