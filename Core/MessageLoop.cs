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
    private readonly LinkedList<Waiter> _waiters = new();
    private readonly LinkedList<IpcPayload> _responses = new();

    public MessageLoop(NamedPipeClientStream pipe) {
        _pipe = pipe;
        _thread = new Thread(Loop) {
            IsBackground = true,
            Name = "Message loop"
        };
    }

    public void Start() => _thread.Start();

    public Task<IpcPayload> WaitForResponse(string nonce) {
        return Task.Run(() => {
            Waiter waiter;
            lock (_responses) {
                IpcPayload result = null;
                // TODO: use LINQ and Single() instead?
                foreach (var response in _responses)
                    if (response.nonce == nonce)
                        result = response;
                if (result is not null) {
                    _responses.Remove(result);
                    if (result.IsErrorResponse())
                        throw new ErrorResponseException(result);
                    return result;
                }

                waiter = new Waiter(nonce);
                _waiters.AddLast(waiter);
            }

            waiter.ResetEvent.WaitOne();
            if (waiter.Response.IsErrorResponse())
                throw new ErrorResponseException(waiter.Response);
            return waiter.Response;
        });
    }

    // Private methods

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
                    SignalNewResponse(payload);
            });
        }
    }

    public event EventHandler<PayloadReceivedArgs> EventReceived;
    public event EventHandler<PayloadReceivedArgs> ResponseReceived;

    private void SignalNewResponse(IpcPayload payload) {
        lock (_responses) {
            // TODO: use Single() instead?
            Waiter waiterToResume = _waiters.FirstOrDefault(
                w => w.Nonce == payload.nonce);
            // Waiter waiterToResume = null;
            // foreach (var waiter in _waiters)
            //     if (waiter.Nonce == response.GetProperty("nonce").GetString())
            //         waiterToResume = waiter;

            if (waiterToResume is not null) {
                _waiters.Remove(waiterToResume);
                waiterToResume.Response = payload;
                waiterToResume.ResetEvent.Set();
            } else {
                _responses.AddLast(payload);
            }
        }
    }
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