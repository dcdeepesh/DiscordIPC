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

        public async Task SubscribeAsync(GuildStatusEvent.Args args) =>
            await SubscribeAsync_Core("GUILD_STATUS", args);

        public async Task SubscribeAsync(GuildCreateEvent.Args _) =>
            await SubscribeAsync_Core("GUILD_CREATE", null);

        public async Task SubscribeAsync(ChannelCreateEvent.Args _) =>
            await SubscribeAsync_Core("CHANNEL_CREATE", null);

        public async Task SubscribeAsync(VoiceChannelSelectEvent.Args _) =>
            await SubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);

        public async Task SubscribeAsync(VoiceStateCreateEvent.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_CREATE", args);

        public async Task SubscribeAsync(VoiceStateUpdateEvent.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_UPDATE", args);

        public async Task SubscribeAsync(VoiceStateDeleteEvent.Args args) =>
            await SubscribeAsync_Core("VOICE_STATE_DELETE", args);

        public async Task SubscribeAsync(VoiceSettingsUpdateEvent.Args _) =>
            await SubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);

        public async Task SubscribeAsync(VoiceConnectionStatusEvent.Args _) =>
            await SubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);

        public async Task SubscribeAsync(SpeakingStartEvent.Args args) =>
            await SubscribeAsync_Core("SPEAKING_START", args);

        public async Task SubscribeAsync(SpeakingStopEvent.Args args) =>
            await SubscribeAsync_Core("SPEAKING_STOP", args);

        public async Task SubscribeAsync(MessageCreateEvent.Args args) =>
            await SubscribeAsync_Core("MESSAGE_CREATE", args);

        public async Task SubscribeAsync(MessageUpdateEvent.Args args) =>
            await SubscribeAsync_Core("MESSAGE_UPDATE", args);

        public async Task SubscribeAsync(MessageDeleteEvent.Args args) =>
            await SubscribeAsync_Core("MESSAGE_DELETE", args);

        public async Task SubscribeAsync(NotificationCreateEvent.Args _) =>
            await SubscribeAsync_Core("NOTIFICATION_CREATE", null);

        public async Task SubscribeAsync(ActivityJoinEvent.Args _) =>
            await SubscribeAsync_Core("ACTIVITY_JOIN", null);

        public async Task SubscribeAsync(ActivitySpectateEvent.Args _) =>
            await SubscribeAsync_Core("ACTIVITY_SPECTATE", null);

        public async Task SubscribeAsync(ActivityJoinRequestEvent.Args _) =>
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

        public async Task UnsubscribeAsync(GuildStatusEvent.Args args) =>
            await UnsubscribeAsync_Core("GUILD_STATUS", args);

        public async Task UnsubscribeAsync(GuildCreateEvent.Args _) =>
            await UnsubscribeAsync_Core("GUILD_CREATE", null);

        public async Task UnsubscribeAsync(ChannelCreateEvent.Args _) =>
            await UnsubscribeAsync_Core("CHANNEL_CREATE", null);

        public async Task UnsubscribeAsync(VoiceChannelSelectEvent.Args _) =>
            await UnsubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);

        public async Task UnsubscribeAsync(VoiceStateCreateEvent.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_CREATE", args);

        public async Task UnsubscribeAsync(VoiceStateUpdateEvent.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_UPDATE", args);

        public async Task UnsubscribeAsync(VoiceStateDeleteEvent.Args args) =>
            await UnsubscribeAsync_Core("VOICE_STATE_DELETE", args);

        public async Task UnsubscribeAsync(VoiceSettingsUpdateEvent.Args _) =>
            await UnsubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);

        public async Task UnsubscribeAsync(VoiceConnectionStatusEvent.Args _) =>
            await UnsubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);

        public async Task UnsubscribeAsync(SpeakingStartEvent.Args args) =>
            await UnsubscribeAsync_Core("SPEAKING_START", args);

        public async Task UnsubscribeAsync(SpeakingStopEvent.Args args) =>
            await UnsubscribeAsync_Core("SPEAKING_STOP", args);

        public async Task UnsubscribeAsync(MessageCreateEvent.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_CREATE", args);

        public async Task UnsubscribeAsync(MessageUpdateEvent.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_UPDATE", args);

        public async Task UnsubscribeAsync(MessageDeleteEvent.Args args) =>
            await UnsubscribeAsync_Core("MESSAGE_DELETE", args);

        public async Task UnsubscribeAsync(NotificationCreateEvent.Args _) =>
            await UnsubscribeAsync_Core("NOTIFICATION_CREATE", null);

        public async Task UnsubscribeAsync(ActivityJoinEvent.Args _) =>
            await UnsubscribeAsync_Core("ACTIVITY_JOIN", null);

        public async Task UnsubscribeAsync(ActivitySpectateEvent.Args _) =>
            await UnsubscribeAsync_Core("ACTIVITY_SPECTATE", null);

        public async Task UnsubscribeAsync(ActivityJoinRequestEvent.Args _) =>
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
