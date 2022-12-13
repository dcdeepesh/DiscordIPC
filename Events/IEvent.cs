namespace Dec.DiscordIPC.Events; 

public interface IEvent<TArgs, in TData> {
    TArgs Arguments { get; set; }
    string Name { get; }
    bool IsMatchingData(TData data);
}