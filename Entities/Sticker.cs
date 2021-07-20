// Done
namespace Dec.DiscordIPC.Entities {
    public class Sticker {
        public static readonly int PNG = 1;
        public static readonly int APNG = 2;
        public static readonly int LOTTIE = 3;

        public string id { get; set; }
        public string pack_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        // [Deprecated] asset
        public int format_type { get; set; }
        public bool available { get; set; }
        public string guild_id { get; set; }
        public User user { get; set; }
        public int sort_value { get; set; }

        public class Item {
            public string id { get; set; }
            public string name { get; set; }
            public int format_type { get; set; }
        }
    }
}
