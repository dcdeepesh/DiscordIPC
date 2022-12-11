namespace Dec.DiscordIPC.Events {
    public class GuildCreateEvent {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}
