namespace Dec.DiscordIPC.Core; 

public abstract class AbstractEventListener {
    public abstract string EventName { get; }
    public abstract bool IsMatchingData(object data);
    public abstract void HandleData(object data);
}