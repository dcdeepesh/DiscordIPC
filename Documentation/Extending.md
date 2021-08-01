# Creating your own implementation

> **WARNING: As of right now, DiscordIPC does _not_ support custom implementations, this documentation is meant to be used once that feature is added. This warning will be removed when that happens.**


If you've seen the [problems with DiscordIPC](https://github.com/dcdeepesh/DiscordIPC#problems-and-changes), and any such software/library, and want to create your own implementation by extending `LowLevelDiscordIPC` and handling the raw JSON returned by IPC, this is the guide for you.

DiscordIPC exposes some internal mechanism for the application developer to use themselves in case these problems occur. Most problems are type related, i.e. the data returned by the IPC may not match the expected type (some properties may be unmatched).

If you look carefully, you'll see that `DiscordIPC` extends `LowLevelDiscordIPC` and just provides overloads for different types. In case these types don't match the return types, you can extend `LowLevelDiscordIPC` and implement those methods yourself. Internally DiscordIPC works with `dynamic` data types and adds necessary casts and conversions when interfacing with user code. These methods are the "weak type" methods (e.g. `SendCommandWeakTypeAsync`); Implementing these methods yourself means you'll be dealing with `dynamic` data types and it's your responsibility to add correct casts wherever necessary. Failure to do so will result in runtime errors. `DiscordIPC` was provided to move these errors from runtime to compile-time, as it adds the necessary casts internally. So by creating your own implementation, you're letting go of the security provided by these overloads.

# Getting your hands dirty
Discord's IPC allows user code to do two things - send commands and receive the response, and handle events. We'll see how to handle the commands first, and then the events.

When you're creating your own implementation, make sure to add
```c#
using Dec.DiscordIPC.Core;
```
in your code, as this namespace contains all the core components.

## Commands


## Events
(Not supported yet)