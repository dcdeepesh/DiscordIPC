﻿using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class NotificationCreateEvent : IEvent<object, NotificationCreateEvent.Data> {
    public string Name => "NOTIFICATION_CREATE";
    // No arguments; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static NotificationCreateEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string channel_id { get; set; }
        public Message message { get; set; }
        public string icon_url { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}