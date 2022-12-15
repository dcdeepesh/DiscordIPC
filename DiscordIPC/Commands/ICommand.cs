using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public interface ICommand<out TArgs> {
    string Name { get; }
    TArgs Arguments { get; }
}
    
[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ICommand<out TArgs, TData> : ICommand<TArgs> {
}