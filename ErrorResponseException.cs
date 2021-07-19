using System.IO;

namespace Dec.DiscordIPC {
    public class ErrorResponseException : IOException {
        public ErrorResponse Response { get; private set; }

        public override string Message => Response.data.message;
        public int Code => Response.data.code;

        public ErrorResponseException(ErrorResponse response) {
            Response = response;
        }
    }
}
