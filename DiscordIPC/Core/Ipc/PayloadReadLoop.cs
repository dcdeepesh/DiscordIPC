using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core.Ipc;

internal class PayloadReadLoop {
    private readonly NamedPipeClientStream _pipe;
    private readonly Thread _thread;

    public PayloadReadLoop(NamedPipeClientStream pipe) {
        _pipe = pipe;
        _thread = new Thread(Loop) {
            IsBackground = true,
            Name = "IPC Message read loop"
        };
    }

    public void Start() => _thread.Start();

    public event EventHandler<PayloadReceivedEventArgs> EventReceived;
    public event EventHandler<PayloadReceivedEventArgs> ResponseReceived;

    private void Loop() {
        byte[] opCodeBytes = new byte[4];
        byte[] lengthBytes = new byte[4];

        while (true) {
            try {
                if (_pipe.Read(opCodeBytes, 0, 4) == 0)
                    break;
                OpCode opCode = (OpCode)BitConverter.ToInt32(opCodeBytes, 0);
                if (_pipe.Read(lengthBytes, 0, 4) == 0)
                    break;
                int length = BitConverter.ToInt32(lengthBytes, 0);
                byte[] data = new byte[length];
                if (_pipe.Read(data, 0, length) == 0)
                    break;
                
                FireEventForPacket(new Packet(opCode, data));
            } catch (Exception e) when (e is ObjectDisposedException or InvalidOperationException) {
                // TODO why this catch block?
                break;
            }
        }
    }

    private void FireEventForPacket(Packet packet) {
        Task.Run(() => {
            Payload payload = JsonSerializer.Deserialize<Payload>(packet.PayloadJson);
            payload.DataJson = JsonDocument.Parse(packet.PayloadJson).RootElement
                .GetProperty(nameof(Payload.data)).GetRawText();

            var eventToFire = payload.cmd == "DISPATCH" ? EventReceived : ResponseReceived;
            eventToFire?.Invoke(this, new PayloadReceivedEventArgs(payload));
        });
    }
}