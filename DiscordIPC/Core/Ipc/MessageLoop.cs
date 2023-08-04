using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core.Ipc;

internal class MessageLoop
{
    private readonly NamedPipeClientStream _pipe;
    private readonly Thread _thread;

    public MessageLoop(NamedPipeClientStream pipe)
    {
        _pipe = pipe;
        _thread = new Thread(Loop)
        {
            IsBackground = true,
            Name = "Message loop"
        };
    }

    public void Start() => _thread.Start();

    public event EventHandler<PayloadReceivedArgs> EventReceived;
    public event EventHandler<PayloadReceivedArgs> ResponseReceived;

    private void Loop()
    {
        byte[] opCodeBytes = new byte[4];
        byte[] lengthBytes = new byte[4];
        IpcPacket packet;

        while (true)
        {
            try
            {
                if (_pipe.Read(opCodeBytes, 0, 4) == 0)
                    break;
                OpCode opCode = (OpCode)BitConverter.ToInt32(opCodeBytes, 0);
                if (_pipe.Read(lengthBytes, 0, 4) == 0)
                    break;
                int len = BitConverter.ToInt32(lengthBytes, 0);
                byte[] data = new byte[len];
                if (_pipe.Read(data, 0, len) == 0)
                    break;
                packet = new IpcPacket(opCode, data);
            }
            catch (Exception e) when (e is ObjectDisposedException or InvalidOperationException)
            {
                break;
            }

            Task.Run(() =>
            {
                IpcPacketPayload payload = JsonSerializer.Deserialize<IpcPacketPayload>(packet.PayloadJson);
                payload.DataJson = JsonDocument.Parse(packet.PayloadJson).RootElement
                    .GetProperty(nameof(IpcPacketPayload.data)).GetRawText();

                var eventToFire = payload.cmd == "DISPATCH" ? EventReceived : ResponseReceived;
                eventToFire?.Invoke(this, new PayloadReceivedArgs(payload));
            });
        }
    }
}

internal class PayloadReceivedArgs
{
    public IpcPacketPayload Payload { get; }

    public PayloadReceivedArgs(IpcPacketPayload payload) => Payload = payload;
}