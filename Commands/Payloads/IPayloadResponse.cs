using Dec.DiscordIPC.Commands.Interfaces;

namespace Dec.DiscordIPC.Commands.Payloads {
    public interface IPayloadResponse : IPayloadResponse<object> {}
    public interface IPayloadResponse<T> : ICommandArgs {}
}