using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events {
    public class VoiceStateDelete {
        public class Args : VoiceStateCreate.Args { }

        public class Data : VoiceStateCreate.Data { }
    }
}
