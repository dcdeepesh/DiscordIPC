# DiscordIPC

[![Nuget badge](https://img.shields.io/nuget/v/Dec.DiscordIPC)](https://www.nuget.org/packages/Dec.DiscordIPC/)

DiscordIPC is a wrapper for Discord's IPC-based RPC. It supports all commands and events unlike the other wrappers that support only presence related commands and events.

## Before you use it
Because Discord's RPC is still under private beta, there are many inconsistencies, errors and outdated information in the documentation. See [problems and changes](#problems-and-changes) for more.

## Table of contents
  - [Adding to your project](#adding-to-your-project)
  - [Usage](#usage)
  - [Problems and changes](#problems-and-changes)
    - [Solving these problems](#solving-these-problems)
  - [Updates and contributing](#updates-and-contributing)
  - [Queries and contact](#queries-and-contact)

## Adding to your project
DiscordIPC can be added as a [NuGet package](https://www.nuget.org/packages/Dec.DiscordIPC/).

## Usage
Here is the general usage:
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

            // Authorize
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
Make sure to include the `rpc` scope when authorizing your app.

See the [complete usage guide](Documentation/Usage.md).

## Problems and changes
The RPC hasn't been officially released by Discord yet, which is why DiscordIPC uses the IPC directly, which poses problems of its own. Because it's still unofficial, Discord's documention about RPC isn't complete, and at times outdated. This means any changes in the implementation are not guaranteed to be documented. It also means that some things in DiscordIPC may break every now and then.

That being said, don't get the impression that DiscordIPC can't be reliably used at all. For the most part, these changes aren't breaking, the fundamental flow of IPC remains the same. So you can use it to build your own applications and it will work seamlessly. But still **think of it as testing something in it's beta stage, where things may break out of nowhere.**

## Updates and contributing
DiscordIPC is certainly not in a mature state and will never be, because of the dynamic nature of Discord RPC. You may encounter some bugs and may have improvements in your mind. Feel free to suggest them as issues, contribute in the form of pull requests, or event DM me your suggestions directly.

There are no strict requirements for pull requests and contributions. Just keep your commits restricted to a single fundamental change and commit messages clean.

## Queries and contact
If there's anything that you didn't understand from the documentation, or want to ask anything else about DiscordIPC directly to me, you can add and DM me on Discord (`Krove#7669`).