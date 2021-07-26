using System.IO;
using System.Text.Json;

namespace Dec.DiscordIPC {
    public class ErrorResponseException : IOException {
        public override string Message { get; }
        public int Code { get; private set; }

        public ErrorResponseException(JsonElement response) {
            var data = response.GetProperty("data");
            Code = data.GetProperty("code").GetInt32();
            Message = data.GetProperty("message").GetString();
        }
    }
}
