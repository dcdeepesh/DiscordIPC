using System;
using System.Text.Json;
using System.IO.Pipes;
using System.Threading.Tasks;

using Dec.DiscordIPC.Events;
using System.Threading;

namespace Dec.DiscordIPC.Core {
    public class LowLevelDiscordIPC {
        private NamedPipeClientStream pipe;
        internal MessageReadLoop messageReadLoop;
        protected readonly string clientId;

        public LowLevelDiscordIPC(string clientId, bool verbose) {
            this.clientId = clientId;
            Util.Verbose = verbose;
        }

        public async Task InitAsync() {
            pipe = new NamedPipeClientStream(".", "discord-ipc-0",
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipe.ConnectAsync();

            messageReadLoop = new MessageReadLoop(pipe, this);
            messageReadLoop.Start();

            EventWaitHandle readyWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            EventHandler<Ready.Data> readyListener = (sender, data) => readyWaitHandle.Set();
            OnReady += readyListener;

            await SendMessageAsync(IPCMessage.Handshake(JsonSerializer.SerializeToUtf8Bytes(new {
                client_id = clientId,
                v = "1",
                nonce = Guid.NewGuid().ToString()
            })));

            await Task.Run(() => {
                readyWaitHandle.WaitOne();
                OnReady -= readyListener;
            });
        }

        public async Task<dynamic> SendCommandWeakTypeAsync(dynamic payload) {
            await SendMessageAsync(new IPCMessage(OpCode.FRAME,
                JsonSerializer.SerializeToUtf8Bytes<dynamic>(payload)));
            return await messageReadLoop.WaitForResponse(payload.nonce);
        }

        #region Events

        public event EventHandler<Ready.Data> OnReady;
        // Event ERROR is handled differently
        public event EventHandler<GuildStatus.Data> OnGuildStatus;
        public event EventHandler<GuildCreate.Data> OnGuildCreate;
        public event EventHandler<ChannelCreate.Data> OnChannelCreate;
        public event EventHandler<VoiceChannelSelect.Data> OnVoiceChannelSelect;
        public event EventHandler<VoiceStateCreate.Data> OnVoiceStateCreate;
        public event EventHandler<VoiceStateUpdate.Data> OnVoiceStateUpdate;
        public event EventHandler<VoiceStateDelete.Data> OnVoiceStateDelete;
        public event EventHandler<VoiceSettingsUpdate.Data> OnVoiceSettingsUpdate;
        public event EventHandler<VoiceConnectionStatus.Data> OnVoiceConnectionStatus;
        public event EventHandler<SpeakingStart.Data> OnSpeakingStart;
        public event EventHandler<SpeakingStop.Data> OnSpeakingStop;
        public event EventHandler<MessageCreate.Data> OnMessageCreate;
        public event EventHandler<MessageUpdate.Data> OnMessageUpdate;
        public event EventHandler<MessageDelete.Data> OnMessageDelete;
        public event EventHandler<NotificationCreate.Data> OnNotificationCreate;
        public event EventHandler<ActivityJoin.Data> OnActivityJoin;
        public event EventHandler<ActivitySpectate.Data> OnActivitySpectate;
        public event EventHandler<ActivityJoinRequest.Data> OnActivityJoinRequest;

        // More events on their way

        internal void FireEvent(string evt, IPCMessage message) {
            JsonElement obj = JsonSerializer.Deserialize<dynamic>(message.Json);
            switch (evt) {
                case "READY":
                    OnReady?.Invoke(this, obj.ToObject<Ready.Data>());
                    break;

                case "GUILD_STATUS":
                    OnGuildStatus?.Invoke(this, obj.ToObject<GuildStatus.Data>());
                    break;

                case "GUILD_CREATE":
                    OnGuildCreate?.Invoke(this, obj.ToObject<GuildCreate.Data>());
                    break;

                case "CHANNEL_CREATE":
                    OnChannelCreate?.Invoke(this, obj.ToObject<ChannelCreate.Data>());
                    break;

                case "VOICE_CHANNEL_SELECT":
                    OnVoiceChannelSelect?.Invoke(this, obj.ToObject<VoiceChannelSelect.Data>());
                    break;

                case "VOICE_STATE_CREATE":
                    OnVoiceStateCreate?.Invoke(this, obj.ToObject<VoiceStateCreate.Data>());
                    break;

                case "VOICE_STATE_UPDATE":
                    OnVoiceStateUpdate?.Invoke(this, obj.ToObject<VoiceStateUpdate.Data>());
                    break;

                case "VOICE_STATE_DELETE":
                    OnVoiceStateDelete?.Invoke(this, obj.ToObject<VoiceStateDelete.Data>());
                    break;

                case "VOICE_SETTINGS_UPDATE":
                    OnVoiceSettingsUpdate?.Invoke(this, obj.ToObject<VoiceSettingsUpdate.Data>());
                    break;

                case "VOICE_CONNECTION_STATUS":
                    OnVoiceConnectionStatus?.Invoke(this, obj.ToObject<VoiceConnectionStatus.Data>());
                    break;

                case "SPEAKING_START":
                    OnSpeakingStart?.Invoke(this, obj.ToObject<SpeakingStart.Data>());
                    break;

                case "SPEAKING_STOP":
                    OnSpeakingStop?.Invoke(this, obj.ToObject<SpeakingStop.Data>());
                    break;

                case "MESSAGE_CREATE":
                    OnMessageCreate?.Invoke(this, obj.ToObject<MessageCreate.Data>());
                    break;

                case "MESSAGE_UPDATE":
                    OnMessageUpdate?.Invoke(this, obj.ToObject<MessageUpdate.Data>());
                    break;

                case "MESSAGE_DELETE":
                    OnMessageDelete?.Invoke(this, obj.ToObject<MessageDelete.Data>());
                    break;

                case "NOTIFICATION_CREATE":
                    OnNotificationCreate?.Invoke(this, obj.ToObject<NotificationCreate.Data>());
                    break;

                case "ACTIVITY_JOIN":
                    OnActivityJoin?.Invoke(this, obj.ToObject<ActivityJoin.Data>());
                    break;

                case "ACTIVITY_SPECTATE":
                    OnActivitySpectate?.Invoke(this, obj.ToObject<ActivitySpectate.Data>());
                    break;

                case "ACTIVITY_JOIN_REQUEST":
                    OnActivityJoinRequest?.Invoke(this, obj.ToObject<ActivityJoinRequest.Data>());
                    break;
            }
        }

        #endregion

        public void Dispose() {
            messageReadLoop.Stop();
            pipe.Dispose();
        }

        #region Private methods

        private async Task SendMessageAsync(IPCMessage message) {
            byte[] bOpCode = BitConverter.GetBytes((int) message.opCode);
            byte[] bLen = BitConverter.GetBytes(message.Length);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bOpCode);
                Array.Reverse(bLen);
            }

            byte[] buffer = new byte[4 + 4 + message.Length];
            Array.Copy(bOpCode, buffer, 4);
            Array.Copy(bLen, 0, buffer, 4, 4);
            Array.Copy(message.data, 0, buffer, 8, message.Length);
            Util.Log("\nSENDING:\n{0}", message.Json);
            await pipe.WriteAsync(buffer, 0, buffer.Length);
        }

        #endregion
    }
}
