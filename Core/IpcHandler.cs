using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core; 

public class IpcHandler {
    private NamedPipeClientStream _pipe;
    private MessageReadLoop _messageReadLoop;
    private readonly string _clientId;

    public IpcHandler(string clientId, bool verbose) {
        Util.Verbose = verbose;
        _clientId = clientId;
    }

    public async Task ConnectToPipeAsync(int pipeNumber = 0, int timeoutMs = 2000,
        CancellationToken ctk = default) {
        
        string pipeName = $"discord-ipc-{pipeNumber}";
        try {
            _pipe = new NamedPipeClientStream(".", pipeName,
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await _pipe.ConnectAsync(timeoutMs, ctk);
        }
        catch (TimeoutException) {
            throw new IOException($"Unable to connect to pipe {pipeName}");
        }

        _messageReadLoop = new MessageReadLoop(_pipe, this);
        _messageReadLoop.Start();
    }
    
    public async Task SendHandshakeAsync(CancellationToken ctk = default) {
        EventWaitHandle readyEventWaitHandle = new(false, EventResetMode.ManualReset);
        OnReady += ReadyEventListener;

        // TODO: use ctk
        await SendPacketAsync(new IpcRawPacket(OpCode.Handshake, new {
            client_id = _clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }));

        // TODO: make this async
        await Task.Run(() => {
            readyEventWaitHandle.WaitOne();
            OnReady -= ReadyEventListener;
        }, ctk);

        void ReadyEventListener(object sender, ReadyEvent.Data data) {
            readyEventWaitHandle.Set();
        }
    }

    public async Task<JsonElement> SendPayloadAsync(IpcPayload payload) {
        await SendPacketAsync(new IpcRawPacket(OpCode.Frame, payload));
        return await _messageReadLoop.WaitForResponse(payload.nonce);
    }

    protected async Task SendPacketAsync(IpcRawPacket packet) {
        byte[] opCodeBytes = BitConverter.GetBytes((int) packet.OpCode);
        byte[] lengthBytes = BitConverter.GetBytes(packet.Length);

        // 4-bit opcode, 4-bit length, and then the data
        byte[] buffer = new byte[4 + 4 + packet.Length];
        Array.Copy(opCodeBytes, buffer, 4);
        Array.Copy(lengthBytes, 0, buffer, 4, 4);
        Array.Copy(packet.Data, 0, buffer, 8, packet.Length);
        
        Util.Log("\nSENDING:\n{0}", packet.Json);
        await _pipe.WriteAsync(buffer, 0, buffer.Length);
    }
    
    #region Events

    public event EventHandler<ReadyEvent.Data> OnReady;
    // Event ERROR is handled differently
    public event EventHandler<GuildStatusEvent.Data> OnGuildStatus;
    public event EventHandler<GuildCreateEvent.Data> OnGuildCreate;
    public event EventHandler<ChannelCreateEvent.Data> OnChannelCreate;
    public event EventHandler<VoiceChannelSelectEvent.Data> OnVoiceChannelSelect;
    public event EventHandler<VoiceStateCreateEvent.Data> OnVoiceStateCreate;
    public event EventHandler<VoiceStateUpdateEvent.Data> OnVoiceStateUpdate;
    public event EventHandler<VoiceStateDeleteEvent.Data> OnVoiceStateDelete;
    public event EventHandler<VoiceSettingsUpdateEvent.Data> OnVoiceSettingsUpdate;
    public event EventHandler<VoiceConnectionStatusEvent.Data> OnVoiceConnectionStatus;
    public event EventHandler<SpeakingStartEvent.Data> OnSpeakingStart;
    public event EventHandler<SpeakingStopEvent.Data> OnSpeakingStop;
    public event EventHandler<MessageCreateEvent.Data> OnMessageCreate;
    public event EventHandler<MessageUpdateEvent.Data> OnMessageUpdate;
    public event EventHandler<MessageDeleteEvent.Data> OnMessageDelete;
    public event EventHandler<NotificationCreateEvent.Data> OnNotificationCreate;
    public event EventHandler<ActivityJoinEvent.Data> OnActivityJoin;
    public event EventHandler<ActivitySpectateEvent.Data> OnActivitySpectate;
    public event EventHandler<ActivityJoinRequestEvent.Data> OnActivityJoinRequest;

    // More events on their way

    internal void FireEvent(string evt, IpcRawPacket packet) {
        JsonElement obj = Json.Deserialize<dynamic>(packet.Json).GetProperty("data");
        switch (evt) {
            case "READY":
                OnReady?.Invoke(this, obj.ToObject<ReadyEvent.Data>());
                break;

            case "GUILD_STATUS":
                OnGuildStatus?.Invoke(this, obj.ToObject<GuildStatusEvent.Data>());
                break;

            case "GUILD_CREATE":
                OnGuildCreate?.Invoke(this, obj.ToObject<GuildCreateEvent.Data>());
                break;

            case "CHANNEL_CREATE":
                OnChannelCreate?.Invoke(this, obj.ToObject<ChannelCreateEvent.Data>());
                break;

            case "VOICE_CHANNEL_SELECT":
                OnVoiceChannelSelect?.Invoke(this, obj.ToObject<VoiceChannelSelectEvent.Data>());
                break;

            case "VOICE_STATE_CREATE":
                OnVoiceStateCreate?.Invoke(this, obj.ToObject<VoiceStateCreateEvent.Data>());
                break;

            case "VOICE_STATE_UPDATE":
                OnVoiceStateUpdate?.Invoke(this, obj.ToObject<VoiceStateUpdateEvent.Data>());
                break;

            case "VOICE_STATE_DELETE":
                OnVoiceStateDelete?.Invoke(this, obj.ToObject<VoiceStateDeleteEvent.Data>());
                break;

            case "VOICE_SETTINGS_UPDATE":
                OnVoiceSettingsUpdate?.Invoke(this, obj.ToObject<VoiceSettingsUpdateEvent.Data>());
                break;

            case "VOICE_CONNECTION_STATUS":
                OnVoiceConnectionStatus?.Invoke(this, obj.ToObject<VoiceConnectionStatusEvent.Data>());
                break;

            case "SPEAKING_START":
                OnSpeakingStart?.Invoke(this, obj.ToObject<SpeakingStartEvent.Data>());
                break;

            case "SPEAKING_STOP":
                OnSpeakingStop?.Invoke(this, obj.ToObject<SpeakingStopEvent.Data>());
                break;

            case "MESSAGE_CREATE":
                OnMessageCreate?.Invoke(this, obj.ToObject<MessageCreateEvent.Data>());
                break;

            case "MESSAGE_UPDATE":
                OnMessageUpdate?.Invoke(this, obj.ToObject<MessageUpdateEvent.Data>());
                break;

            case "MESSAGE_DELETE":
                OnMessageDelete?.Invoke(this, obj.ToObject<MessageDeleteEvent.Data>());
                break;

            case "NOTIFICATION_CREATE":
                OnNotificationCreate?.Invoke(this, obj.ToObject<NotificationCreateEvent.Data>());
                break;

            case "ACTIVITY_JOIN":
                OnActivityJoin?.Invoke(this, obj.ToObject<ActivityJoinEvent.Data>());
                break;

            case "ACTIVITY_SPECTATE":
                OnActivitySpectate?.Invoke(this, obj.ToObject<ActivitySpectateEvent.Data>());
                break;

            case "ACTIVITY_JOIN_REQUEST":
                OnActivityJoinRequest?.Invoke(this, obj.ToObject<ActivityJoinRequestEvent.Data>());
                break;
        }
    }

    #endregion
    
    public void Dispose() => _pipe.Dispose();
}