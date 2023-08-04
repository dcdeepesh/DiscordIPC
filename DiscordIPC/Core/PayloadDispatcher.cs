using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dec.DiscordIPC.Core.Ipc;

namespace Dec.DiscordIPC.Core;

public class PayloadDispatcher {
    private readonly List<EventListener> _eventListeners = new();
    private readonly LinkedList<Waiter> _responseWaiters = new();
    private readonly LinkedList<IpcPacketPayload> _pooledResponsePayloads = new();

    public void AddEventListener(EventListener eventListener) {
        _eventListeners.Add(eventListener);
    }

    public void DispatchEvent(IpcPacketPayload eventPayload) {
        foreach (var listener in _eventListeners) {
            if (listener.IsMatchingData(eventPayload)) {
                listener.HandleData(eventPayload);
            }
        }
    }

    public void DispatchResponse(IpcPacketPayload responsePayload) {
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

    public IpcPacketPayload GetResponseFor(string nonce) {
        IpcPacketPayload response;

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
}

internal class Waiter {
    // TODO: Use ManualResetEvent? Use *Slim? Check performance.
    private readonly AutoResetEvent _event = new(false);
    private IpcPacketPayload _responsePayload;

    public string Nonce { get; }
    
    public Waiter(string nonce) => Nonce = nonce;

    public IpcPacketPayload WaitForResponse() {
        _event.WaitOne();
        return _responsePayload;
    }

    public void Notify(IpcPacketPayload response) {
        _responsePayload = response;
        _event.Set();
    }
}