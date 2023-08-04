using System;

namespace Dec.DiscordIPC.Core.Ipc;

internal class MessageReceivedEventArgs : EventArgs {

    public IpcPacketPayload Payload { get; }

    public MessageReceivedEventArgs(IpcPacketPayload payload) => Payload = payload;
}