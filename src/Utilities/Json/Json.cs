using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class Json
    {
        public static string Encode(object value, bool isIndented = true)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, isIndented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }
        public static object Decode(string value)
        {
            return Decode<object>(value);
        }
        public static object Decode(string value, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, type, new ExpandoObject2.Converters.DynamicJsonConverter(), new ExpandoObject2.Converters.ExpandoObject2Converter());
        }
        public static T Decode<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value, new ExpandoObject2.Converters.DynamicJsonConverter(), new ExpandoObject2.Converters.ExpandoObject2Converter());
        }
    }
}