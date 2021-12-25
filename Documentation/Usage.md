# Usage
This is a complete guide on how to use DiscordIPC. If you're lazy or just want a quick summary, head straight to the [summary](#summary).

# Table of contents
  - [Init/Dispose](#initdispose)
    - [Init](#init)
  - [Commands](#commands)
    - [Sending commands](#sending-commands)
    - [Which commands can be sent](#which-commands-can-be-sent)
  - [Events](#events)
    - [Subscribing](#subscribing)
    - [Unsubscribing](#unsubscribing)
    - [General usage](#general-usage)
    - [What events can I subscribe to?](#what-events-can-i-subscribe-to)
  - [Error responses](#error-responses)
  - [Summary](#summary)

# Init/Dispose

## Init
The first thing you want to do is instantiate `DiscordIPC`, providing it your client ID.
```c#
DiscordIPC discordIPC = new DiscordIPC("<YOUR-CLIENT-ID>");
```

The constructor takes a total of 4 properties,

- `string: clientID` This is the Client ID that is sent in the Handshake Event
- `IPCHello<DiscordIPC>: beforeAuthorize` Defaults to `Task.CompletedTask`. An async method that is called *before* authenticating. If you want to authenticate your connects, do so here. This `delegate` is called every time the pipe reconnects.
- `IPCHello<DiscordIPC>: afterAuthorize` Defaults to `Task.CompletedTask`. An async method that is called *after* authenticating. If you want to subscribe to events after connecting, do so here. This `delegate` is called every time the pipe reconnects.
- `bool: verbose` Defaults to `false`. Settings it to true will tell DiscordIPC to log every json sent and received.

`IPCHello` is a `delegate` that returns a `Task` and provides the constructed `DiscordIPC` and a `CancellationToken`

```c#
delegate Task IPCHello<in T>(T arg, CancellationToken cancellationToken = default)
```

No tasks are performed after constructing the `DiscordIPC` until `Start` is called, which will maintain the connection to the pipe until `Dispose` is called.
```c#
discordIPC.Start();
```

> If you want to receive the payload sent in the `READY` event, register a handler to the `OnReady` event _before_ calling `Init`. More [here](#what-events-can-i-subscribe-to).

```c#
discordIPC.Dispose();
```

`DiscordIPC` handles all of the processing of connecting to the Discord IPC Pipe, by default it only attempts connecting to `discord-ipc-0`, and doesn't check/connect to any other pipes. If the user uses multiple Discord clients (e.g. PTB or Canary).

The IPC pipes are named (by the Discord client) in the order of the clients opening (`discord-ipc-0`, `discord-ipc-1`, etc) and are not changed until that client restarts. Opening two clients and then closing the client responsible for ipc-0 will cause the default setup to never connect, as the remaining client will remain as ipc-1.

In order to change the range of clients one of the following methods can be called. The client will connect to the first available pipe.

```c#
// Specifying a connection number to clamp to will only ever
// attempt connecting to that socket, in this case ipc-1
discordIPC.ClampConnections(1);

// The default value for the method is to only connect to ipc-0
discordIPC.ClampConnections();

// Specifying a range of sockets will instruct the client to attempt
// connecting to the range in a round-robin order. First trying ipc-0,
// then ipc-1, then ipc-2, and then ipc-0 again. Each attempt will wait
// for two seconds before attempting the next connection
discordIPC.ClampConnections(0, 2);
```

# Commands
All commands are defined in the `Dec.DiscordIPC.Commands` namespace. Each command has a class, a subclass `Args` which defines the arguments to provide when sending that command, and a subclass `Data` which defines the data returned in response to that command.

## Sending commands
To send a command, do:
```c#
var responseData = await discordIPC.SendCommandAsync(/* args */);

// e.g. To send the GET_GUILD command
GetGuild.Data data = await discordIPC.SendCommandAsync(new GetGuild.Args() {
    guild_id = "<guild-id>"
});
```
where args is the `Args` subclass of one of the command classes, and `responseData` is the corresponding `Data` subclass. As shown in the example above, you don't need to define all properties in the arguments. Undefined properties default to null and are not sent to the IPC.

Some commands don't take any arguments, so their `Args` subclass is empty, but you still need to provide an instance to differentiate the overloads. Likewise, some commands don't return any data, so their `SendCommandAsync` overloads return nothing.

## Which commands can be sent
All of them! Not just the ones related to rich presence (a.k.a. activity).

Ok all except 3:
  - `DISPATCH`: because it's not meant to be sent from the client.
  - `SUBSCRIBE`, `UNSUBSCRIBE`: because they're used indirectly by calling other methods of `DiscordIPC`. See this next section for more.

If you don't know all the commands, their semantics and other things, [here's the documentation](https://discord.com/developers/docs/topics/rpc#commands-and-events-rpc-commands).

# Events
All events are defined in the `Dec.DiscordIPC.Events` namespace. Each event has a class, a subclass `Args` which defines the arguments to provide when subscribing to it, and a subclass `Data` which is provided to the delegate when that event is fired.

DiscordIPC uses C# style events to trigger Discord's events. When you're done with an event make sure you unsubscribe from it and de-register the delegate.

## Subscribing
To subscribe to an event, do:
```c#
// 1. Register the handler first
// 2. Subscribe

// e.g. To subscribe to the MESSAGE_CREATE event
var handler = (sender, data) => { /* do stuff */};

discordIPC.OnMessageCreate += handler;
await discordIPC.SubscribeAsync(new MessageCreate.Args() {
    channel_id = "<channel-id>"
});
```
where args is the `Args` subclass of one of the event classes, and `data` in the delegate is the corresponding `Data` subclass. Again, you don't need to define all properties in the arguments. Undefined properties default to null and are not sent to the IPC.

Some event subscriptions don't take any arguments, so their `Args` subclass is empty, but you still need to provide an instance to differentiate the overloads. All events return data, so there are no dummy `Data` subclasses.

## Unsubscribing
To unsubscribe from an event, do:
```c#
// 1. Unubscribe
// 2. De-register the handler

// e.g. To unsubscribe from the MESSAGE_CREATE event
await discordIPC.UnsubscribeAsync(new MessageCreate.Args() {
    channel_id = "<channel-id>"
});
discordIPC.OnMessageCreate -= handler;
```
where args is the `Args` subclass of one of the event classes, and `data` in the delegate is the corresponding `Data` subclass. Again, you don't need to define all properties in the arguments. Undefined properties default to null and are not sent to the IPC.

The arguments must match exactly to the arguments provided to the corresponding subscription call. A good way to do it is to cache the arguments object and reuse it when unsubscribing. So the following subsection describes the general usage.

## General usage
```c#
// A dedicated method may also be used instead of a lambda
var handler = (sender, data) => { /* do stuff */};
var args = new MessageCreate.Args() {
    channel_id = "<channel-id>"
};

discordIPC.OnMessageCreate += handler;
await discordIPC.SubscribeAsync(args);

// ...

await discordIPC.UnsubscribeAsync(args);
discordIPC.OnMessageCreate -= handler;
```

## What events can I subscribe to?
All of them! Not just the ones related to rich presence (a.k.a. activity).

Ok all except 2:
  - `READY`: because it's only fired once upon initialization. To handle this event, register a listener to `OnReady` before calling `Init`.
  - `ERROR`: because they're handled automatically internally in the form of `ErrorResponseException`.

If you don't know all the events, their semantics and other things, [here's the documentation](https://discord.com/developers/docs/topics/rpc#commands-and-events-rpc-events).

# Error responses
All `SendCommandAsync` overloads throw `ErrorResponseException` if the IPC returns an error reponse for a command. Usually this happens due to unexpected/wrong types, so it's not necessary to handle it.

However, two notable places this exception may be thrown are in response to the `AUTHORIZE` command, if the user denies authorization, which makes it necessary to handle the exception there, and the `AUTHENTICATE` command, which may return an error because of invalid token, so you may check it there too.

# Summary
Combining all the above information, the general flow is:
  1. Initialize
  2. (Optional) Authorize
  3. Authenticate
  4. Send commands
  5. Sub/unsub to events
  6. Dispose

> Disposing of the IPC Connection closes it, you only want to do this if you're finished receiving events

> You need to authenticate (authorize before authenticating if necessary) before you can use the IPC. More information [here](https://discord.com/developers/docs/topics/rpc#authenticating).

Here's a sample code using DiscordIPC:
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
    discordIPC.Init();
    
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