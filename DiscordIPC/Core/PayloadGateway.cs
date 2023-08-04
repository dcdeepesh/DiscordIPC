using System;
using System.Threading;
using System.Threading.Tasks;
using Dec.DiscordIPC.Core.Ipc;
using Dec.DiscordIPC.Events;

namespace Dec.DiscordIPC.Core;

public class PayloadGateway {

    private readonly string _clientId;
    private readonly IpcClient _ipcClient;
    private PayloadReadLoop _messageLoop;
    private readonly PayloadDispatcher _payloadDispatcher;

    public PayloadGateway(string clientId) {
        _clientId = clientId;
        _ipcClient = new IpcClient();
        _payloadDispatcher = new PayloadDispatcher();
    }
    
    public async Task InitComponentsAsync(int pipeNumber, int timeoutMs, CancellationToken ctk)  {
        // Initialize the IPC client
        await _ipcClient.ConnectToPipeAsync(pipeNumber, timeoutMs, ctk)
            .ConfigureAwait(false);
        
        // Initialize read loop and hook it with the dispatcher
        _messageLoop = new PayloadReadLoop(_ipcClient.Pipe);
        _messageLoop.EventReceived += (_, args) => _payloadDispatcher.DispatchEvent(args.Payload);
        _messageLoop.ResponseReceived += (_, args) => _payloadDispatcher.DispatchResponse(args.Payload);
        _messageLoop.Start();
    }
    
    public async Task PerformHandshakeAsync(CancellationToken ctk = default) {
        EventWaitHandle handshakeCompletionWaitHandle = new(false, EventResetMode.ManualReset);
        
        // Add READY event listener to detect handshake completion
        AddEventListener(EventListener.Create(ReadyEvent.Create(),
            _ => handshakeCompletionWaitHandle.Set()));
        
        // Initiate handshake
        await _ipcClient.SendPacketAsync(new Packet(OpCode.Handshake, new {
            client_id = _clientId,
            v = "1",
            nonce = Guid.NewGuid().ToString()
        }), ctk).ConfigureAwait(false);

        // Wait for handshake completion
        await Task.Run(() => {
            handshakeCompletionWaitHandle.WaitOne();
        }, ctk).ConfigureAwait(false);
    }

    public void AddEventListener(EventListener eventListener)
        => _payloadDispatcher.AddEventListener(eventListener);

    public Payload SendPayload(Payload payload)  {
        _ipcClient.SendPacket(new Packet(OpCode.Frame, payload));
        return _payloadDispatcher.WaitAndGetResponseFor(payload.nonce);
    }

    public async Task<Payload> SendPayloadAsync(Payload payload,
        CancellationToken ctk = default) {
        
        await _ipcClient.SendPacketAsync(new Packet(OpCode.Frame, payload), ctk)
            .ConfigureAwait(false);
        return _payloadDispatcher.WaitAndGetResponseFor(payload.nonce);
    }

    public void Dispose() {
        _ipcClient.Dispose();
    }
}