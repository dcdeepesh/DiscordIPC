using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core; 

internal class MessageReadLoop {
    private readonly LowLevelDiscordIpc _ipcInstance;
    private readonly NamedPipeClientStream _pipe;
    private readonly Thread _thread;
    private readonly LinkedList<Waiter> _waiters = new();
    private readonly LinkedList<JsonElement> _responses = new();

    public MessageReadLoop(NamedPipeClientStream pipe, LowLevelDiscordIpc ipcInstance) {
        this._pipe = pipe;
        this._ipcInstance = ipcInstance;
        _thread = new Thread(Loop) {
            IsBackground = true,
            Name = "Message loop"
        };
    }

    public void Start() => _thread.Start();

    public Task<JsonElement> WaitForResponse(string nonce) {
        return Task.Run(() => {
            Waiter waiter;
            lock (_responses) {
                JsonElement? result = null;
                foreach (var response in _responses)
                    if (response.GetProperty("nonce").GetString() == nonce)
                        result = response;
                if (result.HasValue) {
                    _responses.Remove(result.Value);
                    if (result.Value.IsErrorResponse())
                        throw new ErrorResponseException(result.Value);
                    return result.Value;
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
                var jsonRoot = JsonDocument.Parse(packet.Json).RootElement;
                string cmd = jsonRoot.GetProperty("cmd").GetString();
                string evt = "";
                if (jsonRoot.TryGetProperty("evt", out JsonElement elem))
                    evt = elem.GetString();

                if (cmd == "DISPATCH")
                    _ipcInstance.FireEvent(evt, packet);
                else
                    SignalNewResponse(packet);
            });
        }
    }

    private void SignalNewResponse(IpcRawPacket packet) {
        JsonElement response = Json.Deserialize<dynamic>(packet.Json);
        lock (_responses) {
            Waiter waiterToResume = null;
            foreach (var waiter in _waiters)
                if (waiter.Nonce == response.GetProperty("nonce").GetString())
                    waiterToResume = waiter;

            if (waiterToResume is null is false) {
                _waiters.Remove(waiterToResume);
                waiterToResume.Response = response;
                waiterToResume.ResetEvent.Set();
            } else {
                _responses.AddLast(response);
            }
        }
    }
}

internal class Waiter {
    public string Nonce;
    public AutoResetEvent ResetEvent = new(false);
    public JsonElement Response;

    public Waiter(string nonce) => this.Nonce = nonce;
}