using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dec.DiscordIPC.Entities.Objects;

// TODO
namespace Dec.DiscordIPC.Entities {
    public class Message {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("channel_id")]
        public string ChannelID { get; set; }
        
        [JsonPropertyName("guild_id")]
        public string GuildID { get; set; }
        
        [JsonPropertyName("author")]
        public User Author { get; set; }
        
        [JsonPropertyName("member")]
        public Member Member { get; set; }
        
        [JsonPropertyName("content")]
        public string Content { get; set; }
        
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        
        [JsonPropertyName("edited_timestamp")]
        public string EditedTimestamp { get; set; }
        
        [JsonPropertyName("tts")]
        public bool? TTS { get; set; }
        
        [JsonPropertyName("mention_everyone")]
        public bool? MentionEveryone { get; set; }
        
        // mentions
        [JsonPropertyName("mention_roles")]
        public List<string> MentionRoles { get; set; }
        
        [JsonPropertyName("mention_channels")]
        public List<Mention> MentionChannels { get; set; }
        
        [JsonPropertyName("attachments")]
        public List<Attachment> Attachments { get; set; }
        
        [JsonPropertyName("embeds")]
        public List<Embed> Embeds { get; set; }
        
        [JsonPropertyName("reactions")]
        public List<Reaction> Reactions { get; set; }
        
        [JsonPropertyName("nonce")]
        public object Nonce { get; set; }
        
        [JsonPropertyName("pinned")]
        public bool? Pinned { get; set; }
        
        [JsonPropertyName("webhook_id")]
        public string WebhookID { get; set; }
        
        [JsonPropertyName("type")]
        public int? Type { get; set; }
        
        [JsonPropertyName("activity")]
        public Activity activity { get; set; }
        
        [JsonPropertyName("application")]
        public Application Application { get; set; }
        
        [JsonPropertyName("application_id")]
        public string ApplicationID { get; set; }
        
        [JsonPropertyName("message_reference")]
        public Reference MessageReference { get; set; }
        
        [JsonPropertyName("flags")]
        public int? Flags { get; set; }
        
        [JsonPropertyName("references_message")]
        public Message ReferencedMessage { get; set; }
        
        // interaction
        [JsonPropertyName("thread")]
        public Channel Thread { get; set; }
        
        // components
        [JsonPropertyName("sticker_items")]
        public List<Sticker.Item> StickerItems { get; set; }
        // [Deprecated] stickers
        
        [JsonPropertyName("blocked")]
        public bool? Blocked { get; set; }
        
        [JsonPropertyName("content_parsed")]
        public List<ContentParsed> content_parsed { get; set; }
        
        [JsonPropertyName("author_color")]
        public string AuthorColor { get; set; }
        
        public class ContentParsed {
            [JsonPropertyName("content")]
            public dynamic Content { get; set; }
            
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
        
        public class Reaction {
            [JsonPropertyName("count")]
            public int? Count { get; set; }
            
            [JsonPropertyName("me")]
            public bool? Me { get; set; }
            
            [JsonPropertyName("emoji")]
            public Emoji Emoji { get; set; }
        }
        
        public class Attachment {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            
            [JsonPropertyName("filename")]
            public string Filename { get; set; }
            
            [JsonPropertyName("content_type")]
            public string ContentType { get; set; }
            
            [JsonPropertyName("size")]
            public int? Size { get; set; }
            
            [JsonPropertyName("url")]
            public string URL { get; set; }
            
            [JsonPropertyName("proxy_url")]
            public string ProxyURL { get; set; }
            
            [JsonPropertyName("height")]
            public int? Height { get; set; }
            
            [JsonPropertyName("width")]
            public int? Width { get; set; }
        }

        public class Activity {
            [JsonPropertyName("type")]
            public int? Type { get; set; }
            
            [JsonPropertyName("party_id")]
            public string PartyID { get; set; }
            
            public class Types {
                public static readonly int
                    JOIN = 1,
                    SPECTATE = 2,
                    LISTEN = 3,
                    JOIN_REQUEST = 5;
            }
        }
        
        public class Reference {
            [JsonPropertyName("message_id")]
            public string MessageID { get; set; }
            
            [JsonPropertyName("channel_id")]
            public string ChannelID { get; set; }
            
            [JsonPropertyName("guild_id")]
            public string GuildID { get; set; }
            
            [JsonPropertyName("fail_if_not_exists")]
            public bool? FailIfNotExists { get; set; }
        }
    }
}
