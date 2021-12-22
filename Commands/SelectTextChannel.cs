namespace Dec.DiscordIPC.Commands {
    public class SelectTextChannel {
        public class Args {
            public string channel_id { get; set; }
            public int? timeout { get; set; }
        }

        public class Data : GetChannel.Data { }
    }
}
