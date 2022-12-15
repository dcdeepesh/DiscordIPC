namespace Dec.DiscordIPC.Core; 

public abstract class AbstractEventListener {
    public abstract bool IsMatchingData(IpcPayload eventPayload);
    public abstract void HandleData(IpcPayload eventPayload);
}