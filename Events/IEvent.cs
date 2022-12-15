namespace Dec.DiscordIPC.Events; 

public interface IEvent<out TArgs, in TData> {
    TArgs Arguments { get; }
    string Name { get; }
    bool IsMatchingData(TData data);
}