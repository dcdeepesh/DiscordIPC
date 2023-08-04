using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dec.DiscordIPC.Core.Ipc;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class PayloadGateway {
    private readonly List<EventListener> _eventListeners = new();
    private readonly LinkedList<Waiter> _responseWaiters = new();
    private readonly LinkedList<Payload> _pooledResponsePayloads = new();
    
    private readonly string _clientId;
    private readonly IpcClient _ipcClient;
    private PayloadReadLoop _messageLoop;

    public PayloadGateway(string clientId) {
        _clientId = clientId;
        _ipcClient = new IpcClient();
    }
    
    public async Task InitComponentsAsync(int pipeNumber, int timeoutMs, CancellationToken ctk)  {
        // initialize the IPC client
        await _ipcClient.ConnectToPipeAsync(pipeNumber, timeoutMs, ctk)
            .ConfigureAwait(false);
        
        // Initialize the message loop and prepare dispatch
        _messageLoop = new PayloadReadLoop(_ipcClient.Pipe);
        _messageLoop.EventReceived += (_, args) => DispatchEvent(args.Payload);
        _messageLoop.ResponseReceived += (_, args) => DispatchResponse(args.Payload);
        _messageLoop.Start();
    }
    
    public async Task PerformHandshakeAsync(CancellationToken ctk = default) {
        EventWaitHandle readyEventWaitHandle = new(false, EventResetMode.ManualReset);
        AddEventListener(EventListener.Create(ReadyEvent.Create(), ReadyEventListener));

        // TODO: use ctk
        await _ipcClient.SendPacketAsync(new Packet(OpCode.Handshake, new
        {
            client_id = _clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }), ctk).ConfigureAwait(false);

        // TODO: make this async
        await Task.Run(() =>
        {
            readyEventWaitHandle.WaitOne();
        }, ctk).ConfigureAwait(false);

        void ReadyEventListener(ReadyEvent.Data _)
        {
            readyEventWaitHandle.Set();
        }
    }

    public void AddEventListener(EventListener eventListener) {
        _eventListeners.Add(eventListener);
    }

    #region Methods to send data (payloads) to the IPC
    
    public Payload SendPayload(Payload payload)  {
        _ipcClient.SendPacket(new Packet(OpCode.Frame, payload));
        return WaitAndGetResponseFor(payload.nonce);
    }

    public async Task<Payload> SendPayloadAsync(Payload payload,
        CancellationToken ctk = default) {
        
        await _ipcClient.SendPacketAsync(new Packet(OpCode.Frame, payload), ctk)
            .ConfigureAwait(false);
        return WaitAndGetResponseFor(payload.nonce);
    }
    
    public Payload WaitAndGetResponseFor(string nonce) {
        Payload response;

        lock (_pooledResponsePayloads) {
            response = _pooledResponsePayloads
                .FirstOrDefault(res => res.nonce == nonce);
            if (response is not null) {
                _pooledResponsePayloads.Remove(response);
            }
        }
        
        // TODO: what happens when response is received and added right here?
        // TODO: Does the lock() above need to be extended?
        // TODO: Does _responseWaiters need a lock?

        if (response is null) {
            Waiter waiter = new(nonce);
            _responseWaiters.AddLast(waiter);
            response = waiter.WaitForResponse();
        }

        if (response.evt == "ERROR")
            throw new ErrorResponseException(response);
        return response;
    }
    
    #endregion

    #region Methods to receive data from the IPC (dispatch methods)
    
    public void DispatchEvent(Payload eventPayload) {
        foreach (var listener in _eventListeners) {
            if (listener.IsMatchingData(eventPayload)) {
                listener.HandleData(eventPayload);
            }
        }
    }

    public void DispatchResponse(Payload responsePayload) {
        Waiter existingWaiter = _responseWaiters.FirstOrDefault(
            w => w.Nonce == responsePayload.nonce);

        if (existingWaiter is not null) {
            _responseWaiters.Remove(existingWaiter);
            existingWaiter.Notify(responsePayload);
        } else {
            lock (_pooledResponsePayloads) {
                _pooledResponsePayloads.AddLast(responsePayload);
            }
        }
    }
    
    #endregion

    public void Dispose() {
        _ipcClient.Dispose();
    }
}

internal class Waiter {
    // TODO: Use ManualResetEvent? Use *Slim? Check performance.
    private readonly AutoResetEvent _event = new(false);
    private Payload _responsePayload;

    public string Nonce { get; }
    
    public Waiter(string nonce) => Nonce = nonce;

    public Payload WaitForResponse() {
        _event.WaitOne();
        return _responsePayload;
    }

    public void Notify(Payload response) {
        _responsePayload = response;
        _event.Set();
    }
}