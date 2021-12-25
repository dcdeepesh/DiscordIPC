using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Dec.DiscordIPC.Core {
    internal sealed class LeakyPipeFactory {
        private readonly IPCHello<NamedPipeClientStream> OnStreamConnectEvent;
        private readonly Func<Task> AfterStreamHelloEvent;
        private readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
        
        private int MinIPC = 0;
        private int MaxIPC = 0;
        private int IPCDiff => this.MaxIPC - this.MinIPC + 1;
        
        private readonly AsyncManualResetEvent IsConnected = new AsyncManualResetEvent(false);
        private readonly AsyncManualResetEvent SentHello = new AsyncManualResetEvent(false);
        
        private NamedPipeClientStream Client;
        private Task HelloTask;
        private CancellationTokenSource CancellationToken = new CancellationTokenSource();
        
        public LeakyPipeFactory(
            IPCHello<NamedPipeClientStream> onStreamConnectEvent,
            Func<Task> afterHelloEvent
        ) {
            this.OnStreamConnectEvent = onStreamConnectEvent;
            this.AfterStreamHelloEvent = afterHelloEvent;
        }
        
        /// <summary>
        /// Clamps the IPC Connection to a range of IPC sockets
        /// When disconnected, as long as 'Start' has been called, the client will attempt connecting to the range of pipes in a round-robin
        /// </summary>
        public void Clamp(int min, int max) {
            this.MinIPC = Math.Max(min, 0);
            this.MaxIPC = Math.Max(max, this.MinIPC);
        }
        
        /// <summary>
        /// Wait for the Stream to Connect
        /// </summary>
        public Task AwaitConnectedAsync(CancellationToken cancellationToken = default) => this.IsConnected.WaitAsync(cancellationToken);
        
        /// <summary>
        /// Wait for the HELLO Event (After connecting) to have been sent
        /// </summary>
        public Task AwaitHelloAsync(CancellationToken cancellationToken = default) => this.SentHello.WaitAsync(cancellationToken);
        
        /// <summary>
        /// Await a pipe connection
        /// </summary>
        /// <param name="awaitHello">If the HELLO Event should have completed before attempting to get the connection</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NamedPipeClientStream> AwaitOrCreateConnectionAsync(bool awaitHello, CancellationToken cancellationToken = default) {
            // Awaiting being connected
            await this.IsConnected.WaitAsync(cancellationToken);
            
            // Await authorization completion
            if (awaitHello)
                await this.SentHello.WaitAsync(cancellationToken);
            
            // Try reading the client
            this.Lock.EnterReadLock();
            try {
                return this.Client;
            } finally {
                this.Lock.ExitReadLock();
            }
        }
        
        /// <summary>
        /// Create a new open connection
        /// </summary>
        /// <returns>A new opened client stream</returns>
        public NamedPipeClientStream AwaitConnection(CancellationToken cancellationToken) {
            NamedPipeClientStream stream;
            do {
                Console.WriteLine("Waiting in loop for connection");
                stream = this.GetOrCreateConnection(cancellationToken);
            } while (this.PipeIsDead());
            return stream;
        }
        
        /// <summary>
        /// Get the last connection or create a new one
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A client stream if one exists</returns>
        private NamedPipeClientStream GetOrCreateConnection(CancellationToken cancellationToken = default) {
            this.Lock.EnterUpgradeableReadLock();
            try {
                // Check if we need to re-create the pipe
                if (this.PipeIsDead()) {
                    this.Lock.EnterWriteLock();
                    
                    // Check again after getting the lock
                    if (!this.PipeIsDead())
                        return this.Client;
                    
                    try {
                        // Reset if not done in .Dispose()
                        this.SentHello.Reset();
                        this.IsConnected.Reset();
                        
                        // Cancel the old authentication task if it is running
                        if (this.HelloTask is {IsCompleted: true} is false) {
                            this.CancellationToken?.Cancel();
                            this.CancellationToken?.Dispose();
                        }
                        // Create a fresh CancellationTokenSource for each new connection
                        if (this.CancellationToken is {IsCancellationRequested: true})
                            this.CancellationToken = new CancellationTokenSource();
                        
                        // Create the connection task for when the connection opens
                        CancellationToken inner = this.CancellationToken!.Token;
                        this.HelloTask = Task.Run(async() => {
                            // Wait for the pipe to be connected before triggering HELLO
                            await this.IsConnected.WaitAsync(inner);
                            
                            // Wait briefly after pipe opens
                            await Task.Delay(TimeSpan.FromSeconds(1), inner);
                            
                            // Run the connect event (Send Authenticate and Authorize)
                            await this.OnStreamConnectEvent(this.Client, inner);
                            
                            // Set that the Hello has been triggered
                            this.SentHello.Set();
                            
                            // Run post-authorization events
                            await this.AfterStreamHelloEvent();
                        }, inner);
                        
                        // Loop connection numbers
                        int attempts = 0;
                        
                        // Run as long as the cancellation token provided is not cancelled
                        while (!cancellationToken.IsCancellationRequested) {
                            string name = this.GetPipeName(attempts % this.IPCDiff);
                            Console.WriteLine($"Creating new client '{name}' stream");
                            NamedPipeClientStream clientStream = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous);
                            
                            try {
                                Console.WriteLine($"CONNECT: Pipe attempting connection to '{name}'..");
                                
                                // Try connecting for 2 seconds
                                clientStream.Connect(2000);
                                
                                // Set the connection
                                this.Client = clientStream;
                                
                                // Sleep the thread briefly before setting the pipe as "open"
                                Thread.Sleep(TimeSpan.FromSeconds(1));
                                
                                // Set that we're connected (Set as open)
                                this.IsConnected.Set();
                                
                                break;
                            } catch (TimeoutException) {}
                            
                            attempts++;
                        }
                    } finally {
                        this.Lock.ExitWriteLock();
                    }
                }
                
                Console.WriteLine("Getting client value");
                return this.Client;
            } finally {
                this.Lock.ExitUpgradeableReadLock();
            }
        }
        
        /// <summary>
        /// Check the current pipe status for if it is dead or disconnected
        /// </summary>
        private bool PipeIsDead() {
            // If connection is not "OPEN", LazyLoader is not initialized
            if (!this.IsConnected.IsSet || this.Client is null || this.HelloTask is null)
                return true;
            return !this.Client.IsConnected;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private string GetPipeName(int num) {
            string name = $"discord-ipc-{num}";
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? $"{Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")}/{name}" : name;
        }
        
        /// <summary>
        /// Close the current pipe and release its resources
        /// </summary>
        public void Close() {
            this.Client?.Dispose();
            this.CancellationToken?.Cancel();
            this.SentHello.Reset();
            this.IsConnected.Reset();
        }
    }
}