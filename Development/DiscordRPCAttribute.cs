using System;

namespace Dec.DiscordIPC.Development {
    public class DiscordRPCAttribute : Attribute {
        public readonly string Command;
        
        public DiscordRPCAttribute(string command) {
            this.Command = command;
        }
    }
}