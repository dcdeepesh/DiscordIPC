using System.Collections.Generic;

// TODO
namespace Dec.DiscordIPC.Entities {
    public class Message {
        public string id { get; set; }
        public string channel_id { get; set; }
        public string guild_id { get; set; }
        public User author { get; set; }
        public Member member { get; set; }
        public string content { get; set; }
        public string timestamp { get; set; }
        public string edited_timestamp { get; set; }
        public bool tts { get; set; }
        public bool mention_everyone { get; set; }
        // mentions
        public List<string> mention_roles { get; set; }
        public List<Channel.Mention> mention_channels { get; set; }
        public List<Attachment> attachments { get; set; }
        public List<Embed> embeds { get; set; }
        public List<Reaction> reactions { get; set; }
        public object nonce { get; set; }
        public bool pinned { get; set; }
        public string webhook_id { get; set; }
        public int type { get; set; }
        public Activity activity { get; set; }
        public Application application { get; set; }
        public string application_id { get; set; }
        public Reference message_reference { get; set; }
        public int flags { get; set; }
        public Message referenced_message { get; set; }
        // interaction
        public Channel thread { get; set; }
        // componenets
        public List<Sticker.Item> sticker_items { get; set; }
        // [Deprecated] stickers


        public bool blocked { get; set; }
        public List<ContentParsed> content_parsed { get; set; }
        public string author_color { get; set; }

        public class ContentParsed {
            public string content { get; set; }
            public string type { get; set; }
        }

        public class Reaction {
            public int count { get; set; }
            public bool me { get; set; }
            public Emoji emoji { get; set; }
        }

        public class Attachment {
            public string id { get; set; }
            public string filename { get; set; }
            public string content_type { get; set; }
            public int size { get; set; }
            public string url { get; set; }
            public string proxy_url { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

        public class Activity {
            public int type { get; set; }
            public string party_id { get; set; }

            public class Type {
                public static readonly int
                    JOIN = 1,
                    SPECTATE = 2,
                    LISTEN = 3,
                    JOIN_REQUEST = 5;
            }
        }

        public class Reference {
            public string message_id { get; set; }
            public string channel_id { get; set; }
            public string guild_id { get; set; }
            public bool fail_if_not_exists { get; set; }
        }
    }
}
