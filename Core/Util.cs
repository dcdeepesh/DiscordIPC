using System;
using System.Text.Json;

namespace Dec.DiscordIPC {
    internal static class Extensions {
        public static T ToObject<T>(this JsonElement element) =>
            JsonSerializer.Deserialize<T>(element.GetRawText());
    }

    internal class Util {
        public static bool Verbose { get; set; }
        
        public static void Log(string msg) {
            if (Verbose)
                Console.WriteLine(msg);
        }

        public static void Log(string format, params object[] arg) {
            if (Verbose)
                Console.WriteLine(format, arg);
        }
    }
}
