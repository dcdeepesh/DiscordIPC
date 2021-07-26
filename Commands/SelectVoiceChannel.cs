using Dec.DiscordIPC.Entities;
using System.Collections.Generic;

namespace Dec.DiscordIPC.Commands {
    public class SelectVoiceChannel {
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
            public bool? force { get; set; }
        }

        public class Data {
            public string id { get; set; }
            public string guild_id { get; set; }
            public string name { get; set; }
            public int? type { get; set; }
            public string topic { get; set; }
            public int? bitrate { get; set; }
            public int? user_limit { get; set; }
            public int? position { get; set; }
            public List<VoiceState> voice_states { get; set; }
            public List<Message> messages { get; set; }
        }
    }
}
