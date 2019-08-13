using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
namespace Utilities.ExpandoObject2.Converters
{
    public class DynamicJsonConverter : Newtonsoft.Json.JsonConverter
    {
        //private EmitDynamic.DynamicFactory dynamicFactory = new EmitDynamic.DynamicFactory();

        public DynamicJsonConverter()
        {
            //dynamicFactory.PropertyResolver = (type, interfaces, interceptor) =>
            //{
            //    if (interceptor is Newtonsoft.Json.Linq.JObject)
            //        return (interceptor as Newtonsoft.Json.Linq.JObject).Properties().Select(f =>
            //        new EmitDynamic.DynamicFactory.PropertyInfo2()
            //        {
            //            CanRead = true,
            //            Name = f.Name,
            //            PropertyType = ((Newtonsoft.Json.Linq.JValue)f.Value)?.Value?.GetType(),
            //            //GetMethod = 
            //        }).ToArray();
            //    else
            //        return new EmitDynamic.DynamicFactory.PropertyInfo2[] { };
            //};
            //dynamicFactory.CreateInstanceResolver = (proxyType, interceptor) =>
            //{
            //    var _interceptor = interceptor as Newtonsoft.Json.Linq.JObject;
            //    var obj = Activator.CreateInstance(proxyType, null);
            //    foreach (var item in proxyType.GetProperties().Where(f => f.CanWrite))
            //    {
            //        var _value = (_interceptor[item.Name] as Newtonsoft.Json.Linq.JValue)?.Value;
            //        item.SetValue(obj, _value);
            //    }
            //    return obj;
            //};
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //var value = serializer.Deserialize(reader);

            //return dynamicFactory.CreateProxy(value, true);
            ////return DynamicJsonObject.ToDynamic(value);
            ////return value;
            try
            {
                if (reader.Value != null)
                    return reader.Value;
                if (reader.TokenType == JsonToken.StartArray)
                {
                    return serializer.Deserialize<System.Dynamic.ExpandoObject2[]>(reader);
                }
                else
                {
                    return serializer.Deserialize<System.Dynamic.ExpandoObject2>(reader);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
