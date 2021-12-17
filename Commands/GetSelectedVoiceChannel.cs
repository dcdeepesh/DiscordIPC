using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Development;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    /// <summary>
    /// Used to get the current voice channel the client is in
    /// </summary>
    public class GetSelectedVoiceChannel {
        [DiscordRPC("GET_SELECTED_VOICE_CHANNEL")]
        public class Args : IPayloadResponse<Data>, IDummyCommandArgs { }
        
        public class Data : Channel { }
    }
}
