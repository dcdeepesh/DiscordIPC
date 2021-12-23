# Usage
This is a complete guide on how to use DiscordIPC. [Here's a quick summary](#summary).

## Table of contents
  - [Init/Dispose](#initdispose)
    - [Init](#init)
    - [Dispose](#dispose)
  - [Commands](#commands)
    - [Sending commands](#sending-commands)
    - [Which commands can be sent?](#which-commands-can-be-sent)
  - [Events](#events)
    - [Subscribing](#subscribing)
    - [Unsubscribing](#unsubscribing)
    - [General usage](#general-usage)
    - [What events can I subscribe to?](#what-events-can-i-subscribe-to)
  - [Error responses](#error-responses)
  - [Summary](#summary)

## Init/Dispose

### Init
The first thing you do is instantiate `DiscordIPC`, providing it your client ID.
```c#
DiscordIPC client = new DiscordIPC("<YOUR-CLIENT-ID>");
```

The constructor also takes another boolean argument `verbose`, which defaults to `false`. If set to true, DiscordIPC will log every JSON sent and received.

This call just stores these two values internally, no substantial task is done. It's all done in the following call:
```c#
await client.InitAsync();
```

This attempts to connect to the pipe named `discord-ipc-0`. If you want to connect to a specific pipe, give its number as an argument. e.g. `client.InitAsync(1)` will make a connection attempt to `discord-ipc-1` and so on. If it cannot connect to the pipe within 2 seconds, the method throws an IOException.

If you want to receive the payload sent in the `READY` event, register a handler to the `OnReady` event before calling `InitAsync()`. More [here](#what-events-can-i-subscribe-to). Now you're ready to use the IPC.

### Dispose
Before you see how to actually use it, remember when you're done using it, dispose the instance.
```c#
client.Dispose();
```

## Commands
All commands are defined in the `Dec.DiscordIPC.Commands` namespace. Each command has a class, a subclass `Args` which defines the arguments to provide when sending that command, and a subclass `Data` which defines the data returned in response to that command.

### Sending commands
To send a command, do:
```c#
X.Args args = new X.Args() { /*...*/ };
X.Data data = await client.SendCommandAsync(args);
// where X is a class in the Dec.DiscordIPC.Commands namespace.
```
For example, to send a `GET_GUILD` command:
```c#
GetGuild.Data data = await client.SendCommandAsync(new GetGuild.Args() {
    guild_id = "<guild-id>"
});
```
where args is the `Args` subclass of one of the command classes, and `data` is the corresponding `Data` subclass. As shown in the example above, you don't need to define all properties in the arguments. Undefined properties default to null and are not sent to the IPC.

Some commands don't take any arguments, so their `Args` subclass is empty, but you still need to provide an instance to differentiate the overloads. Likewise, some commands don't return any data, so their `SendCommandAsync` overloads return nothing.

### Which commands can be sent?
All of them! Not just the ones related to rich presence (a.k.a. activity).

All except 3:
  - `DISPATCH`: because it's not meant to be sent from the client.
  - `SUBSCRIBE`, `UNSUBSCRIBE`: because they're used indirectly by calling other methods of `DiscordIPC`. See [Events](#events) for more.

[Here's Discord's documentation](https://discord.com/developers/docs/topics/rpc#commands-and-events-rpc-commands) of all commands and semantics.

## Events
All events are defined in the `Dec.DiscordIPC.Events` namespace. Each event has a class, a subclass `Args` which defines the arguments to provide when subscribing to it, and a subclass `Data` which is provided to the event handler.

DiscordIPC uses C# events to notify of Discord's events. When you're done with an event make sure you unsubscribe from it and de-register the event handler.

### Subscribing
To subscribe to an event, do:
```c#
X.Args args = new X.Args() {/*...*/};
EventHandler<X.Data> handler = (sender, data) => { /*...*/};
// 1. Register the handler
// 2. Subscribe
client.OnX += handler;
await client.SubscribeAsync(args);
// where X is a class in the Dec.DiscordIPC.Events namespace.
```
For example, to subscribe to the `MESSAGE_CREATE` event
```c#
EventHandler<MessageCreate.Data> handler =
  (sender, data) => Console.WriteLine("New message!");
client.OnMessageCreate += handler;
await client.SubscribeAsync(new MessageCreate.Args() {
    channel_id = "<channel-id>"
});
```
where args is the `Args` subclass of one of the event classes, and `data` in the event handler is the corresponding `Data` subclass. You don't need to define all the properties in the arguments. Undefined properties default to null and are not sent to the IPC.

Some event subscriptions don't take any arguments, so their `Args` subclass is empty, but you still need to provide an instance to differentiate the overloads. All events return data, so there are no dummy `Data` subclasses.

### Unsubscribing
To unsubscribe from an event, do:
```c#
// 1. Unubscribe
// 2. De-register the handler
await client.UnsubscribeAsync(args);
client.OnX -= handler;
```
For example, to unsubscribe from the previous `MESSAGE_CREATE` event:
```c#
await client.UnsubscribeAsync(args);
client.OnMessageCreate -= handler;
```
where args are the arguments previously passed to `SubscribeAsync()`.

**Note:** Because of how C# events work, the arguments must match exactly with the arguments provided to the corresponding subscription call. A good way to do it is to cache the arguments object when subscribing, and reuse it when unsubscribing, as done in this guide.

### General usage
With all of that in mind, here's the general usage of events:
```c#
EventHandler<X.Data> handler = (sender, data) => { /*...*/ };
X.Args args = new X.Args() { /*...*/ };

client.OnX += handler;
await client.SubscribeAsync(args);

// ...

await client.UnsubscribeAsync(args);
client.OnX -= handler;
```

### What events can I subscribe to?
All of them! Not just the ones related to rich presence (a.k.a. activity).

All except 2:
  - `READY`: because it's only fired once upon initialization. To handle this event, register a listener to `OnReady` before calling `InitAsync()`.
  - `ERROR`: because it's handled in the form of `ErrorResponseException`.

[Here's Discord's documentation](https://discord.com/developers/docs/topics/rpc#commands-and-events-rpc-events)  of all events and semantics.

## Error responses
All `SendCommandAsync()` overloads throw an `ErrorResponseException` if the IPC returns an error reponse for a command. Usually this happens due to unexpected/wrong types, so it's not necessary to handle it.

However, two notable places where this exception may be thrown are:
1. In response to the `AUTHORIZE` command, if the user denies authorization.
2. In response to the `AUTHENTICATE` command, if the provided token is invalid.

These are the two places where it may be necessary to handle this exception.

## Summary
Combining all that information, the general flow is:
  1. Initialize
  2. (Optional) Authorize
  3. Authenticate
  4. Send commands
  5. Subscribe/Unsubscribe to events
  6. Dispose

> **Tip:** Don't forget to authenticate, [here's why](https://discord.com/developers/docs/topics/rpc#authenticating).

Here's a sample code using DiscordIPC:
```c#
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dec.DiscordIPC;
using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Events;

namespace Example {
    class Program {
        private static readonly string CLIENT_ID = "<CLIENT-ID>";
        
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args) {
            DiscordIPC client = new DiscordIPC(CLIENT_ID);
            await client.InitAsync();

            // Authorize (if necessary)
            string accessToken;
            try {
                Authorize.Data codeResponse = await client.SendCommandAsync(
                    new Authorize.Args() {
                        scopes = new List<string>() { "rpc" },
                        client_id = CLIENT_ID
                    });
                accessToken = getAccessTokenFromAuthCode(codeResponse.code);
            } catch (ErrorResponseException) {
                Console.WriteLine("User denied authorization");
                return;
            }

            // Authenticate (ignoring the response here)
            await client.SendCommandAsync(new Authenticate.Args() {
                access_token = accessToken
            });

            // Subscribe to an event
            EventHandler<MessageCreate.Data> handler =
                (sender, data) => Console.WriteLine("New message!");
            var eventArgs = new MessageCreate.Args() {
                channel_id = "<some-text-channel-id>"
            };
            client.OnMessageCreate += handler;
            await client.SubscribeAsync(eventArgs);

            // Use commands
            GetChannel.Data response = await client.SendCommandAsync(
                new GetChannel.Args() { channel_id = "<some-channel-id>" });
            Console.WriteLine(response.name);

            // ...

            // Unsubscribe from the event
            await client.UnsubscribeAsync(eventArgs);
            client.OnMessageCreate -= handler;

            // Dispose
            client.Dispose();
        }
    }
}
```