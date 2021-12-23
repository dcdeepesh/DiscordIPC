using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Entities {
    public class Team {
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("members")]
        public List<Member> Members { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("owner_user_id")]
        public string OwnerUserID { get; set; }
        
        public class Member {
            [JsonPropertyName("membership_state")]
            public int? MembershipState { get; set; }
            
            [JsonPropertyName("permissions")]
            public List<string> Permissions { get; set; }
            
            [JsonPropertyName("team_id")]
            public string TeamID { get; set; }
            
            [JsonPropertyName("user")]
            public User User { get; set; }

            public class MembershipStates {
                public static readonly int
                    INVITED = 1,
                    ACCEPTED = 2;
            }
        }
    }
}
