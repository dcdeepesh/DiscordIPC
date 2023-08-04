using System;
using Dec.DiscordIPC.Core.Ipc;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public abstract class EventListener {
    public static SpecificEventListener<TData> Create<TArgs, TData>(
        IEvent<TArgs, TData> theEvent, Action<TData> eventHandler) {
        
        return new SpecificEventListener<TData> {
            EventHandler = eventHandler,
            EventName = theEvent.Name,
            DataMatchChecker = theEvent.IsMatchingData
        };
    }

    public abstract bool IsMatchingData(Payload payload);
    public abstract void HandleData(Payload payload);

    public class SpecificEventListener<TEventData> : EventListener {
        public string EventName { get; set; }
        public Action<TEventData> EventHandler { get; set; }
        public Func<TEventData, bool> DataMatchChecker { get; set; }

        public override bool IsMatchingData(Payload payload) =>
            payload.evt == EventName && DataMatchChecker(payload.GetDataAs<TEventData>());

        public override void HandleData(Payload payload) =>
            EventHandler(payload.GetDataAs<TEventData>());
    }
}