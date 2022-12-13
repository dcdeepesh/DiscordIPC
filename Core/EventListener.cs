using System;
using System.Text.Json;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class EventListener<TData> : AbstractEventListener {
    public Action<TData> EventHandler { get; set; }
    public override string EventName { get; set; }
    public Func<TData, bool> DataMatchChecker { get; set; }

    public override bool IsMatchingData(IpcPayload payload, JsonElement serializedEventData) =>
        payload.evt == EventName && DataMatchChecker(serializedEventData.ToObject<TData>());

    public override void HandleData(JsonElement serializedEventData) =>
        EventHandler(serializedEventData.ToObject<TData>());
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