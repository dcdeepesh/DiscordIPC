using System;
using System.Text.Json;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC {
    /// <summary>
    /// The main access point for user programs to use DiscordIPC.
    /// </summary>
    public class DiscordIPC : LowLevelDiscordIPC {
        /// <summary>
        /// Creates a client instance.
        /// </summary>
        /// <remarks>
        /// Does not actually initialize the client.
        /// Use <see cref="LowLevelDiscordIPC.InitAsync"/> after this to initialize the client.
        /// </remarks>
        /// <param name="clientId">Client ID of your app.</param>
        /// <param name="verbose">If true, DiscordIPC logs every JSON
        /// sent and received to the console.</param>
        public DiscordIPC(string clientId, bool verbose = false) : base(clientId, verbose) { }

        #region Commands

        public async Task<TData> SendCommandAsync<TArgs, TData>(ICommand<TArgs, TData> command) =>
            (TData) await SendCommandAsyncImpl(command, typeof(TData));

        public async Task SendCommandAsync<TArgs>(ICommand<TArgs> command) =>
            await SendCommandAsyncImpl(command);

        private async Task<object> SendCommandAsyncImpl<TArgs>(ICommand<TArgs> command,
            Type returnType = null) {
            
            IpcPayload payload = new() {
                cmd = command.Name,
                nonce = Guid.NewGuid().ToString(),
                args = command.Arguments
            };
            
            JsonElement response = await SendPayloadAsync(payload);
            return returnType is null ? null : response.GetProperty("data").ToObject(returnType);
        }

        #endregion

        #region Event subscription

        public async Task SubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
            IpcPayload payload = new() {
                cmd = "SUBSCRIBE",
                nonce = Guid.NewGuid().ToString(),
                evt = theEvent.Name,
                args = theEvent.Arguments
            };

            await SendPayloadAsync(payload);
        }

        #endregion

        #region Event un-subscription

        public async Task UnsubscribeAsync<TArgs>(IEvent<TArgs> theEvent) {
            IpcPayload payload = new() {
                cmd = "UNSUBSCRIBE",
                nonce = Guid.NewGuid().ToString(),
                evt = theEvent.Name,
                args = theEvent.Arguments
            };

            await SendPayloadAsync(payload);
        }

        #endregion
    }
}
