using System;
using System.Collections.Generic;

using Dec.DiscordIPC.Entities;

namespace Dec.DiscordIPC.Commands {
    public class AuthenticateCommand : ICommand<AuthenticateCommand.Args, AuthenticateCommand.Data> {
        
        public static Args Create(Action<Args> argsBuilder) {
            Args args = new();
            argsBuilder(args);
            return args;
        }
        
        public class Args {
            public string access_token { get; set; }
        }

        public class Data {
            public User user { get; set; }
            public List<string> scopes { get; set; }
            public string expires { get; set; }
            public OAuth2AppStructure application { get; set; }

            public class OAuth2AppStructure {
                public string description { get; set; }
                public string icon { get; set; }
                public string id { get; set; }
                public List<string> rpc_origins { get; set; }
                public string name { get; set; }
            }
        }

        public class OAuth2Scopes {
            public static readonly string
                ACTIVITIES_READ = "activities.read",
                ACTIVITIES_WRITE = "activities.write",
                APPLICATIONS_BUILDS_READ = "applications.builds.read",
                APPLICATIONS_BUILDS_UPLOAD = "applications.builds.upload",
                APPLICATIONS_COMMANDS = "applications.commands",
                APPLICATIONS_COMMANDS_UPDATE = "applications.commands.update",
                APPLICATIONS_ENTITLEMENTS = "applications.entitlements",
                APPLICATIONS_STORE_UPDATE = "applications.store.update",
                BOT = "bot",
                CONNECTIONS = "connections",
                EMAIL = "email",
                GDM_JOIN = "gdm.join",
                GUILDS = "guilds",
                GUILDS_JOIN = "guilds.join",
                GUILDS_MEMBERS_READ = "guilds.members.read",
                IDENTIFY = "identify",
                MESSAGES_READ = "messages.read",
                RELATIONSHIPS_READ = "relationships.read",
                RPC = "rpc",
                RPC_ACTIVITIES_WRITE = "rpc.activities.write",
                RPC_NOTIFICATIONS_READ = "rpc.notifications.read",
                RPC_VOICE_READ = "rpc.voice.read",
                RPC_VOICE_WRITE = "rpc.voice.write",
                WEBHOOK_INCOMING = "webhook.incoming";
        }
    }
}
