using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Entities; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Member {
    public User user { get; set; }
    public string nick { get; set; }
    public string avatar { get; set; }
    public List<string> roles { get; set; }
    public string joined_at { get; set; }
    public string premium_since { get; set; }
    public bool? deaf { get; set; }
    public bool? mute { get; set; }
    public bool? pending { get; set; }
    public string permissions { get; set; }
}