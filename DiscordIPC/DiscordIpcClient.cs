using System;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Core.Ipc;
using Dec.DiscordIPC.Events;
using static Dec.DiscordIPC.Core.LoggerWrapper;

namespace Dec.DiscordIPC;

/// <summary>
/// The main access point for user programs to use DiscordIPC.
/// </summary>
public class DiscordIpcClient : IDisposable {

    private readonly DiscordIpcClientOptions _options;
    private readonly PayloadGateway _payloadGateway;

    public DiscordIpcClient(string clientId, DiscordIpcClientOptions options = default) {
        _options = options ?? new DiscordIpcClientOptions();
        _payloadGateway = new PayloadGateway(clientId);
        Logger = options.Logger;
    }
    
    public async Task ConnectToDiscordAsync(int timeoutMs = 2000,
        CancellationToken ctk = default) {

        await _payloadGateway.InitComponentsAsync(_options.PipeNumber, timeoutMs, ctk)
            .ConfigureAwait(false);
        await _payloadGateway.PerformHandshakeAsync(ctk)
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
        
        Payload response = await _payloadGateway.SendPayloadAsync(new Payload {
            cmd = command.Name,
            nonce = Guid.NewGuid().ToString(),
            args = command.Arguments
        }, ctk).ConfigureAwait(false);

        return returnType is null ? null :
            response.GetDataAs(returnType);
    }

    public async Task<EventSubscriptionHandle> SubscribeAsync<TArgs, TData>(
        IEvent<TArgs, TData> theEvent,
        Action<TData> eventHandler,
        CancellationToken ctk = default) {
        
        var eventListener = EventListener.Create(theEvent, eventHandler);
        _payloadGateway.AddEventListener(eventListener);

        // READY event doesn't need a subscription command
        if (theEvent is not ReadyEvent) {
            await _payloadGateway.SendPayloadAsync(new Payload {
                cmd = "SUBSCRIBE",
                nonce = Guid.NewGuid().ToString(),
                evt = theEvent.Name,
                args = theEvent.Arguments
            }, ctk).ConfigureAwait(false);
        }

        return new EventSubscriptionHandle(Unsubscribe, UnsubscribeAsync);

        // Local helper methods
        
        Payload MakeUnsubscribePayload() => new() {
            cmd = "UNSUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        void Unsubscribe() {
            if (theEvent is not ReadyEvent) {
                _payloadGateway.SendPayload(MakeUnsubscribePayload());
            }
        }

        async ValueTask UnsubscribeAsync() {
            if (theEvent is not ReadyEvent) {
                await _payloadGateway.SendPayloadAsync(MakeUnsubscribePayload(), ctk)
                    .ConfigureAwait(false);
            }
        }
    }

    public void Dispose() {
        _payloadGateway.Dispose();
    }
}