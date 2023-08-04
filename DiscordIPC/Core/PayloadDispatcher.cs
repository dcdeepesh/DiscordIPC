using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dec.DiscordIPC.Core.Ipc;

namespace Dec.DiscordIPC.Core; 

public class PayloadDispatcher {
    
    private readonly List<EventListener> _eventListeners = new();
    private readonly LinkedList<ResponseWaiter> _responseWaiters = new();
    private readonly LinkedList<Payload> _pooledResponsePayloads = new();
    
    public void DispatchEvent(Payload eventPayload) {
        foreach (var listener in _eventListeners
                     .Where(el => el.IsMatchingEventData(eventPayload))) {
            listener.HandleEventData(eventPayload);
        }
    }

    public void DispatchResponse(Payload responsePayload) {
        ResponseWaiter existingResponseWaiter = _responseWaiters.FirstOrDefault(
            rw => rw.Nonce == responsePayload.nonce);

        if (existingResponseWaiter is not null) {
            _responseWaiters.Remove(existingResponseWaiter);
            existingResponseWaiter.Notify(responsePayload);
        } else {
            lock (_pooledResponsePayloads) {
                _pooledResponsePayloads.AddLast(responsePayload);
            }
        }
    }
    
    public void AddEventListener(EventListener eventListener) {
        _eventListeners.Add(eventListener);
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
            ResponseWaiter responseWaiter = new(nonce);
            _responseWaiters.AddLast(responseWaiter);
            response = responseWaiter.WaitForResponse();
        }

        if (response.evt == "ERROR")
            throw new ErrorResponseException(response);
        return response;
    }
    
    private class ResponseWaiter {
        // TODO: Use ManualResetEvent? Use *Slim? Check performance.
        private readonly AutoResetEvent _event = new(false);
        private Payload _responsePayload;

        public string Nonce { get; }
    
        public ResponseWaiter(string nonce) => Nonce = nonce;

        public Payload WaitForResponse() {
            _event.WaitOne();
            return _responsePayload;
        }

        public void Notify(Payload response) {
            _responsePayload = response;
            _event.Set();
        }
    }
}