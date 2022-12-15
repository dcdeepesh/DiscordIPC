using System.Collections.Generic;
using System.Text.Json;

namespace Dec.DiscordIPC.Core; 

public class Dispatcher {
    private readonly List<AbstractEventListener> _eventListeners = new();

    public void AddEventListener(AbstractEventListener eventListener) {
        _eventListeners.Add(eventListener);
    }

    public void DispatchEvent(IpcPayload eventPayload, JsonElement serializedEventData) {
        foreach (var listener in _eventListeners) {
            if (listener.IsMatchingData(eventPayload, serializedEventData)) {
                listener.HandleData(serializedEventData);
            }
        }
    }
}