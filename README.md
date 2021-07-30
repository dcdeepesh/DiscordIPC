# DiscordIPC
DiscordIPC is a wrapper for Discord's IPC-based RPC. It supports all commands and events unlike the other wrappers that support only activity-related commands and events.

> **WARNING!: Documentation is WIP. This warning will be removed once it's completed.**

## Before you use it
Because Discord's RPC is still under private beta, there are many inconsistencies, errors and outdated information in the documentation. See [problems and changes](#problems-and-changes) for more.

# Installation
DiscordIPC can be installed as (NuGet package).

# Usage
Here is the base usage:
```c#
using Dec.DiscordIPC;

namespace Foo {
    class Program {
        static void Main(string[] args) {
            DiscordIPC discordIPC = new DiscordIPC("YOUR-CLIENT-ID");
            discordIPC.InitAsync();
            
            // do stuff

            discordIPC.Dispose();
        }
    }
}
```
Make sure to include the `rpc` scope when authorizing your app.

See the (complete usage documentation) for more information.

# Problems and changes
The RPC hasn't been officially released by Discord yet, which is why DiscordIPC uses the IPC directly instead of the upcoming RPC, which poses problems of its own. Because it's still unofficial, Discord's documention about RPC isn't complete, and some of the existing documentation is also outdated. This means any changes in the implementation are not guaranteed to be documented, up until it's finally released. It also means some things in DiscordIPC may break every now and then. Needless to say, it's not wise to use it in production, the best choice is to wait for the RPC to release (if it will ever be released in the first place).

That being said, don't get the impression that DiscordIPC can't be reliably used at all. For the most part, these changes aren't breaking, the fundamental flow of IPC remains the same. So you can use it to build your own application and it will work well. **But still think of it as testing something in it's alpha stage, not knowing what may break tomorrow.**

## Solving these problems
> Warning: Technical section

Given these changes and problems, it doesn't mean they can't be fixed. DiscordIPC exposes some internal mechanism for the application developer to use themselves in case these problems occur. Most problems are type related, i.e. the data returned by the IPC may not match the expected type (some properties may be unmatched).

If you look carefully, you'll see that `DiscordIPC` extends `LowLevelDiscordIPC` and just provides overloads for different types. In case these types don't match the return types, you can extend `LowLevelDiscordIPC` and implement those methods yourself. Internally DiscordIPC works with `dynamic` data types and adds necessary casts and conversions when interfacing with user code. These methods are the "weak type" methods (e.g. `SendCommandWeakTypeAsync`); Implementing these methods yourself means you'll be dealing with `dynamic` data types and it's your responsibility to add correct casts wherever necessary. Failure to do so will result in runtime exceptions. `DiscordIPC` was provided to move these exception from runtime to compile-time, as it adds the necessary casts internally, so you're basically letting go of the security provided by it by implementing your own `LowLevelDiscordIPC`.