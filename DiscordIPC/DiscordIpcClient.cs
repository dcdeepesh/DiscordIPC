using System;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;

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
    }
    
    public async Task ConnectToDiscordAsync(int timeoutMs = 2000,
        CancellationToken ctk = default) {

        await _ipcHandler.ConnectToPipeAsync(_options.PipeNumber, timeoutMs, ctk)
            .ConfigureAwait(false);
        _ipcHandler.InitMessageLoopAndDispatcher();
        await _ipcHandler.SendHandshakeAsync(ctk)
            .ConfigureAwait(false);
    }

    public async Task<TData> SendCommandAsync<TArgs, TData>(ICommand<TArgs, TData> command) =>
        (TData) await SendCommandAsync(command, typeof(TData)).ConfigureAwait(false);

    public async Task SendCommandAsync<TArgs>(ICommand<TArgs> command) =>
        await SendCommandAsync(command, null).ConfigureAwait(false);

    private async Task<object> SendCommandAsync<TArgs>(
        ICommand<TArgs> command,
        Type returnType) {
        
        IpcPayload response = await _ipcHandler.SendPayloadAsync(new IpcPayload {
            cmd = command.Name,
            nonce = Guid.NewGuid().ToString(),
            args = command.Arguments
        }).ConfigureAwait(false);

        return returnType is null ? null :
            response.GetData(returnType);
    }

    public async Task<EventHandle> SubscribeAsync<TArgs, TData>(
        IEvent<TArgs, TData> theEvent,
        Action<TData> eventHandler) {
        
        var eventListener = EventListener.Create(theEvent, eventHandler);
        _dispatcher.AddEventListener(eventListener);

        // READY event doesn't need a subscription command
        if (theEvent is not ReadyEvent) {
            await _ipcHandler.SendPayloadAsync(new IpcPayload {
                cmd = "SUBSCRIBE",
                nonce = Guid.NewGuid().ToString(),
                evt = theEvent.Name,
                args = theEvent.Arguments
            }).ConfigureAwait(false);
        }

        return new EventHandle(UnsubscribeAsync);

        async ValueTask UnsubscribeAsync() {
            // READY event doesn't need an unsubscription command
            if (theEvent is not ReadyEvent) {
                await _ipcHandler.SendPayloadAsync(new IpcPayload {
                    cmd = "UNSUBSCRIBE",
                    nonce = Guid.NewGuid().ToString(),
                    evt = theEvent.Name,
                    args = theEvent.Arguments
                }).ConfigureAwait(false);
            }
        }
    }

    public void Dispose() {
        _ipcHandler.Dispose();
    }
}