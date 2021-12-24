using System;

namespace Dec.DiscordIPC.Development {
    public class DiscordRPCAttribute : Attribute {
        public readonly string Command;
        public readonly bool Authenticated;
        
        public DiscordRPCAttribute(string command, bool authenticated = true) {
            this.Command = command;
            this.Authenticated = authenticated;
        }
    }
}