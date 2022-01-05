using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Commands {
    public class GetSelectedVoiceChannel {
        // No arguments; dummy
        public class Args { }

        // Documentation states that it returns GetChannel.Data
        // but it actually returns VoiceStateCreate.Data
        public class Data : VoiceStateCreate.Data { }
    }
}
