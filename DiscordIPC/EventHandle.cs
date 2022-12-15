using System;
using System.Threading.Tasks;

namespace Dec.DiscordIPC; 

public class EventHandle : IAsyncDisposable {
    private readonly Func<ValueTask> _unsubscribeAction;
    
    public EventHandle(Func<ValueTask> unsubscribeAction) {
        _unsubscribeAction = unsubscribeAction;
    }

    public async ValueTask UnsubscribeAsync() => await _unsubscribeAction();
    public async ValueTask DisposeAsync() => await UnsubscribeAsync();
}