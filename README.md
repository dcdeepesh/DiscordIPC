# DiscordIPC
DiscordIPC is a wrapper for Discord's IPC-based RPC. It supports all commands and events unlike the other wrappers that support only activity-related commands and events.

## This is a Fork
This repo is a fork of [DiscordIPC](https://www.github.com/dcdeepesh/DiscordIPC) created by [dcdeepesh](https://www.github.com/dcdeepesh).

The fork was mainly created because some things didn't work in my Linux environment, and I wanted to fix them. A couple of other tweaks were made as well:

```diff
+ Added named pipe support for Linux
+ Changed C# version to 8.0
+ Fixed some typo's that were present in Objects used for JSON serialization
+ Updated some Objects for JSON Serialization that were updated by Discord
+ Added CancellationTokens to await methods that are Cancelled when calling .Dispose() on the IPC Client
 - Not cancelling awaiting methods can cause the app to hang when waiting for ipc messages
+ Changed JSON Serialization to use JsonPropertyNameAttributes
+ Added Discord Documentation to some objects
```

## Before you use it
Because Discord's RPC is still under private beta, there are many inconsistencies, errors and outdated information in the documentation. See [problems and changes](#problems-and-changes) for more.

**Discord RPC and IPC are currently only allowed to be used in [Applications](https://discord.com/developers/applications) whitelisted by Discord. Currently Discord is not accepting any new applications onto the whitelist.** The only way to currently bypass the whitelist is to add any accounts to the `Application Testers` list of your application.
1) Create an [Application](https://discord.com/developers/applications)
2) Add accounts to the `Application Testers` list
3) Use RPC on those accounts

Discord IPC still has a number of bugs in it that can cause issues in runtime. If the client is moved into a channel that it does not have permission to join, calling `GET_SELECTED_VOICE_CHANNEL` will never get a response back from Discord. This will cause any `await`ing methods to hang indefinitely. CancellationTokens have been built into this Fork to safely stop the application if this occurs.

# Usage
Here is a generic way of connecting to the IPC socket briefly:
```c#
using Dec.DiscordIPC;
using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Events;

// Replace CLIENT_ID with your Applications Client ID
private const string CLIENT_ID = "<CLIENT-ID>";

static async Task Main(string[] args) {
    // Create the IPC Connection with the Client ID
    using DiscordIPC discordIPC = new DiscordIPC(Program.CLIENT_ID));
    
    // Start the IPC reading thread
    // The thread will automatically reconnect to IPC
    //   if the connection is lost, or if Discord is
    //   restarted.
    discordIPC.Start();
    
    Authorize.Data data;
    
    // Authorize
    try {
        // Send a popup to the Discord Client to authenticate
        data = await discordIPC.SendCommandAsync(new Authorize.Args() {
            Scopes = new List<string>() { "rpc" },
            ClientID = CLIENT_ID,
            CallbackURL = "http://localhost"
        });
    } catch (ErrorResponseException e) {
        Console.Log("User denied authorization");
        return;
    }
    
    // TODO: ... Leverage 'data' to fetch an OAuth Access Token
    string accessToken = "<ACCESS-TOKEN>";
    
    // Authenticate using accessToken (ignoring the response here)
    await discordIPC.SendCommandAsync(new Authenticate.Args() {
        AccessToken = accessToken
    });
    
    // Add an event listener by creating a lamba callback
    // (Or Reference to a method)
    discordIPC.OnMessageCreate += (sender, data) => Console.Log("New message!");
    
    // Send the event subscribe so we start receiving on our listener
    await discordIPC.SubscribeAsync(new MessageCreate.Args() {
        ChannelID = "<some-text-channel-id>"
    });
    
    // Use commands to get information about a channel
    GetChannel.Data response = await discordIPC.SendCommandAsync(new GetChannel.Args() {
        ChannelID = "<some-channel-id>"
    });
    Console.Log(response.name);
    
    // TODO: ... (do other stuff)
    
    // Our discordIPC object is discarded here due to the using statement
}
```

If you want to be able to set the IPC socket and then forget it, it can be created in such a way that it will automatically run a login event after connecting, and each time after it disconnects and reconnects.

```c#
using Dec.DiscordIPC;
using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Events;

// Replace CLIENT_ID with your Applications Client ID
private const string CLIENT_ID = "<CLIENT-ID>";

static void Main(string[] args) {
    // Create the IPC Connection
    DiscordIPC discordIPC = new DiscordIPC(
        Program.CLIENT_ID, // Pass in the Client ID for Authenticating
        Authenticate, // Authenticate method, called automatically after connecting
        Listen // Listening method, called automatically after READY event received
    );
    
    // Start the IPC reading thread
    // The thread will automatically reconnect to IPC
    //   if the connection is lost, or if Discord is
    //   restarted.
    discordIPC.Start();
}

// The Authenticate method will be called 1 second after the IPC pipe opens
// This is where you can authenticate with the client in order to make priviledged requests
// Attempting to run any priviledged requests here (Such as Subscribing to Events) will hang
static async Task Authenticate(DiscordIPC discordIPC, CancellationToken cancellationToken) {
    Authorize.Data data;
    
    // Authorize
    try {
        // Send a popup to the Discord Client to authenticate
        data = await discordIPC.SendCommandAsync(new Authorize.Args() {
            Scopes = new List<string>() { "rpc" },
            ClientID = CLIENT_ID,
            CallbackURL = "http://localhost"
        });
    } catch (ErrorResponseException e) {
        Console.Log("User denied authorization");
        return;
    }
    
    // TODO: ... Leverage 'data' to fetch an OAuth Access Token
    string accessToken = "<ACCESS-TOKEN>";
    
    // Authenticate using accessToken (ignoring the response here)
    await discordIPC.SendCommandAsync(new Authenticate.Args() {
        AccessToken = accessToken
    });
    
    // Add an event listener by creating a lamba callback
    // (Or Reference to a method)
    discordIPC.OnMessageCreate += (sender, data) => Console.Log("New message!");
}

// The Listen method runs after the Authenticate method has completed,
// and Discord sends the READY payload. Here priviledged requests such as subscribing
// to events can be made.
static async Task Listen(DiscordIPC discordIPC, CancellationToken cancellationToken) {
    // Send the event subscribe so we start receiving on our listener
    await discordIPC.SubscribeAsync(new MessageCreate.Args() {
        ChannelID = "<some-text-channel-id>"
    });
    
    // Use commands to get information about a channel
    GetChannel.Data response = await discordIPC.SendCommandAsync(new GetChannel.Args() {
        ChannelID = "<some-channel-id>"
    });
    Console.Log(response.name);
    
    // TODO: ... (do other stuff)
}
```