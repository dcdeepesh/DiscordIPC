using System.Collections.Generic;

namespace Dec.DiscordIPC.Entities {
    public class Team {
        public string icon { get; set; }
        public string id { get; set; }
        public List<Member> members { get; set; }
        public string name { get; set; }
        public string owner_user_id { get; set; }

        public class Member {
            public int? membership_state { get; set; }
            public List<string> permissions { get; set; }
            public string team_id { get; set; }
            public User user { get; set; }

            public class MembershipState {
                public static readonly int
                    INVITED = 1,
                    ACCEPTED = 2;
            }
        }
    }
}
