using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dec.DiscordIPC.Commands.Payloads;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core {
    public class LowLevelDiscordIPC : IDisposable {
        private NamedPipeClientStream Pipe;
        internal MessageReadLoop MessageReadLoop;
        protected readonly string ClientId;
        /*private readonly CancellationTokenSource SourceToken = new CancellationTokenSource();
        protected CancellationToken CancellationToken => this.SourceToken.Token;*/
        private string NamedPipe {
            get {
                const string NAME = "discord-ipc-0";
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? $"{Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")}/{NAME}" : NAME;
            }
        }
        
        public LowLevelDiscordIPC(string clientId, bool verbose) {
            this.ClientId = clientId;
            Util.Verbose = verbose;
        }
        
        public async Task InitAsync(CancellationToken cancellationToken = default) {
            this.Pipe = new NamedPipeClientStream(".", this.NamedPipe, PipeDirection.InOut, PipeOptions.Asynchronous);
            
            Console.WriteLine("CONNECT: Pipe connecting");
            await this.Pipe.ConnectAsync(cancellationToken);
            Console.WriteLine("CONNECT: Pipe connected");
            
            this.MessageReadLoop = new MessageReadLoop(this.Pipe, this);
            this.MessageReadLoop.Start();
            
            EventWaitHandle readyWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            void ReadyListener(object sender, Ready.Data data) => readyWaitHandle.Set();
            this.OnReady += ReadyListener;
            
            await this.SendMessageAsync(IPCMessage.Handshake(Json.SerializeToBytes(new {
                client_id = this.ClientId,
                v = "1",
                nonce = Guid.NewGuid().ToString()
            })), cancellationToken);
            
            await Task.Run(() => {
                readyWaitHandle.WaitOne();
                this.OnReady -= ReadyListener;
            }, cancellationToken);
        }
        
        public async Task<JsonElement> SendCommandAsync(CommandPayload payload, CancellationToken cancellationToken = default) {
            await this.SendMessageAsync(new IPCMessage(OpCode.FRAME, Json.SerializeToBytes<dynamic>(payload)), cancellationToken);
            return await this.MessageReadLoop.WaitForResponse(payload.Nonce, cancellationToken);
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
            JsonElement obj = Json.Deserialize<dynamic>(message.Json).GetProperty("data");
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
        
        private async Task SendMessageAsync(IPCMessage message, CancellationToken cancellationToken = default) {
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
            Util.Log("TRANSMIT: {0}", message.Json);
            await this.Pipe.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }
        
        #endregion
    }
}
