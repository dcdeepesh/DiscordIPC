﻿using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class MessageCreateEvent {
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string channel_id { get; set; }
            public Message message { get; set; }
        }
    }
}
