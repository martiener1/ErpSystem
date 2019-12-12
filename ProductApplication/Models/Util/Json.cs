using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Util
{
    public static class Json
    {

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.DeserializeObject<T>(json, dateTimeConverter);
        }
    }
}