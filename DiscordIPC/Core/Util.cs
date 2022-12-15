using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dec.DiscordIPC.Core; 

internal static class Extensions {
    public static T ToObject<T>(this JsonElement element) =>
        Json.Deserialize<T>(element.GetRawText());
        
    public static object ToObject(this JsonElement element, Type type) =>
        Json.Deserialize(element.GetRawText(), type);

    public static bool IsErrorResponse(this IpcPayload element) {
        return element.evt == "ERROR";
    }
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

internal class Json {
    public static T Deserialize<T>(string json) {
        return JsonSerializer.Deserialize<T>(json);
    }
        
    public static object Deserialize(string json, Type type) {
        return JsonSerializer.Deserialize(json, type);
    }

    public static byte[] SerializeToBytes<T>(T obj) {
        return JsonSerializer.SerializeToUtf8Bytes(obj, new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}