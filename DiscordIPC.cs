using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dec.DiscordIPC {
    public class DiscordIPC : LowLevelDiscordIPC {

        public DiscordIPC(string clientId) : base(clientId) { }

        #region Commands

        public async Task<Authorize.Data> SendCommandAsync(Authorize.Args args) =>
            await SendCommandAsync_Core("AUTHORIZE", args) as Authorize.Data;

        public async Task<Authenticate.Data> SendCommandAsync(Authenticate.Args args) =>
            await SendCommandAsync_Core("AUTHENTICATE", args) as Authenticate.Data;

        private async Task<dynamic> SendCommandAsync_Core(string cmd, dynamic args) {
            var nonce = Guid.NewGuid().ToString();
            dynamic payload = new {
                cmd,
                nonce,
                args
            };

            JsonElement response = await SendCommandWeakTypeAsync(payload);
            return response.GetProperty("data").ToObject<Authorize.Data>();
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
            dynamic payload;
            if (args is null)
                payload = new { cmd = "SUBSCRIBE", nonce, evt };
            else
                payload = new { cmd = "SUBSCRIBE", nonce, evt, args };

            await SendCommandWeakTypeAsync(payload);
            await messageReadLoop.WaitForResponse(nonce);
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
            dynamic payload;
            if (args is null)
                payload = new { cmd = "UNSUBSCRIBE", nonce, evt, };
            else
                payload = new { cmd = "UNSUBSCRIBE", nonce, evt, args };

            await SendCommandWeakTypeAsync(payload);
            await messageReadLoop.WaitForResponse(nonce);
        }

        #endregion
    }
}
