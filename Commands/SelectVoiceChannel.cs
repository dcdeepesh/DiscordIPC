using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class SelectVoiceChannel {
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
            public bool? force { get; set; }
        }

        public class Data : GetChannel.Data { }
    }
}
