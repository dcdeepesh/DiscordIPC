using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class MessageDelete {
        public class Args : MessageCreate.Args { }

        public class Data : MessageCreate.Data { }
    }
}
