using System.Text.Json;

namespace Dec.DiscordIPC.Core; 

public abstract class AbstractEventListener {
    public abstract string EventName { get; set; }
    public abstract bool IsMatchingData(IpcPayload eventPayload, JsonElement serializedEventData);
    public abstract void HandleData(JsonElement serializedEventData);
}