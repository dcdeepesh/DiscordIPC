using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Commands.Interfaces;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core {
    public class LowLevelDiscordIPC : IDisposable {
        private readonly LeakyPipeConnection Pipe;
        private readonly IPCHello<LowLevelDiscordIPC> UserHello;
        protected readonly string ClientId;
        private string NamedPipe {
            get {
                const string NAME = "discord-ipc-0";
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? $"{Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")}/{NAME}" : NAME;
            }
        }
        
        public LowLevelDiscordIPC(
            string clientId,
            IPCHello<LowLevelDiscordIPC> beforeAuthorize,
            IPCHello<LowLevelDiscordIPC> afterAuthorize,
            bool verbose = false
        ) {
            this.ClientId = clientId;
            this.UserHello = beforeAuthorize;
            Util.Verbose = verbose;
            this.Pipe = new LeakyPipeConnection(this.NamedPipe, this.HelloEvent, () => afterAuthorize(this), this.FireEvent);
        }
        public LowLevelDiscordIPC(string clientId, bool verbose = false): this(clientId, LowLevelDiscordIPC.EmptyHello, LowLevelDiscordIPC.EmptyHello, verbose) {}
        
        /// <summary>
        /// Start the connection loop
        /// </summary>
        public void Init() => this.Pipe.Start();
        
        public async Task<JsonElement> SendCommandAsync(CommandPayload payload, bool authorized = true, CancellationToken cancellationToken = default) {
            await this.SendMessageAsync(new IPCMessage(OpCode.FRAME, Json.SerializeToBytes<dynamic>(payload)), authorized, cancellationToken);
            return await this.Pipe.WaitForResponse(payload.Nonce, cancellationToken);
        }
        
        /// <summary>
        /// Wait for the Stream to Connect
        /// </summary>
        public Task AwaitConnectedAsync(CancellationToken cancellationToken = default) => this.Pipe.AwaitConnectedAsync(cancellationToken);
        
        /// <summary>
        /// Wait for the HELLO Event (After connecting) to have been sent
        /// </summary>
        public Task AwaitHelloAsync(CancellationToken cancellationToken = default) => this.Pipe.AwaitHelloAsync(cancellationToken);
        
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        private async Task HelloEvent(NamedPipeClientStream stream, CancellationToken cancellationToken) {
            EventWaitHandle readyWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            void ReadyListener(object sender, Ready.Data data) => readyWaitHandle.Set();
            this.OnReady += ReadyListener;
            
            await this.SendMessageAsync(stream, IPCMessage.Handshake(Json.SerializeToBytes(new {
                client_id = this.ClientId,
                v = "1",
                nonce = Guid.NewGuid()
                    .ToString()
            })), cancellationToken);
            
            await Task.Run(() => {
                readyWaitHandle.WaitOne();
                this.OnReady -= ReadyListener;
            }, cancellationToken);
            
            await this.UserHello(this, cancellationToken);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="message"></param>
        private void FireEvent(string evt, IPCMessage message) {
            JsonElement obj = Json.Deserialize<dynamic>(message.RawData).GetProperty("data");
            object _ = evt switch {
                "READY" => this.InvokeEvent(this.OnReady, obj),
                "GUILD_STATUS" => this.InvokeEvent(this.OnGuildStatus, obj),
                "GUILD_CREATE" => this.InvokeEvent(this.OnGuildCreate, obj),
                "CHANNEL_CREATE" => this.InvokeEvent(this.OnChannelCreate, obj),
                "VOICE_CHANNEL_SELECT" => this.InvokeEvent(this.OnVoiceChannelSelect, obj),
                "VOICE_STATE_CREATE" => this.InvokeEvent(this.OnVoiceStateCreate, obj),
                "VOICE_STATE_UPDATE" => this.InvokeEvent(this.OnVoiceStateUpdate, obj),
                "VOICE_STATE_DELETE" => this.InvokeEvent(this.OnVoiceStateDelete, obj),
                "VOICE_SETTINGS_UPDATE" => this.InvokeEvent(this.OnVoiceSettingsUpdate, obj),
                "VOICE_CONNECTION_STATUS" => this.InvokeEvent(this.OnVoiceConnectionStatus, obj),
                "SPEAKING_START" => this.InvokeEvent(this.OnSpeakingStart, obj),
                "SPEAKING_STOP" => this.InvokeEvent(this.OnSpeakingStop, obj),
                "MESSAGE_CREATE" => this.InvokeEvent(this.OnMessageCreate, obj),
                "MESSAGE_UPDATE" => this.InvokeEvent(this.OnMessageUpdate, obj),
                "MESSAGE_DELETE" => this.InvokeEvent(this.OnMessageDelete, obj),
                "NOTIFICATION_CREATE" => this.InvokeEvent(this.OnNotificationCreate, obj),
                "ACTIVITY_JOIN" => this.InvokeEvent(this.OnActivityJoin, obj),
                "ACTIVITY_SPECTATE" => this.InvokeEvent(this.OnActivitySpectate, obj),
                "ACTIVITY_JOIN_REQUEST" => this.InvokeEvent(this.OnActivityJoinRequest, obj),
                _ => null
            };
        }
        
        public T InvokeEvent<T>(EventHandler<T> handler, JsonElement element) {
            T obj = element.ToObject<T>();
            
            handler.Invoke(this, obj);
            
            return obj;
        }
        
        #endregion
        
        public void Dispose() => this.Pipe.Dispose();
        
        #region Private methods
        
        private async Task SendMessageAsync(IPCMessage message, bool authorized = true, CancellationToken cancellationToken = default) {
            byte[] bOpCode = BitConverter.GetBytes((int) message.OpCode);
            byte[] bLen = BitConverter.GetBytes(message.Length);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bOpCode);
                Array.Reverse(bLen);
            }
            
            byte[] buffer = new byte[4 + 4 + message.Length];
            Array.Copy(bOpCode, buffer, 4);
            Array.Copy(bLen, 0, buffer, 4, 4);
            Array.Copy(message.Data, 0, buffer, 8, message.Length);
            await this.Pipe.WriteAsync(buffer, 0, buffer.Length, authorized, cancellationToken);
            Util.Log("TRANSMIT: {0}", message.RawData);
        }
        
        private async Task SendMessageAsync(Stream client, IPCMessage message, CancellationToken cancellationToken = default) {
            byte[] bOpCode = BitConverter.GetBytes((int) message.OpCode);
            byte[] bLen = BitConverter.GetBytes(message.Length);
            if (!BitConverter.IsLittleEndian) {
                Array.Reverse(bOpCode);
                Array.Reverse(bLen);
            }
            
            byte[] buffer = new byte[4 + 4 + message.Length];
            Array.Copy(bOpCode, buffer, 4);
            Array.Copy(bLen, 0, buffer, 4, 4);
            Array.Copy(message.Data, 0, buffer, 8, message.Length);
            Util.Log("TRANSMIT: {0}", message.RawData);
            await client.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }
        
        #endregion
        
        internal static Task EmptyHello(LowLevelDiscordIPC ipc, CancellationToken token) => Task.CompletedTask;
        internal static bool IsAuthorizedPayload(ICommandArgs payload) => !(payload is Authenticate.Args || payload is Authorize.Args);
    }
}
