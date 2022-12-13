using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Core; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IpcPayload {
    public string nonce { get; set; }
    
    public string cmd { get; set; }
    public object args { get; set; }
     
    public string evt { get; set; }
    public dynamic data { get; set; }
}
