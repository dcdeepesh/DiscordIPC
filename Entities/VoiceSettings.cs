using System.Collections.Generic;

// Done
namespace Dec.DiscordIPC.Entities {
    public class VoiceSettings {
        public Input input { get; set; }
        public Output output { get; set; }
        public Mode mode { get; set; }
        public bool? automatic_gain_control { get; set; }
        public bool? echo_cancellation { get; set; }
        public bool? noise_suppression { get; set; }
        public bool? qos { get; set; }
        public bool? silence_warning { get; set; }
        public bool? deaf { get; set; }
        public bool? mute { get; set; }

        public class Input {
            public string device_id { get; set; }
            public float? volume { get; set; }
            public List<Device> available_devices { get; set; }
        }

        public class Output {
            public string device_id { get; set; }
            public float? volume { get; set; }
            public List<Device> available_devices { get; set; }
        }

        public class Device {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Mode {
            public string type { get; set; }
            public bool? auto_threshold { get; set; }
            public float? threshold { get; set; }
            public ShortcutKeyCombo shortcut { get; set; }
            public float? delay { get; set; }
        }

        public class ShortcutKeyCombo {
            public int? type { get; set; }
            public int? code { get; set; }
            public string name { get; set; }

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
