using System;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class MessageCreateEvent : IEvent<MessageCreateEvent.Args, MessageCreateEvent.Data> {
    public string Name => "MESSAGE_CREATE";
    public Args Arguments { get; set; }

    public bool IsMatchingData(Data data) =>
        data.channel_id == Arguments.channel_id;
    
    public static MessageCreateEvent Create(Action<Args> argsBuilder) {
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