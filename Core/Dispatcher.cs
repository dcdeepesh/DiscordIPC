using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
        lock (_pooledResponsePayloads) {
            // TODO: use Single() instead?
            Waiter waiterToResume = _responseWaiters.FirstOrDefault(
                w => w.Nonce == responsePayload.nonce);

            if (waiterToResume is not null) {
                _responseWaiters.Remove(waiterToResume);
                waiterToResume.Response = responsePayload;
                waiterToResume.ResetEvent.Set();
            } else {
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

        if (response is null) {
            Waiter waiter = new(nonce);
            _responseWaiters.AddLast(waiter);
            waiter.ResetEvent.WaitOne();
            response = waiter.Response;
        }

        if (response.IsErrorResponse())
            throw new ErrorResponseException(response);
        return response;
    }
}