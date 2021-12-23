﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Development;

namespace Dec.DiscordIPC {
    public class DiscordIPC : LowLevelDiscordIPC {
        
        public DiscordIPC(
            string clientId,
            IPCHello<DiscordIPC> beforeAuthorize,
            IPCHello<DiscordIPC> afterAuthorize,
            bool verbose = false
        ) : base(clientId, (ipc, can) => beforeAuthorize(ipc as DiscordIPC, can), (ipc, can) => afterAuthorize(ipc as DiscordIPC, can), verbose) {}
        public DiscordIPC(string clientId, bool verbose = false) : this(clientId, LowLevelDiscordIPC.EmptyHello, LowLevelDiscordIPC.EmptyHello, verbose) { }
        
        #region Commands
        
        /// <summary>Send a command payload that does not have a return type</summary>
        public async Task SendCommandAsync(IPayloadResponse args, CancellationToken cancellationToken = default) {
            Type type = args.GetType();
            DiscordRPCAttribute attribute = type.GetCustomAttribute<DiscordRPCAttribute>() ?? throw new ArgumentException("Payloads must have the DiscordRPC Attribute", nameof(args));
            await this.SendCommandAsync(attribute.Command, args, cancellationToken);
        }
        
        /// <summary>Emit a command that has no return type</summary>
        private Task SendCommandAsync(string cmd, ICommandArgs args, CancellationToken cancellationToken = default) => this.SendCommandAsync<object>(cmd, args, cancellationToken);
        
        /// <summary>Send a command payload that has a return type</summary>
        public async Task<T> SendCommandAsync<T>(IPayloadResponse<T> args, CancellationToken cancellationToken = default) {
            Type type = args.GetType();
            DiscordRPCAttribute attribute = type.GetCustomAttribute<DiscordRPCAttribute>() ?? throw new ArgumentException("Payloads must have the DiscordRPC Attribute", nameof(args));
            return await this.SendCommandAsync<T>(attribute.Command, args, cancellationToken);
        }
        
        /// <summary>Emit a command that has a return type</summary>
        private async Task<T> SendCommandAsync<T>(string cmd, ICommandArgs args, CancellationToken cancellationToken = default) {
            string nonce = Guid.NewGuid().ToString();
            CommandPayload payload;
            if (args is null || args is IDummyCommandArgs)
                payload = new CommandPayload {
                    Command = cmd,
                    Nonce = nonce
                };
            else
                payload = new CommandPayloadArgs {
                    Command = cmd,
                    Nonce = nonce,
                    Args = args
                };
            
            JsonElement response = await this.SendCommandAsync(payload, LowLevelDiscordIPC.IsAuthorizedPayload(args), cancellationToken);
            return typeof(T) == typeof(object) ? default : response.GetProperty("data").ToObject<T>();
        }
        
        #endregion
        
        #region Event subscription
        
        public async Task SubscribeAllAsync(IEnumerable<ICommandArgs> list, CancellationToken cancellationToken = default) {
            foreach (ICommandArgs args in list)
                await this.SubscribeAsync(args, cancellationToken);
        }
        public Task SubscribeAsync(ICommandArgs args, CancellationToken cancellationToken = default) {
            Type type = args.GetType();
            DiscordRPCAttribute attribute = type.GetCustomAttribute<DiscordRPCAttribute>() ?? throw new ArgumentException("Payloads must have the DiscordRPC Attribute", nameof(args));
            return this.SubscribeAsync(attribute.Command, args, cancellationToken);
        }
        private async Task SubscribeAsync(string evnt, ICommandArgs args, CancellationToken cancellationToken = default) {
            string nonce = Guid.NewGuid().ToString();
            EventPayload payload;
            if (args is null || args is IDummyCommandArgs)
                payload = new EventPayload {
                    Command = "SUBSCRIBE",
                    Nonce = nonce,
                    Event = evnt
                };
            else
                payload = new EventPayloadArgs {
                    Command = "SUBSCRIBE",
                    Nonce = nonce,
                    Event = evnt,
                    Args = args
                };
            
            await this.SendCommandAsync(payload, cancellationToken: cancellationToken);
        }
        
        #endregion
        
        #region Event un-subscription
        
        public async Task UnsubscribeAllAsync(IEnumerable<ICommandArgs> list, CancellationToken cancellationToken = default) {
            foreach (ICommandArgs args in list)
                await this.UnsubscribeAsync(args, cancellationToken);
        }
        public Task UnsubscribeAsync(ICommandArgs args, CancellationToken cancellationToken = default) {
            Type type = args.GetType();
            DiscordRPCAttribute attribute = type.GetCustomAttribute<DiscordRPCAttribute>() ?? throw new ArgumentException("Payloads must have the DiscordRPC Attribute", nameof(args));
            return this.UnsubscribeAsync(attribute.Command, args, cancellationToken);
        }
        private async Task UnsubscribeAsync(string evnt, ICommandArgs args, CancellationToken cancellationToken = default) {
            string nonce = Guid.NewGuid().ToString();
            EventPayload payload;
            if (args is null || args is IDummyCommandArgs)
                payload = new EventPayload {
                    Command = "UNSUBSCRIBE",
                    Nonce = nonce,
                    Event = evnt
                };
            else
                payload = new EventPayloadArgs {
                    Command = "UNSUBSCRIBE",
                    Nonce = nonce,
                    Event = evnt,
                    Args = args
                };
            
            await this.SendCommandAsync(payload, cancellationToken: cancellationToken);
        }
        
        #endregion
        
    }
}
