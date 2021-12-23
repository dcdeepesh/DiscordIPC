using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core {
    public delegate Task IPCHello<in T>(T arg, CancellationToken cancellationToken = default);
}