using System.Diagnostics.CodeAnalysis;
using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Events; 

public class ReadyEvent : IEvent<object, ReadyEvent.Data> {
    public string Name => "READY";
    // No args; dummy
    public object Arguments { get; set; }
    
    public bool IsMatchingData(Data _) => true;

    public static ReadyEvent Create() => new();

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public int? v { get; set; }
        public RPCServerConfig config { get; set; }
        public User user { get; set; }

        public class RPCServerConfig {
            public string cdn_host { get; set; }
            public string api_endpoint { get; set; }
            public string environment { get; set; }
        }
    }
}