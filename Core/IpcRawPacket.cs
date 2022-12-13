﻿using System.Text;

namespace Dec.DiscordIPC.Core; 

public class IpcRawPacket {
    public OpCode OpCode { get; }
    public byte[] Data { get; }
    
    public int Length => Data.Length;
    public string Json => Encoding.UTF8.GetString(Data);

    public IpcRawPacket(OpCode opCode, object data)
        : this(opCode, Dec.DiscordIPC.Core.Json.SerializeToBytes(data)) {
    }
        
    public IpcRawPacket(OpCode opCode, byte[] data) {
        OpCode = opCode;
        Data = data;
    }
}

public enum OpCode {
     Handshake, Frame, Close, Ping, Pong
}