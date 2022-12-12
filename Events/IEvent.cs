namespace Dec.DiscordIPC.Events; 

public interface IEvent<TArgs> {
    TArgs Arguments { get; set; }
    string Name { get; }
}

public interface IEvent<TArgs, TData> : IEvent<TArgs> {
}