﻿using Newtonsoft.Json;

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

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}