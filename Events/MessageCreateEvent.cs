using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class MessageCreateEvent : IEvent<MessageCreateEvent.Args> {
        public string Name => "MESSAGE_CREATE";
        public Args Arguments { get; set; }
        
        public MessageCreateEvent Create(Action<Args> argsBuilder) {
            MessageCreateEvent theEvent = new() {
                Arguments = new Args()
            };
            argsBuilder(theEvent.Arguments);
            return theEvent;
        }
        
        public class Args {
            public string channel_id { get; set; }
        }

        public class Data {
            public string channel_id { get; set; }
            public Message message { get; set; }
        }
    }
}
