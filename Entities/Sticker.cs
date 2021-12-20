namespace Dec.DiscordIPC.Entities {
    public class Sticker {
        public string id { get; set; }
        public string pack_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        // [Deprecated] asset
        public int? type { get; set; }
        public int? format_type { get; set; }
        public bool? available { get; set; }
        public string guild_id { get; set; }
        public User user { get; set; }
        public int? sort_value { get; set; }

        public class Item {
            public string id { get; set; }
            public string name { get; set; }
            public int? format_type { get; set; }
        }

        public class Type {
            public static readonly int
                STANDARD = 1,
                GUILD = 2;
        }

        public class FormatType {
            public static readonly int
                PNG = 1,
                APNG = 2,
                LOTTIE = 3;
        }
    }
}
