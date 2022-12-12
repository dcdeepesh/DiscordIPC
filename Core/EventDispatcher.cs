using System.Collections.Generic;
using System.Text.Json;

namespace Dec.DiscordIPC.Core; 

public class EventDispatcher {
    private readonly List<AbstractEventListener> _eventListeners = new();

    public void AddEventListener(AbstractEventListener eventListener) {
        _eventListeners.Add(eventListener);
    }

    public void Dispatch(IpcPayload eventPayload, JsonElement serializedEventData) {
        foreach (var listener in _eventListeners) {
            if (listener.IsMatchingData(eventPayload.data)) {
                listener.HandleData(eventPayload.data);
            }
        }
    }
}