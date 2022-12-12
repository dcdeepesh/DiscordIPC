using System;

namespace Dec.DiscordIPC.Events; 

public class MessageUpdateEvent : IEvent<MessageUpdateEvent.Args, MessageUpdateEvent.Data> {
    public string Name => "MESSAGE_UPDATE";
    public Args Arguments { get; set; }
    
    public bool IsMatchingData(Data data) =>
        data.channel_id == Arguments.channel_id;
    
    public MessageUpdateEvent Create(Action<Args> argsBuilder) {
        MessageUpdateEvent theEvent = new() {
            Arguments = new Args()
        };
        argsBuilder(theEvent.Arguments);
        return theEvent;
    }
        
    public class Args : MessageCreateEvent.Args { }

    public class Data : MessageCreateEvent.Data { }
}