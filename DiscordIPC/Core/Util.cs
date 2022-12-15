using System.Text.Json;

namespace Dec.DiscordIPC.Core; 

internal static class Extensions {
    public static T ToObject<T>(this JsonElement element) =>
        JsonSerializer.Deserialize<T>(element.GetRawText());
}