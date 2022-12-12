using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core; 

public class LowLevelDiscordIpc {
    private NamedPipeClientStream _pipe;
    internal MessageReadLoop _messageReadLoop;
    protected readonly string clientId;

    public LowLevelDiscordIpc(string clientId, bool verbose) {
        this.clientId = clientId;
        Util.Verbose = verbose;
    }

    /// <summary>
    /// Initializes the IPC client. Use this before calling any other methods.
    /// </summary>
    /// <remarks>
    /// Attempts to connect to <c>discord-ipc-&lt;pipeNumber&gt;</c> with a 2 second<br/>
    /// timeout and throws an IOException if connection is unsuccessful.<br/>
    /// No need to specify the pipe number explicitly unless connecting to a<br/>
    /// secondary discord client (e.g. Canary).
    /// </remarks>
    /// <param name="pipeNumber">Pipe number to connect to.</param>
    /// <returns></returns>
    public async Task InitAsync(int pipeNumber = 0) {
        string pipeName = "discord-ipc-" + pipeNumber;
        try {
            _pipe = new NamedPipeClientStream(".", pipeName,
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await _pipe.ConnectAsync(2000);
        } catch (TimeoutException) {
            throw new IOException("Could not connect to pipe " + pipeName);
        }

        _messageReadLoop = new MessageReadLoop(_pipe, this);
        _messageReadLoop.Start();

        EventWaitHandle readyWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        EventHandler<ReadyEvent.Data> readyListener = (_, _) => readyWaitHandle.Set();
        OnReady += readyListener;

        await SendPacketAsync(new IpcRawPacket(OpCode.Handshake, new {
            client_id = clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }));

        await Task.Run(() => {
            readyWaitHandle.WaitOne();
            OnReady -= readyListener;
        });
    }

    public async Task<JsonElement> SendPayloadAsync(IpcPayload payload) {
        await SendPacketAsync(new IpcRawPacket(OpCode.Frame, payload));
        return await _messageReadLoop.WaitForResponse(payload.nonce);
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

    /// <summary>
    /// Disposes the client. Use when the client is no longer in use.
    /// </summary>
    public void Dispose() => _pipe.Dispose();

    #region Private methods

    private async Task SendPacketAsync(IpcRawPacket packet) {
        byte[] bOpCode = BitConverter.GetBytes((int) packet.OpCode);
        byte[] bLen = BitConverter.GetBytes(packet.Length);
        if (!BitConverter.IsLittleEndian) {
            Array.Reverse(bOpCode);
            Array.Reverse(bLen);
        }

        byte[] buffer = new byte[4 + 4 + packet.Length];
        Array.Copy(bOpCode, buffer, 4);
        Array.Copy(bLen, 0, buffer, 4, 4);
        Array.Copy(packet.Data, 0, buffer, 8, packet.Length);
        Util.Log("\nSENDING:\n{0}", packet.Json);
        await _pipe.WriteAsync(buffer, 0, buffer.Length);
    }

    #endregion
}