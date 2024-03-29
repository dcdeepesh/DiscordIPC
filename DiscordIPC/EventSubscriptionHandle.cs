﻿using System;
using System.Threading.Tasks;

namespace Dec.DiscordIPC; 

public class EventSubscriptionHandle : IDisposable, IAsyncDisposable {
    private readonly Action _unsubscribeAction;
    private readonly Func<ValueTask> _asyncUnsubscribeAction;
    
    public EventSubscriptionHandle(Action unsubscribeAction, Func<ValueTask> asyncUnsubscribeAction) {
        _unsubscribeAction = unsubscribeAction;
        _asyncUnsubscribeAction = asyncUnsubscribeAction;
    }

    public async ValueTask UnsubscribeAsync() =>
        await _asyncUnsubscribeAction().ConfigureAwait(false);

    public void Dispose() => _unsubscribeAction();
    
    public async ValueTask DisposeAsync() =>
        await UnsubscribeAsync().ConfigureAwait(false);
}