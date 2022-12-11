namespace Dec.DiscordIPC.Core; 

public class IpcPayload {
    public string nonce { get; set; }
    
    public string cmd { get; set; }
    public object args { get; set; }
     
    public string evt { get; set; }
    public object data { get; set; }
}
