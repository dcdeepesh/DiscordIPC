using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dec.DiscordIPC; 

public class DiscordIpcClientOptions {
    public int PipeNumber { get; set; } = 0;
    // TODO: implement this limit
    public int ResponsePoolLimit { get; set; } = 32;
    public ILogger Logger { get; set; } = NullLogger.Instance;
}