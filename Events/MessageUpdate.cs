using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class MessageUpdate {
        public class Args : MessageCreate.Args { }

        public class Data : MessageCreate.Data { }
    }
}
