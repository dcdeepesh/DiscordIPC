using Dec.DiscordIPC.Core.Ipc;

namespace Dec.DiscordIPC.Core;

public abstract class AbstractEventListener {
    public abstract bool IsMatchingData(IpcPacketPayload eventPayload);
    public abstract void HandleData(IpcPacketPayload eventPayload);
}