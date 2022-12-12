﻿using System;
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
public class DiscordIpcClient {

    public readonly IpcHandler _ipcHandler;
    
    public DiscordIpcClient(string clientId, bool verbose = false) {
        _ipcHandler = new IpcHandler(clientId, verbose);
    }
    
    public async Task ConnectToDiscordAsync(int pipeNumber = 0, int timeoutMs = 2000,
        CancellationToken ctk = default) {

        await _ipcHandler.ConnectToPipeAsync(pipeNumber, timeoutMs, ctk);
        await _ipcHandler.SendHandshakeAsync(ctk);
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
            
        JsonElement response = await _ipcHandler.SendPayloadAsync(payload);
        return returnType is null ? null : response.GetProperty("data").ToObject(returnType);
    }

    public async Task SubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
        IpcPayload payload = new() {
            cmd = "SUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        await _ipcHandler.SendPayloadAsync(payload);
    }

    public async Task UnsubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
        IpcPayload payload = new() {
            cmd = "UNSUBSCRIBE",
            nonce = Guid.NewGuid().ToString(),
            evt = theEvent.Name,
            args = theEvent.Arguments
        };

        await _ipcHandler.SendPayloadAsync(payload);
    }
}