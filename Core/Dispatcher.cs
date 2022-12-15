using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace Dec.DiscordIPC.Core; 

public class Dispatcher {
    private readonly List<AbstractEventListener> _eventListeners = new();
    private readonly LinkedList<Waiter> _responseWaiters = new();
    private readonly LinkedList<IpcPayload> _pooledResponsePayloads = new();

    public void AddEventListener(AbstractEventListener eventListener) {
        _eventListeners.Add(eventListener);
    }

    public void DispatchEvent(IpcPayload eventPayload, JsonElement serializedEventData) {
        foreach (var listener in _eventListeners) {
            if (listener.IsMatchingData(eventPayload, serializedEventData)) {
                listener.HandleData(serializedEventData);
            }
        }
    }

    public void DispatchResponse(IpcPayload responsePayload) {
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

    public IpcPayload WaitForResponse(string nonce) {
        IpcPayload response;

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

        if (response.IsErrorResponse())
            throw new ErrorResponseException(response);
        return response;
    }
}

internal class Waiter {
    // TODO: Use ManualResetEvent? Use *Slim? Check performance.
    private readonly AutoResetEvent _event = new(false);
    private IpcPayload _response;

    public string Nonce { get; }
    
    public Waiter(string nonce) => Nonce = nonce;

    public IpcPayload WaitForResponse() {
        _event.WaitOne();
        return _response;
    }

    public void Notify(IpcPayload response) {
        _response = response;
        _event.Set();
    }
}