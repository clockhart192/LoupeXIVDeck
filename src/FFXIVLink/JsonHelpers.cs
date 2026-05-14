using Newtonsoft.Json;

namespace Loupedeck.LoupeXIVDeck
{
    internal static class JsonHelpers
    {
        public static string SerializeAnyObject(object o) => JsonConvert.SerializeObject(o);
        public static T DeserializeObject<T>(string s) => JsonConvert.DeserializeObject<T>(s)!;
    }
}
