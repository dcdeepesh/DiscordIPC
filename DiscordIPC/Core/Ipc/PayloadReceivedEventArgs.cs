using System;

namespace Dec.DiscordIPC.Core.Ipc;

internal class PayloadReceivedEventArgs : EventArgs {

    public Payload Payload { get; }

    public PayloadReceivedEventArgs(Payload payload) => Payload = payload;
}