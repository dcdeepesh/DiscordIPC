using System;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class EventListener<TArgs> : AbstractEventListener {
    public Action EventHandler { get; }
    public override string EventName { get; }

    public EventListener(IEvent<TArgs> theEvent, Action eventHandler) {
        EventHandler = eventHandler;
        EventName = theEvent.Name;
    }

    public override bool IsMatchingData(object data) => true;
    public override void HandleData(object dataObj) => EventHandler();
}

public class EventListener<TArgs, TData> : AbstractEventListener {
    public Action<TData> EventHandler { get; }
    public override string EventName { get; }

    public EventListener(IEvent<TArgs, TData> theEvent, Action<TData> eventHandler) {
        EventHandler = eventHandler;
        EventName = theEvent.Name;
    }

    public override bool IsMatchingData(object data) {
        if (data is TData) {
            return true;
        }
        
        return false;
    }

    public override void HandleData(object dataObj) {
        TData data = (TData) dataObj;
        EventHandler(data);
    }
}