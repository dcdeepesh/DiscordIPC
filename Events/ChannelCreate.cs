namespace Dec.DiscordIPC.Events {
    public class ChannelCreate {
        // No arguments; dummy
        public class Args { }

        public class Data {
            public string id { get; set; }
            public string name { get; set; }
            public int type { get; set; }
        }
    }
}
