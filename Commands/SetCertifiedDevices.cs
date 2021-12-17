using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to send info about certified hardware devices
    /// </summary>
    public class SetCertifiedDevices {
        public class Args : ICommandArgs {
            [JsonPropertyName("devices")]
            public List<Device> Devices { get; set; }
        }
        
        // No data
        
        public class Device {
            [JsonPropertyName("type")]
            public string Type { get; set; }
            
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("vendor")]
            public VendorType Vendor { get; set; }
            
            [JsonPropertyName("model")]
            public ModelType Model { get; set; }
            
            [JsonPropertyName("related")]
            public List<string> Related { get; set; }
            
            [JsonPropertyName("echo_cancellation")]
            public bool? EchoCancellation { get; set; }
            
            [JsonPropertyName("noise_suppression")]
            public bool? NoiseSuppression { get; set; }
            
            [JsonPropertyName("automatic_gain_control")]
            public bool? AutomaticGainControl { get; set; }
            
            [JsonPropertyName("hardware_mute")]
            public bool? HardwareMute { get; set; }
            
            public class Types {
                public static readonly string
                    AUDIO_INPUT = "audioinput",
                    AUDIO_OUTPUT = "audiooutput",
                    VIDEO_INPUT = "videoinput";
            }
            
            public class VendorType {
                [JsonPropertyName("name")]
                public string Name { get; set; }
                
                [JsonPropertyName("url")]
                public string URL { get; set; }
            }

            public class ModelType {
                [JsonPropertyName("name")]
                public string Name { get; set; }
                
                [JsonPropertyName("url")]
                public string URL { get; set; }
            }
        }
    }
}
