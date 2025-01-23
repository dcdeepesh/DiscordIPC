# DiscordIPC

[![Nuget badge](https://img.shields.io/nuget/v/Dec.DiscordIPC)](https://www.nuget.org/packages/Dec.DiscordIPC/)

---

## !! Discontinuation Notice !!

**TL;DR:** No more active development; obstrusive issues will, however, be triaged;
DiscordIPC will be rewritten from scratch in Rust, with cross-platform support and a native library export;
then C# bindings will/may be implemented; any/all/some of this may happen in this repo;
everything will (hopefully) be stabilized by mid-2025; this note will stay updated accordingly.

**Long version:** This library was created during the development of [Doge](https://github.com/dcdeepesh/Doge) and was
initially only meant to be a backend for it. It eventually evolved into a standalone library and I started to like the
concept of it. A lot has changed since then. Most notably, I've switched to Linux full-time and have grown to not
particulary like some aspects of Discord and .NET. This project, however, along with Doge, are tightly bound to Windows.
Changing either requires changing the other. Between the dilemma of how to begin refactoring these projects and my personal
life not being by my side, no practical progress was made. The master branch contains some commits for the planned revamp
of the project which never saw the light of day. Neither did Doge. Skipping to present, I'm now trying to revive everything
I've put off for so long, and bring everything back on track.

DiscordIPC (and Doge) will be improved to become full-fledged software systems. DiscordIPC will be rewritten in Rust,
will be made cross-platform, and thus will provide native libraries as the build output so any language can comfortably use it.
In accordance, Doge will be rewritten, possibly using Tauri (yes, I know, web on desktop, but it won't be a lousy piece of software),
thus also becoming cross-platform. The documentation will be made even more comprehenive, and complete design documents will be made.
I still haven't decided how to manage repositories among all this transience. Most likely, new repositories will be created for
DiscordIPC and it's language binding(s), and this repository will be archived. Until then, this repository stays un-archived but stale.
Should Discord's internal implementation of the IPC change not-too-drastically and cause issues for the current DiscordIPC,
I'll make appropriate changes. No other updates will be provided. I consider it wasted work as the future DiscordIPC for .NET
will just be a set of language bindings around the new DiscordIPC native library.

Despite my diminishing usage of Discord, I've loved working on these projects and the response I've gotten, and that is why they'll
be revived as soon as feasible. I haven't had a good track record of announcing expected dates (try blaming this readme),
but all of the aforementioned advancements are expected to start getting stabilized around mid-2025, unless life takes another
huge U-turn. Thus, DiscordIPC is not being discontinued, just the current model of it. The project will be reincarnated with
a better model moving forward. As any progress is made, this note will be updated.

> **A note on branches:**
> The branch `v1` contains the "stable" version of the library. Future fixes, if any, will continue on that branch.
> The branch `master` contains commits for the planned revamp that never happened. I considered re-branching,
> but decided otherwise to not mess up any clones.
> Had I re-branched, I would've renamed `v1 -> master` and `master -> revamp`, with `v1 -> master` being the default branch.

End of notice.

---

DiscordIPC is a wrapper for Discord's IPC-based RPC. It supports all commands and events unlike the other wrappers that support only presence related commands and events.

## Before you use it
Because Discord's RPC is still under private beta, there are many inconsistencies, errors and outdated information in the documentation. See [problems and changes](#problems-and-changes) for more.

## Table of contents
  - [Adding to your project](#adding-to-your-project)
  - [Usage](#usage)
  - [Problems and changes](#problems-and-changes)
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
        
        static async Task Main(string[] args) {
            DiscordIpcClient client = new DiscordIpcClient(CLIENT_ID);
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
The RPC hasn't been officially released by Discord yet, which is why DiscordIPC uses the IPC directly, which poses problems of its own. Because it's still unofficial, Discord's documentation about RPC isn't complete, and at times outdated. This means any changes in the implementation are not guaranteed to be documented. It also means that some things in DiscordIPC may break every now and then.

That being said, don't get the impression that DiscordIPC can't be reliably used at all. For the most part, these changes aren't breaking, the fundamental flow of IPC remains the same. So you can use it to build your own applications and it will work seamlessly. But still **think of it as testing something in it's beta stage, where things may break out of nowhere.**

## Updates and contributing
DiscordIPC is certainly not in a mature state and will never be, because of the dynamic nature of Discord RPC. You may encounter some bugs and may have improvements in your mind. Feel free to suggest them as issues, contribute in the form of pull requests, or event DM me your suggestions directly.

There are no strict requirements for pull requests and contributions. Just keep your commits restricted to a single fundamental change and commit messages clean.

## Queries and contact
If there's anything that you don't understand from the documentation, or want to ask anything else about DiscordIPC directly to me, you can add and DM me on Discord (`Krove#5477`).
