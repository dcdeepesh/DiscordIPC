using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Entities; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Emoji {
    public string id { get; set; }
    public string name { get; set; }
    public List<string> roles { get; set; }
    public User user { get; set; }
    public bool? require_colons { get; set; }
    public bool? managed { get; set; }
    public bool? animated { get; set; }
    public bool? available { get; set; }
}