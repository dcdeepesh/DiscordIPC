using System.IO;
using Dec.DiscordIPC.Core;

namespace Dec.DiscordIPC; 

/// <summary>
/// Exception thrown when IPC returns an error response.
/// </summary>
public class ErrorResponseException : IOException {
    /// <summary>
    /// The error message
    /// </summary>
    public override string Message { get; }
    /// <summary>
    /// The error code
    /// </summary>
    public int Code { get; }

    internal ErrorResponseException(IpcPayload response) {
        Code = response.data.code;
        Message = response.data.message;
    }
}