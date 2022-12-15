using System;

namespace Dec.DiscordIPC.Events; 

public class MessageDeleteEvent : IEvent<MessageDeleteEvent.Args, MessageDeleteEvent.Data> {
    public string Name => "MESSAGE_DELETE";
    public Args Arguments { get; set; }
    
    public bool IsMatchingData(Data data) =>
        data.channel_id == Arguments.channel_id;
    
    public static MessageDeleteEvent Create(Action<Args> argsBuilder) {
        MessageDeleteEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    public class Args : MessageCreateEvent.Args { }

    public class Data : MessageCreateEvent.Data { }
}