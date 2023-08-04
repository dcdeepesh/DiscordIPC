using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core.Ipc;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IpcPacketPayload
{
    public string nonce { get; set; }

    public string cmd { get; set; }
    public object args { get; set; }

    public string evt { get; set; }
    public dynamic data { get; set; }

    [JsonIgnore]
    public string DataJson { get; set; }

    public TData GetDataAs<TData>() =>
        JsonSerializer.Deserialize<TData>(DataJson);

    public object GetDataAs(Type returnType) =>
        JsonSerializer.Deserialize(DataJson, returnType);
}
