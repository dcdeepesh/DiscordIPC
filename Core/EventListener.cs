using System;
using System.Text.Json;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class EventListener<TArgs, TData> : AbstractEventListener {
    public Action<TData> EventHandler { get; }
    public override string EventName { get; }
    public Func<TData, bool> DataMatchChecker { get; }

    public EventListener(IEvent<TArgs, TData> theEvent, Action<TData> eventHandler) {
        EventHandler = eventHandler;
        EventName = theEvent.Name;
        DataMatchChecker = theEvent.IsMatchingData;
    }

    public override bool IsMatchingData(IpcPayload payload, JsonElement serializedEventData) =>
        payload.evt == EventName && DataMatchChecker(serializedEventData.ToObject<TData>());

    public override void HandleData(JsonElement serializedEventData) =>
        EventHandler(serializedEventData.ToObject<TData>());
}