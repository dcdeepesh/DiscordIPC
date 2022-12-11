using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public interface ICommand<TArgs> {
    string Name { get; }
    TArgs Arguments { get; set; }
}
    
[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ICommand<TArgs, TData> : ICommand<TArgs> {
}