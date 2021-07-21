using System;
using System.Text.Json;

namespace Dec.DiscordIPC {
    internal static class Extensions {
        public static T ToObject<T>(this JsonElement element) =>
            JsonSerializer.Deserialize<T>(element.GetRawText());
    }

    internal class Util {
        public static void Log(string msg) => Console.WriteLine(msg);
        public static void Log(string format, params object[] arg) =>
            Console.WriteLine(format, arg);
    }
}
