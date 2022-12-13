using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dec.DiscordIPC.Commands; 

public class AuthorizeCommand : ICommand<AuthorizeCommand.Args, AuthorizeCommand.Data> {
        
    public string Name => "AUTHORIZE";
    public Args Arguments { get; set; }
        
    public static AuthorizeCommand Create(Action<Args> argsBuilder) {
        AuthorizeCommand command = new() {
            Arguments = new Args()
        };
        argsBuilder(command.Arguments);
        return command;
    }
        
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Args {
        public List<string> scopes { get; set; }
        public string client_id { get; set; }
        public string rpc_token { get; set; }
        public string username { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Data {
        public string code { get; set; }
    }
}