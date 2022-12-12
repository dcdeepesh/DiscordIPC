using System;
using System.Text.Json;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC; 

/// <summary>
/// The main access point for user programs to use DiscordIPC.
/// </summary>
public class DiscordIpcClient : IpcHandler {
    /// <summary>
    /// Creates a client instance.
    /// </summary>
    /// <remarks>
    /// Does not actually initialize the client.
    /// Use <see cref="IpcHandler.InitAsync"/> after this to initialize the client.
    /// </remarks>
    /// <param name="clientId">Client ID of your app.</param>
    /// <param name="verbose">If true, DiscordIPC logs every JSON
    /// sent and received to the console.</param>
    public DiscordIpcClient(string clientId, bool verbose = false) : base(clientId, verbose) { }

    public async Task<TData> SendCommandAsync<TArgs, TData>(ICommand<TArgs, TData> command) =>
        (TData) await SendCommandAsync(command, typeof(TData));

    public async Task SendCommandAsync<TArgs>(ICommand<TArgs> command) =>
        await SendCommandAsync(command, null);

    private async Task<object> SendCommandAsync<TArgs>(ICommand<TArgs> command,
        Type returnType) {
            
        IpcPayload payload = new() {
            cmd = command.Name,
            nonce = Guid.NewGuid().ToString(),
            args = command.Arguments
        };
            
        JsonElement response = await SendPayloadAsync(payload);
        return returnType is null ? null : response.GetProperty("data").ToObject(returnType);
    }

    public async Task SubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
        IpcPayload payload = new() {
            cmd = "SUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        await SendPayloadAsync(payload);
    }

    public async Task UnsubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
        IpcPayload payload = new() {
            cmd = "UNSUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        await SendPayloadAsync(payload);
    }
}