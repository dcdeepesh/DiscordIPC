using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    /// <summary>
    /// 
    /// </summary>
    public class VoiceSettings {
        /// <summary>Input settings</summary>
        [JsonPropertyName("input")]
        public VoiceInput InputDevice { get; set; }
        
        /// <summary>Output settings</summary>
        [JsonPropertyName("output")]
        public VoiceOutput OutputDevice { get; set; }
        
        /// <summary>Voice mode settings</summary>
        [JsonPropertyName("mode")]
        public VoiceMode Mode { get; set; }
        
        /// <summary>State of automatic gain control</summary>
        [JsonPropertyName("automatic_gain_control")]
        public bool? AutomaticGainControl { get; set; }
        
        /// <summary>State of echo cancellation</summary>
        [JsonPropertyName("echo_cancellation")]
        public bool? EchoCancellation { get; set; }
        
        /// <summary>State of noise suppression</summary>
        [JsonPropertyName("noise_suppression")]
        public bool? NoiseSuppression { get; set; }
        
        /// <summary>State of voice quality of service</summary>
        [JsonPropertyName("qos")]
        public bool? QualityOfService { get; set; }
        
        /// <summary>State of silence warning notice</summary>
        [JsonPropertyName("silence_warning")]
        public bool? SilenceWarning { get; set; }
        
        /// <summary>State of self-deafen</summary>
        [JsonPropertyName("deaf")]
        public bool? Deaf { get; set; }
        
        /// <summary>State of self-mute</summary>
        [JsonPropertyName("mute")]
        public bool? Mute { get; set; }
        
        /// <summary>
        /// Voice Settings Input Object
        /// </summary>
        public class VoiceInput {
            /// <summary>Device ID</summary>
            [JsonPropertyName("device_id")]
            public string DeviceID { get; set; }
            
            /// <summary>Input voice level (min: 0, max: 100)</summary>
            [JsonPropertyName("volume")]
            public float? Volume { get; set; }
            
            /// <summary>Array of read-only device objects</summary>
            [JsonPropertyName("available_devices")]
            public List<Device> AvailableDevices { get; set; }
        }
        
        /// <summary>
        /// Voice Settings Output Object
        /// </summary>
        public class VoiceOutput {
            /// <summary>Device ID</summary>
            [JsonPropertyName("device_id")]
            public string DeviceID { get; set; }
            
            /// <summary>Output voice level (min: 0, max: 200)</summary>
            [JsonPropertyName("volume")]
            public float? Volume { get; set; }
            
            /// <summary>Array of read-only device objects</summary>
            [JsonPropertyName("available_devices")]
            public List<Device> AvailableDevices { get; set; }
        }
        
        public class Device {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
        
        /// <summary>
        /// Voice Settings Mode Object
        /// </summary>
        public class VoiceMode {
            /// <summary>Voice setting mode type (Can be `PUSH_TO_TALK` or `VOICE_ACTIVITY`)</summary>
            [JsonPropertyName("type")]
            public string Type { get; set; }
            
            /// <summary>Voice activity threshold automatically sets its threshold</summary>
            [JsonPropertyName("auto_threshold")]
            public bool? AutoThreshold { get; set; }
            
            /// <summary>Threshold for voice activity (in dB) (min: -100, max: 0)</summary>
            [JsonPropertyName("threshold")]
            public float? Threshold { get; set; }
            
            /// <summary>Shortcut key combos for PTT</summary>
            [JsonPropertyName("shortcut")]
            public List<ShortcutKeyCombo> Shortcut { get; set; }
            
            /// <summary>The PTT release delay (in ms) (min: 0, max: 2000)</summary>
            [JsonPropertyName("delay")]
            public float? Delay { get; set; }
            
            public enum Types {
                PUSH_TO_TALK,
                VOICE_ACTIVITY
            }
        }
        public class ShortcutKeyCombo {
            /// <summary>See <see cref="KeyType"/></summary>
            [JsonPropertyName("type")]
            public int? Type { get; set; }
            
            /// <summary>Key code</summary>
            [JsonPropertyName("code")]
            public int? Code { get; set; }
            
            /// <summary>Key name</summary>
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            public class KeyType {
                public static readonly int
                    KEYBOARD_KEY = 0,
                    MOUSE_BUTTON = 1,
                    KEYBOARD_MODIFIER_KEY = 2,
                    GAMEPAD_BUTTON = 3;
            }
        }
    }
}
