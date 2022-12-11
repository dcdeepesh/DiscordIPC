using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class VoiceStateCreateEvent {
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public VoiceState voice_state { get; set; }
            public User user { get; set; }
            public string nick { get; set; }
            public float? volume { get; set; }
            public bool? mute { get; set; }
            public Pan pan { get; set; }

            public class Pan {
                public float? left { get; set; }
                public float? right { get; set; }
            }
        }
    }
}
