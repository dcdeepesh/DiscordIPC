# DiscordIPC
DiscordIPC is a wrapper for Discord's IPC-based RPC. It supports all commands and events unlike the other wrappers that support only activity-related commands and events.

> **WARNING!: Documentation is WIP. This warning will be removed once it's completed.**

## Before you use it
Because Discord's RPC is still under private beta, there are many inconsistencies, errors and outdated information in the documentation. See [problems and changes](#problems-and-changes) for more.

# Table of contents
  - [Adding to your project](#adding-to-your-project)
  - [Usage](#usage)
  - [Problems and changes](#problems-and-changes)
    - [Solving these problems](#solving-these-problems)
  - [Queries and support](#queries-and-support)

# Adding to your project
DiscordIPC can be installed as (NuGet package).

# Usage
Here is the general usage:
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
Make sure to include the `rpc` scope when authorizing your app.

See the [complete usage documentation](https://github.com/dcdeepesh/DiscordIPC/blob/master/Documentation/Usage.md) for more information.

# Problems and changes
The RPC hasn't been officially released by Discord yet, which is why DiscordIPC uses the IPC directly instead of the upcoming RPC, which poses problems of its own. Because it's still unofficial, Discord's documention about RPC isn't complete, and some of the existing documentation is also outdated. This means any changes in the implementation are not guaranteed to be documented, up until it's finally released. It also means some things in DiscordIPC may break every now and then. Needless to say, it's not wise to use it in production, the best choice is to wait for the RPC to release (if it will ever be released in the first place).

That being said, don't get the impression that DiscordIPC can't be reliably used at all. For the most part, these changes aren't breaking, the fundamental flow of IPC remains the same. So you can use it to build your own application and it will work well. **But still think of it as testing something in it's alpha stage, not knowing what may break tomorrow.**

## Solving these problems
To see how to solve these problems, and information about other issues, see [how to extend LowLevedDiscordIPC yourself](https://github.com/dcdeepesh/DiscordIPC/blob/master/Documentation/Extending.md).

# Queries and support
TODO