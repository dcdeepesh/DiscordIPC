﻿using System;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;
using static Dec.DiscordIPC.Core.LoggerWrapper;

namespace Dec.DiscordIPC; 

/// <summary>
/// The main access point for user programs to use DiscordIPC.
/// </summary>
public class DiscordIpcClient : IDisposable {

    private readonly IpcHandler _ipcHandler;
    private readonly Dispatcher _dispatcher;
    private readonly DiscordIpcClientOptions _options;

    public DiscordIpcClient(string clientId) : this(clientId, new DiscordIpcClientOptions()) {
    }

    public DiscordIpcClient(string clientId, DiscordIpcClientOptions options) {
        _dispatcher = new Dispatcher();
        _ipcHandler = new IpcHandler(clientId, _dispatcher);
        _options = options;
        Logger = options.Logger;
    }
    
    public async Task ConnectToDiscordAsync(int timeoutMs = 2000,
        CancellationToken ctk = default) {

        await _ipcHandler.ConnectToPipeAsync(_options.PipeNumber, timeoutMs, ctk)
            .ConfigureAwait(false);
        _ipcHandler.InitMessageLoopAndDispatcher();
        await _ipcHandler.SendHandshakeAsync(ctk)
            .ConfigureAwait(false);
    }

    public async Task<TData> SendCommandAsync<TArgs, TData>(
        ICommand<TArgs, TData> command,
        CancellationToken ctk = default) =>
        (TData) await SendCommandAsync(command, typeof(TData), ctk).ConfigureAwait(false);

    public async Task SendCommandAsync<TArgs>(
        ICommand<TArgs> command,
        CancellationToken ctk = default) =>
        await SendCommandAsync(command, null, ctk).ConfigureAwait(false);

    private async Task<object> SendCommandAsync<TArgs>(
        ICommand<TArgs> command,
        Type returnType,
        CancellationToken ctk = default) {
        
        IpcPayload response = await _ipcHandler.SendPayloadAsync(new IpcPayload {
            cmd = command.Name,
            nonce = Guid.NewGuid().ToString(),
            args = command.Arguments
        }, ctk).ConfigureAwait(false);

        return returnType is null ? null :
            response.GetData(returnType);
    }

    public async Task<EventHandle> SubscribeAsync<TArgs, TData>(
        IEvent<TArgs, TData> theEvent,
        Action<TData> eventHandler,
        CancellationToken ctk = default) {
        
        var eventListener = EventListener.Create(theEvent, eventHandler);
        _dispatcher.AddEventListener(eventListener);

        // READY event doesn't need a subscription command
        if (theEvent is not ReadyEvent) {
            await _ipcHandler.SendPayloadAsync(new IpcPayload {
                cmd = "SUBSCRIBE",
                nonce = Guid.NewGuid().ToString(),
                evt = theEvent.Name,
                args = theEvent.Arguments
            }, ctk).ConfigureAwait(false);
        }

        return new EventHandle(Unsubscribe, UnsubscribeAsync);

        // Local helper methods
        
        IpcPayload MakeUnsubscribePayload() => new() {
            cmd = "UNSUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        void Unsubscribe() {
            if (theEvent is not ReadyEvent) {
                _ipcHandler.SendPayload(MakeUnsubscribePayload());
            }
        }

        async ValueTask UnsubscribeAsync() {
            if (theEvent is not ReadyEvent) {
                await _ipcHandler.SendPayloadAsync(MakeUnsubscribePayload(), ctk)
                    .ConfigureAwait(false);
            }
        }
    }

    public void Dispose() {
        _ipcHandler.Dispose();
    }
}