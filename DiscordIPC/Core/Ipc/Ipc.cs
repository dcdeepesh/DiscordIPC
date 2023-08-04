using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core.Ipc;

public class Ipc
{
    private NamedPipeClientStream _pipe;
    private MessageLoop _messageLoop;
    private readonly PayloadDispatcher _dispatcher;
    private readonly string _clientId;

    public Ipc(string clientId, PayloadDispatcher dispatcher)
    {
        _clientId = clientId;
        _dispatcher = dispatcher;
    }

    public async Task ConnectToPipeAsync(int pipeNumber, int timeoutMs,
        CancellationToken ctk = default)
    {

        string pipeName = $"discord-ipc-{pipeNumber}";
        try
        {
            _pipe = new NamedPipeClientStream(".", pipeName,
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await _pipe.ConnectAsync(timeoutMs, ctk).ConfigureAwait(false);
        }
        catch (TimeoutException ex)
        {
            throw new IOException($"Unable to connect to pipe {pipeName}", ex);
        }
    }

    public void InitMessageLoopAndDispatcher()
    {
        _messageLoop = new MessageLoop(_pipe);
        _messageLoop.EventReceived += (_, args) =>
        {
            _dispatcher.DispatchEvent(args.Payload);
        };
        _messageLoop.ResponseReceived += (_, args) =>
        {
            _dispatcher.DispatchResponse(args.Payload);
        };
        _messageLoop.Start();
    }

    public async Task SendHandshakeAsync(CancellationToken ctk = default)
    {
        EventWaitHandle readyEventWaitHandle = new(false, EventResetMode.ManualReset);
        _dispatcher.AddEventListener(
            EventListener.Create(ReadyEvent.Create(), ReadyEventListener));

        // TODO: use ctk
        await SendPacketAsync(new IpcPacket(OpCode.Handshake, new
        {
            client_id = _clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }), ctk).ConfigureAwait(false);

        // TODO: make this async
        await Task.Run(() =>
        {
            readyEventWaitHandle.WaitOne();
        }, ctk).ConfigureAwait(false);

        void ReadyEventListener(ReadyEvent.Data _)
        {
            readyEventWaitHandle.Set();
        }
    }

    public IpcPacketPayload SendPayload(IpcPacketPayload payload)
    {
        SendPacket(new IpcPacket(OpCode.Frame, payload));
        return _dispatcher.GetResponseFor(payload.nonce);
    }

    public async Task<IpcPacketPayload> SendPayloadAsync(IpcPacketPayload payload, CancellationToken ctk = default)
    {
        await SendPacketAsync(new IpcPacket(OpCode.Frame, payload), ctk)
            .ConfigureAwait(false);
        return _dispatcher.GetResponseFor(payload.nonce);
    }

    public void SendPacket(IpcPacket packet)
    {
        byte[] packetBytes = SerializePacket(packet);
        _pipe.Write(packetBytes, 0, packetBytes.Length);
    }

    public async Task SendPacketAsync(IpcPacket packet, CancellationToken ctk = default)
    {
        byte[] packetBytes = SerializePacket(packet);
        await _pipe.WriteAsync(packetBytes, 0, packetBytes.Length, ctk)
            .ConfigureAwait(false);
    }

    private static byte[] SerializePacket(IpcPacket packet)
    {
        byte[] opCodeBytes = BitConverter.GetBytes((int)packet.OpCode);
        byte[] lengthBytes = BitConverter.GetBytes(packet.PayloadLength);

        // 4-bit opcode, 4-bit length, and then the data
        byte[] buffer = new byte[4 + 4 + packet.PayloadLength];
        Array.Copy(opCodeBytes, buffer, 4);
        Array.Copy(lengthBytes, 0, buffer, 4, 4);
        Array.Copy(packet.Payload, 0, buffer, 8, packet.PayloadLength);

        return buffer;
    }

    public void Dispose() => _pipe.Dispose();
}