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

        public async Task<TData> SendCommandAsync<TArgs, TData>(ICommand<TArgs, TData> command) {
            var nonce = Guid.NewGuid().ToString();
            IpcPayload payload;
            var args = command.Arguments;
            string cmd = command.Name;
            if (args is null)
                payload = new IpcPayload() { cmd=cmd, nonce=nonce };
            else
                payload = new IpcPayload() { cmd=cmd, nonce=nonce, args=args };
        
            JsonElement response = await SendPayloadAsync(payload);
            return response.GetProperty("data").ToObject<TData>();
        }
        
        public async Task SendCommandAsync<TArgs>(ICommand<TArgs> command) {
            var nonce = Guid.NewGuid().ToString();
            IpcPayload payload;
            var args = command.Arguments;
            string cmd = command.Name;
            if (args is null)
                payload = new IpcPayload { cmd=cmd, nonce=nonce };
            else
                payload = new IpcPayload { cmd=cmd, nonce=nonce, args=args };
            
            await SendPayloadAsync(payload);
        }

        #endregion

        #region Event subscription

        public async Task SubscribeAsync(GuildStatus.Args args) =>
            await SubscribeAsync_Core("GUILD_STATUS", args);

        public async Task SubscribeAsync(GuildCreate.Args _) =>
            await SubscribeAsync_Core("GUILD_CREATE", null);

        public async Task SubscribeAsync(ChannelCreate.Args _) =>
            await SubscribeAsync_Core("CHANNEL_CREATE", null);

        public async Task SubscribeAsync(VoiceChannelSelect.Args _) =>
            await SubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);

        public async Task SubscribeAsync(VoiceStateCreate.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_CREATE", args);

        public async Task SubscribeAsync(VoiceStateUpdate.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_UPDATE", args);

        public async Task SubscribeAsync(VoiceStateDelete.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_DELETE", args);

        public async Task SubscribeAsync(VoiceSettingsUpdate.Args _) =>
            await SubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);

        public async Task SubscribeAsync(VoiceConnectionStatus.Args _) =>
            await SubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);

        public async Task SubscribeAsync(SpeakingStart.Args args) =>
            await SubscribeAsync_Core("SPEAKING_START", args);

        public async Task SubscribeAsync(SpeakingStop.Args args) =>
            await SubscribeAsync_Core("SPEAKING_STOP", args);

        public async Task SubscribeAsync(MessageCreate.Args args) =>
            await SubscribeAsync_Core("MESSAGE_CREATE", args);

        public async Task SubscribeAsync(MessageUpdate.Args args) =>
            await SubscribeAsync_Core("MESSAGE_UPDATE", args);

        public async Task SubscribeAsync(MessageDelete.Args args) =>
            await SubscribeAsync_Core("MESSAGE_DELETE", args);

        public async Task SubscribeAsync(NotificationCreate.Args _) =>
            await SubscribeAsync_Core("NOTIFICATION_CREATE", null);

        public async Task SubscribeAsync(ActivityJoin.Args _) =>
            await SubscribeAsync_Core("ACTIVITY_JOIN", null);

        public async Task SubscribeAsync(ActivitySpectate.Args _) =>
            await SubscribeAsync_Core("ACTIVITY_SPECTATE", null);

        public async Task SubscribeAsync(ActivityJoinRequest.Args _) =>
            await SubscribeAsync_Core("ACTIVITY_JOIN_REQUEST", null);

        private async Task SubscribeAsync_Core(string evt, dynamic args) {
            var nonce = Guid.NewGuid().ToString();
            IpcPayload payload;
            if (args is null)
                payload = new IpcPayload { cmd = "SUBSCRIBE", nonce=nonce, evt=evt };
            else
                payload = new IpcPayload { cmd = "SUBSCRIBE", nonce=nonce, evt=evt, args=args };

            await SendPayloadAsync(payload);
        }

        #endregion

        #region Event un-subscription

        public async Task UnsubscribeAsync(GuildStatus.Args args) =>
            await UnsubscribeAsync_Core("GUILD_STATUS", args);

        public async Task UnsubscribeAsync(GuildCreate.Args _) =>
            await UnsubscribeAsync_Core("GUILD_CREATE", null);

        public async Task UnsubscribeAsync(ChannelCreate.Args _) =>
            await UnsubscribeAsync_Core("CHANNEL_CREATE", null);

        public async Task UnsubscribeAsync(VoiceChannelSelect.Args _) =>
            await UnsubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);

        public async Task UnsubscribeAsync(VoiceStateCreate.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_CREATE", args);

        public async Task UnsubscribeAsync(VoiceStateUpdate.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_UPDATE", args);

        public async Task UnsubscribeAsync(VoiceStateDelete.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_DELETE", args);

        public async Task UnsubscribeAsync(VoiceSettingsUpdate.Args _) =>
            await UnsubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);

        public async Task UnsubscribeAsync(VoiceConnectionStatus.Args _) =>
            await UnsubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);

        public async Task UnsubscribeAsync(SpeakingStart.Args args) =>
            await UnsubscribeAsync_Core("SPEAKING_START", args);

        public async Task UnsubscribeAsync(SpeakingStop.Args args) =>
            await UnsubscribeAsync_Core("SPEAKING_STOP", args);

        public async Task UnsubscribeAsync(MessageCreate.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_CREATE", args);

        public async Task UnsubscribeAsync(MessageUpdate.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_UPDATE", args);

        public async Task UnsubscribeAsync(MessageDelete.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_DELETE", args);

        public async Task UnsubscribeAsync(NotificationCreate.Args _) =>
            await UnsubscribeAsync_Core("NOTIFICATION_CREATE", null);

        public async Task UnsubscribeAsync(ActivityJoin.Args _) =>
            await UnsubscribeAsync_Core("ACTIVITY_JOIN", null);

        public async Task UnsubscribeAsync(ActivitySpectate.Args _) =>
            await UnsubscribeAsync_Core("ACTIVITY_SPECTATE", null);

        public async Task UnsubscribeAsync(ActivityJoinRequest.Args _) =>
            await UnsubscribeAsync_Core("ACTIVITY_JOIN_REQUEST", null);

        private async Task UnsubscribeAsync_Core(string evt, dynamic args) {
            var nonce = Guid.NewGuid().ToString();
            IpcPayload payload;
            if (args is null)
                payload = new IpcPayload { cmd = "UNSUBSCRIBE", nonce=nonce, evt=evt, };
            else
                payload = new IpcPayload { cmd = "UNSUBSCRIBE", nonce=nonce, evt=evt, args=args };

            await SendPayloadAsync(payload);
        }

        #endregion
    }
}
