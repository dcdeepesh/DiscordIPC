using System.IO;
using System.Text.Json;

namespace Dec.DiscordIPC {
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
        public int Code { get; private set; }

        internal ErrorResponseException(JsonElement response) {
            var data = response.GetProperty("data");
            Code = data.GetProperty("code").GetInt32();
            Message = data.GetProperty("message").GetString();
        }
    }
}
