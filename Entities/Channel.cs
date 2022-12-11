using System.Collections.Generic;

namespace Dec.DiscordIPC.Entities; 

public class Channel {
    public string id { get; set; }
    public int? type { get; set; }
    public string guild_id { get; set; }
    public int? position { get; set; }
    // permission_overwrites
    public string name { get; set; }
    public string topic { get; set; }
    public bool? nsfw { get; set; }
    public string last_message_id { get; set; }

    public int? bitrate { get; set; }
    public int? user_limit { get; set; }

    public List<VoiceState> voice_states { get; set; }
    public List<Message> messages { get; set; }

    public class Mention {
        public string id { get; set; }
        public string guild_id { get; set; }
        public int? type { get; set; }
        public string name { get; set; }
    }

    public class Type {
        public static readonly int
            GUILD_TEXT = 0,
            DM = 1,
            GUILD_VOICE = 2,
            GROUP_DM = 3,
            GUILD_CATEGORY = 4,
            GUILD_NEWS = 5,
            GUILD_STORE = 6,
            GUILD_NEWS_THREAD = 10,
            GUILD_PUBLIC_THREAD = 11,
            GUILD_PRIVATE_THREAD = 12,
            GUILD_STAGE_VOICE = 13;
    }
}