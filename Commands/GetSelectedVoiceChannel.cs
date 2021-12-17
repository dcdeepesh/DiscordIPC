using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to get the current voice channel the client is in
    /// </summary>
    public class GetSelectedVoiceChannel {
        // No arguments; dummy
        public class Args : IDummyCommandArgs { }
        
        public class Data : Channel { }
    }
}
