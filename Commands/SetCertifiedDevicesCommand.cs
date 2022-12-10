using System;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class SetCertifiedDevicesCommand : ICommand<SetCertifiedDevicesCommand.Args> {
        
        public string Name => "SET_CERTIFIED_DEVICES";
        public Args Arguments { get; set; }
        
        public static SetCertifiedDevicesCommand Create(Action<Args> argsBuilder) {
            SetCertifiedDevicesCommand command = new() {
                Arguments = new Args()
            };
            argsBuilder(command.Arguments);
            return command;
        }
        
        public class Args {
            public List<Device> devices { get; set; }
        }

        // No data

        public class Device {
            public string type { get; set; }
            public string id { get; set; }
            public Vendor vendor { get; set; }
            public Model model { get; set; }
            public List<string> related { get; set; }
            public bool? echo_cancellation { get; set; }
            public bool? noise_suppression { get; set; }
            public bool? automatic_gain_control { get; set; }
            public bool? hardware_mute { get; set; }

            public class Type {
                public static readonly string
                    AUDIO_INPUT = "audioinput",
                    AUDIO_OUTPUT = "audiooutput",
                    VIDEO_INPUT = "videoinput";
            }

            public class Vendor {
                public string name { get; set; }
                public string url { get; set; }
            }

            public class Model {
                public string name { get; set; }
                public string url { get; set; }
            }
        }
    }
}
