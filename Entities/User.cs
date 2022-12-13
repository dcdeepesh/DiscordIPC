using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Entities; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class User {
    public string id { get; set; }
    public string username { get; set; }
    public string discriminator { get; set; }
    public string avatar { get; set; }
    public bool? bot { get; set; }
    public bool? system { get; set; }
    public bool? mfa_enabled { get; set; }
    public string banner { get; set; }
    public int? accent_color { get; set; }
    public string locale { get; set; }
    public bool? verified { get; set; }
    public string email { get; set; }
    public int? flags { get; set; }
    public int? premium_type { get; set; }
    public int? public_flags { get; set; }

    public class Flags {
        public static readonly int
            None = 0,
            STAFF = 1 << 0,
            PARTNER = 1 << 1,
            HYPESQUAD = 1 << 2,
            BUG_HUNTER_LEVEL_1 = 1 << 3,
            HYPESQUAD_ONLINE_HOUSE_1 = 1 << 6,
            HYPESQUAD_ONLINE_HOUSE_2 = 1 << 7,
            HYPESQUAD_ONLINE_HOUSE_3 = 1 << 8,
            PREMIUM_EARLY_SUPPORTER = 1 << 9,
            TEAM_PSEUDO_USER = 1 << 10,
            BUG_HUNTER_LEVEL_2 = 1 << 14,
            VERIFIED_BOT = 1 << 16,
            VERIFIED_DEVELOPER = 1 << 17,
            CERTIFIED_MODERATOR = 1 << 18,
            BOT_HTTP_INTERACTIONS = 1 << 19;
    }

    public class PremiumType {
        public static readonly int
            None = 0,
            NitroClassic = 1,
            Nitro = 2;
    }
}