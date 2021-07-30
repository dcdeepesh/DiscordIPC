# Usage
This article/page/whatever explains how to use DiscordIPC. If you're lazy or just want a quick summary, head straight to the [summary](#summary).

# Table of contents
  - [Init/Dispose](#initdispose)
    - [Init](#init)
    - [Dispose](#dispose)
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
The first you do is instantiate `DiscordIPC`, providing it your client ID.
```c#
DiscordIPC discordIPC = new DiscordIPC("<YOUR-CLIENT-ID>");
```

The constructor also takes another (boolean) argument `verbose`, which defaults to `false`. Settings it to true will tell DiscordIPC to log every json sent and received.

This call just stores these two values internally, no substantial task is done. It's all done in the following call:
```c#
discordIPC.InitAsync();
```

This does everything like connecting to the pipe etc. Now you're ready to do the real stuff.

> As of right now, DiscordIPC connects only to `discord-ipc-0` if it can, and doesn't check/connect to other pipes. So you can't connect to another pipe if you have two discord clients running (e.g. Canary).

> If you want to receive the payload sent in the `READY` event, register a handler to the `OnReady` event before calling `InitAsync`. More [here](#what-events-can-i-subscribe-to).

## Dispose
Okay before you see how to actually use it, remember when you're done using DiscordIPC, dispose the instance.
```c#
discordIPC.Dispose();
```

# Commands
All commands are defined in the `Dec.DiscordIPC.Commands` namespace. Each command has a class, a subclass `Args` which defines the arguments to provide when sending that command, and a subclass `Data` which defines the data returned in response to that command.

## Sending commands
To send a command, do:
```c#
var responseData = discordIPC.SendCommandAsync(/* args */);

// e.g. To send the GET_GUILD command
GetGuild.Data data = discordIPC.SendCommandAsync(new GetGuild.Args() {
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
discordIPC.SubscribeAsync(new MessageCreate.Args() {
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
discordIPC.UnsubscribeAsync(new MessageCreate.Args() {
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
discordIPC.SubscribeAsync(args);

// ...

discordIPC.UnsubscribeAsync(args);
discordIPC.OnMessageCreate -= handler;
```

## What events can I subscribe to?
All of them! Not just the ones related to rich presence (a.k.a. activity).

Ok all except 2:
  - `READY`: because it's only fired once upon initialization. To handle this event, register a listener to `OnReady` before calling `InitAsync`.
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

> You need to authenticate (authorize before authenticating if necessary) before you can use the IPC. More information [here](https://discord.com/developers/docs/topics/rpc#authenticating).

Here's a sample code using DiscordIPC:
```c#
using Dec.DiscordIPC;
using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Events;

namespace Example {
    class Program {
        private static readonly string CLIENT_ID = "<CLIENT-ID>";
        static async Task Main() {
            DiscordIPC discordIPC = new DiscordIPC(CLIENT_ID));
            discordIPC.InitAsync();

            // Authorize
            string accessToken = "";
            try {
                discordIPC.Authorize(new Authorize.Args() {
                    scopes = new List<string>() { "rpc" },
                    client_id = CLIENT_ID
                });
            } catch (ErrorResponseException e) {
                Console.Log("User denied authorization");
                return;
            }

            // Authenticate (ignoring the response here)
            discordIPC.Authenticate(new Authenticate.Args() {
                access_token = accessToken
            });

            // Subscribe to an event
            var handler = (sender, data) => Console.Log("New message!");
            var args = new MessageCreate.Args() {
                channel_id = "<some-text-channel-id>"
            };
            discordIPC.OnMessageCreate += handler;
            discordIPC.SubscribeAsync(args);

            // Use commands
            GetChannel.Data response = discordIPC.SendCommandAsync(new GetChannel.Args() {
                channel_id = "<some-channel-id>"
            });
            Console.Log(response.name);

            // ... (do random stuff)

            // Unsubscribe from the event
            discordIPC.UnsubscribeAsync(args);
            discordIPC.OnMessageCreate -= handler;

            // Dispose
            discordIPC.Dispose();
        }
    }
}
```