﻿using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core; 

public class IpcRawPacket {
    public OpCode OpCode { get; }
    public byte[] PayloadData { get; }
    
    public int Length => PayloadData.Length;
    public string PayloadJson => Encoding.UTF8.GetString(PayloadData);

    public IpcRawPacket(OpCode opCode, object data) {
        OpCode = opCode;
        PayloadData = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
        
    public IpcRawPacket(OpCode opCode, byte[] payloadData) {
        OpCode = opCode;
        PayloadData = payloadData;
    }
}

public enum OpCode {
     Handshake, Frame, Close, Ping, Pong
}