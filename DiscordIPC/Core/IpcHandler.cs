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
    private MessageLoop _messageLoop;
    private readonly Dispatcher _dispatcher;
    private readonly string _clientId;

    public IpcHandler(string clientId, bool verbose, Dispatcher dispatcher) {
        Util.Verbose = verbose;
        _clientId = clientId;
        _dispatcher = dispatcher;
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
    }

    public void InitMessageLoopAndDispatcher() {
        _messageLoop = new MessageLoop(_pipe);
        _messageLoop.EventReceived += (_, args) => {
            _dispatcher.DispatchEvent(args.Payload,
                JsonDocument.Parse(JsonSerializer.Serialize(args.Payload.data)).RootElement);
        };
        _messageLoop.ResponseReceived += (_, args) => {
            _dispatcher.DispatchResponse(args.Payload);
        };
        _messageLoop.Start();
    }
    
    public async Task SendHandshakeAsync(CancellationToken ctk = default) {
        EventWaitHandle readyEventWaitHandle = new(false, EventResetMode.ManualReset);
        _dispatcher.AddEventListener(
            EventListener.Create(ReadyEvent.Create(), ReadyEventListener));

        // TODO: use ctk
        await SendPacketAsync(new IpcRawPacket(OpCode.Handshake, new {
            client_id = _clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }));

        // TODO: make this async
        await Task.Run(() => {
            readyEventWaitHandle.WaitOne();
        }, ctk);

        void ReadyEventListener(ReadyEvent.Data _) {
            readyEventWaitHandle.Set();
        }
    }

    public async Task<IpcPayload> SendPayloadAsync(IpcPayload payload) {
        await SendPacketAsync(new IpcRawPacket(OpCode.Frame, payload));
        return _dispatcher.GetResponseFor(payload.nonce);
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

    public void Dispose() => _pipe.Dispose();
}