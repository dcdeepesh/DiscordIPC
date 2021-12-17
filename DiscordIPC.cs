using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC {
    public class DiscordIPC : LowLevelDiscordIPC {
        
        public DiscordIPC(string clientId, bool verbose = false) : base(clientId, verbose) { }
        
        #region Commands
        
        public async Task<Authorize.Data> SendCommandAsync(Authorize.Args args) => await this.SendCommandAsync_Core<Authorize.Data>("AUTHORIZE", args);
        public async Task<Authenticate.Data> SendCommandAsync(Authenticate.Args args) => await this.SendCommandAsync_Core<Authenticate.Data>("AUTHENTICATE", args);
        public async Task<GetGuild.Data> SendCommandAsync(GetGuild.Args args) => await this.SendCommandAsync_Core<GetGuild.Data>("GET_GUILD", args);
        public async Task<GetGuilds.Data> SendCommandAsync(GetGuilds.Args _) => await this.SendCommandAsync_Core<GetGuilds.Data>("GET_GUILDS", null);
        public async Task<GetChannel.Data> SendCommandAsync(GetChannel.Args args) => await this.SendCommandAsync_Core<GetChannel.Data>("GET_CHANNEL", args);
        public async Task<GetChannels.Data> SendCommandAsync(GetChannels.Args args) => await this.SendCommandAsync_Core<GetChannels.Data>("GET_CHANNELS", args);
        public async Task<SetUserVoiceSettings.Data> SendCommandAsync(SetUserVoiceSettings.Args args) => await this.SendCommandAsync_Core<SetUserVoiceSettings.Data>("SET_USER_VOICE_SETTINGS", args);
        public async Task<SelectVoiceChannel.Data> SendCommandAsync(SelectVoiceChannel.Args args) => await this.SendCommandAsync_Core<SelectVoiceChannel.Data>("SELECT_VOICE_CHANNEL", args);
        public async Task<GetSelectedVoiceChannel.Data> SendCommandAsync(GetSelectedVoiceChannel.Args _) => await this.SendCommandAsync_Core<GetSelectedVoiceChannel.Data>("GET_SELECTED_VOICE_CHANNEL", null);
        public async Task<SelectTextChannel.Data> SendCommandAsync(SelectTextChannel.Args args) => await this.SendCommandAsync_Core<SelectTextChannel.Data>("SELECT_TEXT_CHANNEL", args);
        public async Task<GetVoiceSettings.Data> SendCommandAsync(GetVoiceSettings.Args _) => await this.SendCommandAsync_Core<GetVoiceSettings.Data>("GET_VOICE_SETTINGS", null);
        public async Task<SetVoiceSettings.Data> SendCommandAsync(SetVoiceSettings.Args _) => await this.SendCommandAsync_Core<SetVoiceSettings.Data>("SET_VOICE_SETTINGS", null);
        public async Task SendCommandAsync(SetCertifiedDevices.Args args) => await this.SendCommandAsync_Core<object>("SET_CERTIFIED_DEVICES", args, false);
        public async Task SendCommandAsync(SetActivity.Args args) => await this.SendCommandAsync_Core<object>("SET_ACTIVITY", args, false);
        public async Task SendCommandAsync(SendActivityJoinInvite.Args args) => await this.SendCommandAsync_Core<object>("SET_ACTIVITY_JOIN_INVITE", args, false);
        public async Task SendCommandAsync(CloseActivityRequest.Args args) => await this.SendCommandAsync_Core<object>("CLOSE_ACTIVITY_REQUEST", args, false);
        
        private async Task<T> SendCommandAsync_Core<T>(string cmd, ICommandArgs args, bool convert = true) {
            string nonce = Guid.NewGuid().ToString();
            CommandPayload payload;
            if (args is null)
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
            
            JsonElement response = await this.SendCommandWeakTypeAsync(payload);
            return convert ? response.GetProperty("data").ToObject<T>() : default;
        }
        
        #endregion
        
        #region Event subscription
        
        public async Task SubscribeAllAsync(IEnumerable<ICommandArgs> list) {
            foreach (ICommandArgs args in list)
                await this.SubscribeAsync(args);
        }
        public Task SubscribeAsync(ICommandArgs obj) {
            return obj switch {
                GuildStatus.Args args => this.SubscribeAsync(args),
                GuildCreate.Args args => this.SubscribeAsync(args),
                ChannelCreate.Args args => this.SubscribeAsync(args),
                VoiceChannelSelect.Args args => this.SubscribeAsync(args),
                VoiceStateCreate.Args args => this.SubscribeAsync(args),
                VoiceStateUpdate.Args args => this.SubscribeAsync(args),
                VoiceStateDelete.Args args => this.SubscribeAsync(args),
                VoiceSettingsUpdate.Args args => this.SubscribeAsync(args),
                VoiceConnectionStatus.Args args => this.SubscribeAsync(args),
                SpeakingStart.Args args => this.SubscribeAsync(args),
                SpeakingStop.Args args => this.SubscribeAsync(args),
                MessageCreate.Args args => this.SubscribeAsync(args),
                MessageUpdate.Args args => this.SubscribeAsync(args),
                MessageDelete.Args args => this.SubscribeAsync(args),
                NotificationCreate.Args args => this.SubscribeAsync(args),
                ActivityJoin.Args args => this.SubscribeAsync(args),
                ActivitySpectate.Args args => this.SubscribeAsync(args),
                ActivityJoinRequest.Args args => this.SubscribeAsync(args),
                _ => Task.CompletedTask
            };
        }
        public async Task SubscribeAsync(GuildStatus.Args args) => await this.SubscribeAsync_Core("GUILD_STATUS", args);
        public async Task SubscribeAsync(GuildCreate.Args _) => await this.SubscribeAsync_Core("GUILD_CREATE", null);
        public async Task SubscribeAsync(ChannelCreate.Args _) => await this.SubscribeAsync_Core("CHANNEL_CREATE", null);
        public async Task SubscribeAsync(VoiceChannelSelect.Args _) => await this.SubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);
        public async Task SubscribeAsync(VoiceStateCreate.Args args) => await this.SubscribeAsync_Core("VOICE_STATE_CREATE", args);
        public async Task SubscribeAsync(VoiceStateUpdate.Args args) => await this.SubscribeAsync_Core("VOICE_STATE_UPDATE", args);
        public async Task SubscribeAsync(VoiceStateDelete.Args args) => await this.SubscribeAsync_Core("VOICE_STATE_DELETE", args);
        public async Task SubscribeAsync(VoiceSettingsUpdate.Args _) => await this.SubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);
        public async Task SubscribeAsync(VoiceConnectionStatus.Args _) => await this.SubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);
        public async Task SubscribeAsync(SpeakingStart.Args args) => await this.SubscribeAsync_Core("SPEAKING_START", args);
        public async Task SubscribeAsync(SpeakingStop.Args args) => await this.SubscribeAsync_Core("SPEAKING_STOP", args);
        public async Task SubscribeAsync(MessageCreate.Args args) => await this.SubscribeAsync_Core("MESSAGE_CREATE", args);
        public async Task SubscribeAsync(MessageUpdate.Args args) => await this.SubscribeAsync_Core("MESSAGE_UPDATE", args);
        public async Task SubscribeAsync(MessageDelete.Args args) => await this.SubscribeAsync_Core("MESSAGE_DELETE", args);
        public async Task SubscribeAsync(NotificationCreate.Args _) => await this.SubscribeAsync_Core("NOTIFICATION_CREATE", null);
        public async Task SubscribeAsync(ActivityJoin.Args _) => await this.SubscribeAsync_Core("ACTIVITY_JOIN", null);
        public async Task SubscribeAsync(ActivitySpectate.Args _) => await this.SubscribeAsync_Core("ACTIVITY_SPECTATE", null);
        public async Task SubscribeAsync(ActivityJoinRequest.Args _) => await this.SubscribeAsync_Core("ACTIVITY_JOIN_REQUEST", null);
        
        private async Task SubscribeAsync_Core(string evnt, ICommandArgs args) {
            string nonce = Guid.NewGuid().ToString();
            EventPayload payload;
            if (args is null)
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
            
            await this.SendCommandWeakTypeAsync(payload);
        }
        
        #endregion
        
        #region Event un-subscription
        
        public async Task UnsubscribeAllAsync(IEnumerable<ICommandArgs> list) {
            foreach (ICommandArgs args in list)
                await this.UnsubscribeAsync(args);
        }
        public Task UnsubscribeAsync(ICommandArgs obj) {
            return obj switch {
                GuildStatus.Args args => this.UnsubscribeAsync(args),
                GuildCreate.Args args => this.UnsubscribeAsync(args),
                ChannelCreate.Args args => this.UnsubscribeAsync(args),
                VoiceChannelSelect.Args args => this.UnsubscribeAsync(args),
                VoiceStateCreate.Args args => this.UnsubscribeAsync(args),
                VoiceStateUpdate.Args args => this.UnsubscribeAsync(args),
                VoiceStateDelete.Args args => this.UnsubscribeAsync(args),
                VoiceSettingsUpdate.Args args => this.UnsubscribeAsync(args),
                VoiceConnectionStatus.Args args => this.UnsubscribeAsync(args),
                SpeakingStart.Args args => this.UnsubscribeAsync(args),
                SpeakingStop.Args args => this.UnsubscribeAsync(args),
                MessageCreate.Args args => this.UnsubscribeAsync(args),
                MessageUpdate.Args args => this.UnsubscribeAsync(args),
                MessageDelete.Args args => this.UnsubscribeAsync(args),
                NotificationCreate.Args args => this.UnsubscribeAsync(args),
                ActivityJoin.Args args => this.UnsubscribeAsync(args),
                ActivitySpectate.Args args => this.UnsubscribeAsync(args),
                ActivityJoinRequest.Args args => this.UnsubscribeAsync(args),
                _ => Task.CompletedTask
            };
        }
        public async Task UnsubscribeAsync(GuildStatus.Args args) => await this.UnsubscribeAsync_Core("GUILD_STATUS", args);
        public async Task UnsubscribeAsync(GuildCreate.Args _) => await this.UnsubscribeAsync_Core("GUILD_CREATE", null);
        public async Task UnsubscribeAsync(ChannelCreate.Args _) => await this.UnsubscribeAsync_Core("CHANNEL_CREATE", null);
        public async Task UnsubscribeAsync(VoiceChannelSelect.Args _) => await this.UnsubscribeAsync_Core("VOICE_CHANNEL_SELECT", null);
        public async Task UnsubscribeAsync(VoiceStateCreate.Args args) => await this.UnsubscribeAsync_Core("VOICE_STATE_CREATE", args);
        public async Task UnsubscribeAsync(VoiceStateUpdate.Args args) => await this.UnsubscribeAsync_Core("VOICE_STATE_UPDATE", args);
        public async Task UnsubscribeAsync(VoiceStateDelete.Args args) => await this.UnsubscribeAsync_Core("VOICE_STATE_DELETE", args);
        public async Task UnsubscribeAsync(VoiceSettingsUpdate.Args _) => await this.UnsubscribeAsync_Core("VOICE_SETTINGS_UPDATE", null);
        public async Task UnsubscribeAsync(VoiceConnectionStatus.Args _) => await this.UnsubscribeAsync_Core("VOICE_CONNECTION_STATUS", null);
        public async Task UnsubscribeAsync(SpeakingStart.Args args) => await this.UnsubscribeAsync_Core("SPEAKING_START", args);
        public async Task UnsubscribeAsync(SpeakingStop.Args args) => await this.UnsubscribeAsync_Core("SPEAKING_STOP", args);
        public async Task UnsubscribeAsync(MessageCreate.Args args) => await this.UnsubscribeAsync_Core("MESSAGE_CREATE", args);
        public async Task UnsubscribeAsync(MessageUpdate.Args args) => await this.UnsubscribeAsync_Core("MESSAGE_UPDATE", args);
        public async Task UnsubscribeAsync(MessageDelete.Args args) => await this.UnsubscribeAsync_Core("MESSAGE_DELETE", args);
        public async Task UnsubscribeAsync(NotificationCreate.Args _) => await this.UnsubscribeAsync_Core("NOTIFICATION_CREATE", null);
        public async Task UnsubscribeAsync(ActivityJoin.Args _) => await this.UnsubscribeAsync_Core("ACTIVITY_JOIN", null);
        public async Task UnsubscribeAsync(ActivitySpectate.Args _) => await this.UnsubscribeAsync_Core("ACTIVITY_SPECTATE", null);
        public async Task UnsubscribeAsync(ActivityJoinRequest.Args _) => await this.UnsubscribeAsync_Core("ACTIVITY_JOIN_REQUEST", null);
        
        private async Task UnsubscribeAsync_Core(string evnt, ICommandArgs args) {
            string nonce = Guid.NewGuid().ToString();
            EventPayload payload;
            if (args is null)
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
            
            await this.SendCommandWeakTypeAsync(payload);
        }
        
        #endregion
    }
}
