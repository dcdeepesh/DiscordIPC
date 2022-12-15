using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core; 

internal class MessageLoop {
    private readonly NamedPipeClientStream _pipe;
    private readonly Thread _thread;
    
    public MessageLoop(NamedPipeClientStream pipe) {
        _pipe = pipe;
        _thread = new Thread(Loop) {
            IsBackground = true,
            Name = "Message loop"
        };
    }

    public void Start() => _thread.Start();

    private void Loop() {
        byte[] bOpCode = new byte[4];
        byte[] bLen = new byte[4];
        IpcRawPacket packet;

        while (true) {
            try {
                if (_pipe.Read(bOpCode, 0, 4) == 0)
                    break;
                OpCode opCode = (OpCode) BitConverter.ToInt32(bOpCode, 0);
                if (_pipe.Read(bLen, 0, 4) == 0)
                    break;
                int len = BitConverter.ToInt32(bLen, 0);
                byte[] data = new byte[len];
                if (_pipe.Read(data, 0, len) == 0)
                    break;
                packet = new IpcRawPacket(opCode, data);
            } catch (Exception e) when (e is ObjectDisposedException or InvalidOperationException) {
                break;
            }

            Task.Run(() => {
                Util.Log("\nRECEIVED:\n{0}", packet.Json);
                IpcPayload payload = JsonDocument.Parse(packet.Json).RootElement.ToObject<IpcPayload>();

                if (payload.cmd == "DISPATCH")
                    EventReceived?.Invoke(this, new PayloadReceivedArgs(payload));
                else
                    ResponseReceived?.Invoke(this, new PayloadReceivedArgs(payload));
            });
        }
    }

    public event EventHandler<PayloadReceivedArgs> EventReceived;
    public event EventHandler<PayloadReceivedArgs> ResponseReceived;
}

internal class PayloadReceivedArgs {
    public IpcPayload Payload { get; }

    public PayloadReceivedArgs(IpcPayload payload) => Payload = payload;
}

internal class Waiter {
    public string Nonce;
    public AutoResetEvent ResetEvent = new(false);
    public IpcPayload Response;

    public Waiter(string nonce) => Nonce = nonce;
}