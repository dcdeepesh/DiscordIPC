namespace Dec.DiscordIPC.Commands {
    public interface ICommand<TArgs> {
        TArgs Arguments { get; set; }
    }
    
    public interface ICommand<TArgs, TData> {
        TArgs Arguments { get; set; }
    }
}