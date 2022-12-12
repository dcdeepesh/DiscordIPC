using System;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core; 

public class EventListener<TArgs, TData> {
    public IEvent<TArgs, TData> theEvent { get; set; }
    public Action<TData> EventHandler { get; set; }
}