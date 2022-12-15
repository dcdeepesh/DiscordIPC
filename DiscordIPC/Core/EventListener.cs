using System;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class EventListener<TData> : AbstractEventListener {
    public Action<TData> EventHandler { get; set; }
    public string EventName { get; set; }
    public Func<TData, bool> DataMatchChecker { get; set; }

    public override bool IsMatchingData(IpcPayload payload) =>
        payload.evt == EventName && DataMatchChecker(payload.GetData<TData>());

    public override void HandleData(IpcPayload payload) =>
        EventHandler(payload.GetData<TData>());
}

public class EventListener {
    public static EventListener<TData> Create<TArgs, TData>(
        IEvent<TArgs, TData> theEvent, Action<TData> eventHandler) {
        
        return new EventListener<TData> {
            EventHandler = eventHandler,
            EventName = theEvent.Name,
            DataMatchChecker = theEvent.IsMatchingData
        };
    }
}