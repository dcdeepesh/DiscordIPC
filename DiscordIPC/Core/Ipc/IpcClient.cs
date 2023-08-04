using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Dec.DiscordIPC.Core.Ipc;

public class IpcClient  {
    
    public NamedPipeClientStream Pipe { get; private set; }

    public async Task ConnectToPipeAsync(int pipeNumber, int timeoutMs,
        CancellationToken ctk = default)  {
        
        string pipeName = $"discord-ipc-{pipeNumber}";
        try  {
            Pipe = new NamedPipeClientStream(".", pipeName,
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await Pipe.ConnectAsync(timeoutMs, ctk).ConfigureAwait(false);
        } catch (TimeoutException ex)  {
            throw new IOException($"Unable to connect to pipe {pipeName}", ex);
        }
    }
    
    public void SendPacket(Packet packet) {
        byte[] packetBytes = SerializePacket(packet);
        Pipe.Write(packetBytes, 0, packetBytes.Length);
    }

    public async Task SendPacketAsync(Packet packet, CancellationToken ctk = default) {
        byte[] packetBytes = SerializePacket(packet);
        await Pipe.WriteAsync(packetBytes, 0, packetBytes.Length, ctk)
            .ConfigureAwait(false);
    }

    private static byte[] SerializePacket(Packet packet) {
        byte[] opCodeBytes = BitConverter.GetBytes((int)packet.OpCode);
        byte[] lengthBytes = BitConverter.GetBytes(packet.PayloadLength);

        // 4-bit opcode, 4-bit length, and then the data
        byte[] buffer = new byte[4 + 4 + packet.PayloadLength];
        Array.Copy(opCodeBytes, buffer, 4);
        Array.Copy(lengthBytes, 0, buffer, 4, 4);
        Array.Copy(packet.Payload, 0, buffer, 8, packet.PayloadLength);

        return buffer;
    }

    public void Dispose() => Pipe.Dispose();
}