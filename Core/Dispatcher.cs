﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core; 

public class Dispatcher {
    private readonly List<AbstractEventListener> _eventListeners = new();
    private readonly LinkedList<Waiter> _waiters = new();
    private readonly LinkedList<IpcPayload> _responses = new();

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
        lock (_responses) {
            // TODO: use Single() instead?
            Waiter waiterToResume = _waiters.FirstOrDefault(
                w => w.Nonce == responsePayload.nonce);

            if (waiterToResume is not null) {
                _waiters.Remove(waiterToResume);
                waiterToResume.Response = responsePayload;
                waiterToResume.ResetEvent.Set();
            } else {
                _responses.AddLast(responsePayload);
            }
        }
    }
    
    public Task<IpcPayload> WaitForResponse(string nonce) {
        return Task.Run(() => {
            Waiter waiter;
            lock (_responses) {
                IpcPayload result = null;
                // TODO: use LINQ and Single() instead?
                foreach (var response in _responses)
                    if (response.nonce == nonce)
                        result = response;
                if (result is not null) {
                    _responses.Remove(result);
                    if (result.IsErrorResponse())
                        throw new ErrorResponseException(result);
                    return result;
                }

                waiter = new Waiter(nonce);
                _waiters.AddLast(waiter);
            }

            waiter.ResetEvent.WaitOne();
            if (waiter.Response.IsErrorResponse())
                throw new ErrorResponseException(waiter.Response);
            return waiter.Response;
        });
    }
}