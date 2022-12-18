﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IpcPayload {
    public string nonce { get; set; }
    
    public string cmd { get; set; }
    public object args { get; set; }
     
    public string evt { get; set; }
    public dynamic data { get; set; }
    
    [JsonIgnore]
    public string DataJson { get; set; }

    public TData GetData<TData>() =>
        JsonSerializer.Deserialize<TData>(DataJson);

    public object GetData(Type returnType) =>
        JsonSerializer.Deserialize(DataJson, returnType);
}