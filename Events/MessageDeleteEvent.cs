using System;

namespace Dec.DiscordIPC.Events; 

public class MessageDeleteEvent : IEvent<MessageDeleteEvent.Args> {
    public string Name => "MESSAGE_DELETE";
    public Args Arguments { get; set; }
        
    public MessageDeleteEvent Create(Action<Args> argsBuilder) {
        MessageDeleteEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    public class Args : MessageCreateEvent.Args { }

    public class Data : MessageCreateEvent.Data { }
}