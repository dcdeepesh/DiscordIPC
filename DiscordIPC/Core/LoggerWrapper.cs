using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dec.DiscordIPC.Core; 

internal class LoggerWrapper {
    public static ILogger Logger { get; set; } = NullLogger.Instance;
}