using System.IO;
using System.Text.Json;

namespace Dec.DiscordIPC {
    public class ErrorResponseException : IOException {
        public override string Message { get; }
        public int Code { get; private set; }
        
        public ErrorResponseException(JsonElement response) {
            JsonElement data = response.GetProperty("data");
            this.Code = data.GetProperty("code").GetInt32();
            this.Message = data.GetProperty("message").GetString();
        }
    }
}
