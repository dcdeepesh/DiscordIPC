using System;
using System.IO.Pipes;
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
    
    public event EventHandler<PayloadReceivedArgs> EventReceived;
    public event EventHandler<PayloadReceivedArgs> ResponseReceived;

    private void Loop() {
        byte[] opCodeBytes = new byte[4];
        byte[] lengthBytes = new byte[4];
        IpcRawPacket packet;

        while (true) {
            try {
                if (_pipe.Read(opCodeBytes, 0, 4) == 0)
                    break;
                OpCode opCode = (OpCode) BitConverter.ToInt32(opCodeBytes, 0);
                if (_pipe.Read(lengthBytes, 0, 4) == 0)
                    break;
                int len = BitConverter.ToInt32(lengthBytes, 0);
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

                var eventToFire = payload.cmd == "DISPATCH" ? EventReceived : ResponseReceived;
                eventToFire?.Invoke(this, new PayloadReceivedArgs(payload));
            });
        }
    }
}

internal class PayloadReceivedArgs {
    public IpcPayload Payload { get; }

    public PayloadReceivedArgs(IpcPayload payload) => Payload = payload;
}