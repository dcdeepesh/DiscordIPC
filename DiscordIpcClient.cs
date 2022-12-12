using System;
using System.Text.Json;
using System.Threading;
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
    
    public async Task InitAsync(int pipeNumber = 0, int timeoutMs = 2000,
        CancellationToken ctk = default) {

        await ConnectToPipeAsync(pipeNumber, timeoutMs, ctk);
        await WaitForReadyEventAsync(ctk);
    }

    public new async Task ConnectToPipeAsync(int pipeNumber = 0, int timeoutMs = 2000, CancellationToken ctk = default) =>
        await base.ConnectToPipeAsync(pipeNumber, timeoutMs, ctk);
    
    public async Task WaitForReadyEventAsync(CancellationToken ctk = default) {
        EventWaitHandle readyEventWaitHandle = new(false, EventResetMode.ManualReset);
        OnReady += ReadyEventListener;

        // TODO: use ctk
        await SendPacketAsync(new IpcRawPacket(OpCode.Handshake, new {
            client_id = clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }));

        // TODO: make this async
        await Task.Run(() => {
            readyEventWaitHandle.WaitOne();
            OnReady -= ReadyEventListener;
        }, ctk);

        void ReadyEventListener(object sender, ReadyEvent.Data data) {
            readyEventWaitHandle.Set();
        }
    }

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