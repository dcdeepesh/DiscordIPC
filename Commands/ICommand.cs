namespace Dec.DiscordIPC.Commands {
    public interface ICommand<TArgs> {
        string Name { get; }
        TArgs Arguments { get; set; }
    }
    
    public interface ICommand<TArgs, TData> : ICommand<TArgs> {
    }
}