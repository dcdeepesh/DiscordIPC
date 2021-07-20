using System.Collections.Generic;

namespace Dec.DiscordIPC.Entities {
    public class Role {
        public string id { get; set; }
        public string name { get; set; }
        public int color { get; set; }
        public bool hoist { get; set; }
        public int position { get; set; }
        public string permissions { get; set; }
        public bool managed { get; set; }
        public bool mentionable { get; set; }
        public List<Tag> tags { get; set; }

        public class Tag {
            public string bot_id { get; set; }
            public string integration_id { get; set; }
            public object premium_subscriber { get; set; }
        }
    }
}
