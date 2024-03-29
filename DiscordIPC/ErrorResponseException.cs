﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using Dec.DiscordIPC.Core.Ipc;

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

    internal ErrorResponseException(Payload responsePayload) {
        ErrorPayload response = responsePayload.GetDataAs<ErrorPayload>();
        Code = response.code;
        Message = response.message;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class ErrorPayload {
        public int code { get; set; }
        public string message { get; set; }
    }
}