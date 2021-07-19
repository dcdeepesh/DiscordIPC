namespace Dec.DiscordIPC {
    public class ErrorResponse {
        public string cmd { get; set; }
        public string nonce { get; set; }
        public string evt { get; set; }
        public Data data { get; set; }

        public class Data {
            public int code { get; set; }
            public string message { get; set; }
        }
    }
}
