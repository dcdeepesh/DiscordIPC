﻿using System.Collections.Generic;

// Done
namespace Dec.DiscordIPC.Entities {
    public class Presence {
        public User user { get; set; }
        public string guild_id { get; set; }
        public string status { get; set; }
        public List<Activity> activities { get; set; }
        public ClientStatus client_status { get; set; }

        public class ClientStatus {
            public string desktop { get; set; }
            public string mobile { get; set; }
            public string web { get; set; }
        }

        public class Activity {
            public string name { get; set; }
            public int type { get; set; }
            public string url { get; set; }
            public int created_at { get; set; }
            public Timestamps timestamps { get; set; }
            public string application_id { get; set; }
            public string details { get; set; }
            public string state { get; set; }
            public Emoji emoji { get; set; }
            public Party party { get; set; }
            public Assets assets { get; set; }
            public Secrets secrets { get; set; }
            public bool instance { get; set; }
            public int flags { get; set; }
            public List<Button> buttons { get; set; }

            public class Timestamps {
                public int start { get; set; }
                public int end { get; set; }
            }

            public class Emoji {
                public string name { get; set; }
                public string id { get; set; }
                public bool animated { get; set; }
            }

            public class Party {
                public string id { get; set; }
                public List<int> size { get; set; }
                public int current_size => size[0];
                public int max_size => size[1];
            }

            public class Assets {
                public string large_image { get; set; }
                public string large_text { get; set; }
                public string small_image { get; set; }
                public string small_text { get; set; }
            }

            public class Secrets {
                public string join { get; set; }
                public string spectate { get; set; }
                public string match { get; set; }
            }

            public class Button {
                public string label { get; set; }
                public string url { get; set; }
            }
        }
    }
}