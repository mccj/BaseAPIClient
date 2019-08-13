using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.JsonConverter
{
    public class ObjectOrArrayConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }
        private Type[] excludeTypes()
        {
            return new[] { typeof(string) };
        }
        private bool isArray(Type objectType)
        {
            if (objectType.IsArray)
                return true;
            if (excludeTypes().Contains(objectType))
                return false;
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(objectType))
                return true;
            return false;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject && isArray(objectType))
            {
                var _object = serializer.Deserialize<Newtonsoft.Json.Linq.JObject>(reader);
                var r = serializer.Deserialize(new System.IO.StringReader("[" + (_object.Count > 0 ? _object.ToString() : string.Empty) + "]"), objectType);
                return r;
            }
            else if (reader.TokenType == JsonToken.StartArray && !isArray(objectType))
            {
                var r = serializer.Deserialize(reader, objectType.MakeArrayType());
                return (r as IEnumerable<object>).FirstOrDefault();
            }
            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}