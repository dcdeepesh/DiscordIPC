using System.Collections.Generic;

namespace Dec.DiscordIPC.Core; 

public class EventDispatcher {
    // private List<EventListener<>
    private List<object> _eventListeners = new();

    public void Dispatch(object eventPayload) {
        
    }
}