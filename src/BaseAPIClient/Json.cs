using Utilities.ExpandoObject2.Converters;
using Newtonsoft.Json;
using System;
using System.Linq;
namespace SDK.BaseAPI
{
    /// <summary>
    /// JsonResult格式化扩展
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 将 JavaScript 对象表示法 (JSON) 格式的数据转换为数据对象。
        /// </summary>
        /// <param name="value">要转换的 JSON 编码字符串。</param>
        /// <returns>已转换为数据对象的 JSON 编码数据。</returns>
        public static dynamic Decode(string value)
        {
            //var v = JsonConvert.DeserializeObject(value);
            //return DynamicDictionary.ToDynamic(v);
            return Decode<object>(value);
        }
        /// <summary>
        /// 将 JavaScript 对象表示法 (JSON) 格式的数据转换为指定的强类型数据列表。
        /// </summary>
        /// <typeparam name="T">要将 JSON 数据转换为的强类型列表的类型。</typeparam>
        /// <param name="value">要转换的 JSON 编码字符串。</param>
        /// 
        /// <returns>已转换为强类型列表的 JSON 编码数据。</returns>
        public static T Decode<T>(string value)
        {
            //return JsonConvert.DeserializeObject<T>(value, new InterfaceConverter(typeof(T)));
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value, new DynamicJsonConverter(), new ExpandoObject2Converter());
        }

        /// <summary>
        /// 将 JavaScript 对象表示法 (JSON) 格式的数据转换为指定类型的数据对象。
        /// </summary>
        /// <param name="value">要转换的 JSON 编码字符串。</param>
        /// <param name="targetType">应将 value 数据转换为的类型。</param>
        /// <returns>已转换为指定类型的 JSON 编码数据。</returns>
        public static object Decode(string value, Type targetType)
        {
            //return JsonConvert.DeserializeObject(value, targetType, new InterfaceConverter(targetType));
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, targetType, new DynamicJsonConverter(), new ExpandoObject2Converter());
        }
        /// <summary>
        /// 将数据对象转换为 JavaScript 对象表示法 (JSON) 格式的字符串。
        /// </summary>
        /// <param name="value">要转换的数据对象。</param>
        /// <returns>返回已转换为 JSON 格式的数据的字符串。</returns>
        public static string Encode(object value)
        {
            if (value is Newtonsoft.Json.Linq.JObject)
                return (value as Newtonsoft.Json.Linq.JObject).ToString(Formatting.None);
            return JsonConvert.SerializeObject(value);

            //return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            //return System.Web.Helpers.Json.Encode(value);
        }
        //class DynamicDictionary : System.Dynamic.DynamicObject
        //{
        //    private readonly System.Collections.Generic.IDictionary<string, object> _values = new System.Collections.Generic.Dictionary<string, object>();
        //    public DynamicDictionary() { }
        //    public DynamicDictionary(Newtonsoft.Json.Linq.JToken obj)
        //    {
        //        foreach (Newtonsoft.Json.Linq.JProperty item in obj)
        //        {
        //            if (item.Value is Newtonsoft.Json.Linq.JValue)
        //                _values.Add(item.Name, (item.Value as Newtonsoft.Json.Linq.JValue).Value);
        //            else if (item.Value is Newtonsoft.Json.Linq.JObject)
        //                _values.Add(item.Name, new DynamicDictionary(item.Value as Newtonsoft.Json.Linq.JObject));
        //            else if (item.Value is Newtonsoft.Json.Linq.JArray)
        //                _values.Add(item.Name, (item.Value as Newtonsoft.Json.Linq.JArray).Select(f => new DynamicDictionary(f)).ToArray());
        //        }
        //    }
        //    public override System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames()
        //    {
        //        return _values.Keys;
        //    }
        //    public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        //    {
        //        result = _values.ContainsKey(binder.Name) ? _values[binder.Name] : null;
        //        return true;
        //    }

        //    public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        //    {
        //        _values[binder.Name] = DynamicDictionary.ToDynamic(value);
        //        return true;
        //    }
        //    //public override bool TryConvert(System.Dynamic.ConvertBinder binder, out object result)
        //    //{
        //    //    result = "{\"sss\":\"dddd\"}";
        //    //    return true;
        //    //    //if (binder.Type == typeof(IEnumerable) || binder.Type == typeof(object[]))
        //    //    //{
        //    //    //    var ie = (IsArray)
        //    //    //        ? xml.Elements().Select(x => ToValue(x))
        //    //    //        : xml.Elements().Select(x => (dynamic)new KeyValuePair<string, object>(x.Name.LocalName, ToValue(x)));
        //    //    //    result = (binder.Type == typeof(object[])) ? ie.ToArray() : ie;
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    result = Deserialize(binder.Type);
        //    //    //}
        //    //    //return true;
        //    //    //return base.TryConvert(binder, out result);
        //    //}
        //    //public override bool TryBinaryOperation(System.Dynamic.BinaryOperationBinder binder, object arg, out object result)
        //    //{
        //    //    result = "{\"sss\":\"dddd\"}";
        //    //    return true;
        //    //    //return base.TryBinaryOperation(binder, arg, out result);
        //    //}
        //    //public override bool TryUnaryOperation(System.Dynamic.UnaryOperationBinder binder, out object result)
        //    //{
        //    //    result = "{\"sss\":\"dddd\"}";
        //    //    return true;
        //    //    //return base.TryUnaryOperation(binder, out result);
        //    //}
        //    //public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        //    //{
        //    //    result = "{\"sss\":\"dddd\"}";
        //    //    return true;
        //    //    //return base.TryInvokeMember(binder, args, out result);
        //    //}
        //    //public override bool TryInvoke(System.Dynamic.InvokeBinder binder, object[] args, out object result)
        //    //{
        //    //    result = "{\"sss\":\"dddd\"}";
        //    //    return true;
        //    //    //return base.TryInvoke(binder, args, out result);
        //    //}
        //    //public override bool TrySetIndex(System.Dynamic.SetIndexBinder binder, object[] indexes, object value)
        //    //{
        //    //    return base.TrySetIndex(binder, indexes, value);
        //    //}
        //    //public override bool TryGetIndex(System.Dynamic.GetIndexBinder binder, object[] indexes, out object result)
        //    //{
        //    //    return base.TryGetIndex(binder, indexes, out result);
        //    //}
        //    //public override bool TryDeleteIndex(System.Dynamic.DeleteIndexBinder binder, object[] indexes)
        //    //{
        //    //    return base.TryDeleteIndex(binder, indexes);
        //    //}
        //    //用户自定义从Number型到整型的显示转换 
        //    public static explicit operator string(DynamicDictionary dyn)
        //    {
        //        return dyn.ToString();
        //    }
        //    public static explicit operator DynamicDictionary(string str)
        //    {
        //        var v = JsonConvert.DeserializeObject(str);
        //        return DynamicDictionary.ToDynamic(v);
        //    }
        //    public override string ToString()
        //    {
        //        return Json.Encode(this);
        //    }
        //    public static dynamic ToDynamic(object obj)
        //    {
        //        if (obj is Newtonsoft.Json.Linq.JObject)
        //            return new DynamicDictionary(obj as Newtonsoft.Json.Linq.JObject);
        //        else if (obj is Newtonsoft.Json.Linq.JArray)
        //            return (obj as Newtonsoft.Json.Linq.JArray).Select(f => new DynamicDictionary(f)).ToArray();
        //        else
        //        {
        //            return obj;
        //        }
        //    }


        //}

        //class InterfaceConverter : JsonConverter
        //{
        //    private System.Collections.Generic.Dictionary<Type, Type> dic = new System.Collections.Generic.Dictionary<Type, Type>();
        //    public InterfaceConverter(Type type)
        //    {
        //        // TODO: Complete member initialization
        //        var _type = type.IsArray ? type.GetElementType() : type;
        //        var ss = _type.GetCustomAttributes(typeof(InterfacePropertyAttribute), true);
        //        foreach (InterfacePropertyAttribute item in ss)
        //        {
        //            addTypeMap(item);

        //        }
        //        foreach (var item in _type.GetProperties())
        //        {
        //            var ss1 = item.GetCustomAttributes(typeof(InterfacePropertyAttribute), true);
        //            foreach (InterfacePropertyAttribute item2 in ss1)
        //            {
        //                addTypeMap(item2, item.PropertyType);
        //            }
        //        }
        //    }
        //    private void addTypeMap(InterfacePropertyAttribute item, Type interfaceType = null)
        //    {
        //        if (item == null || item.PropertyType == null) return;
        //        if (item.InterfaceType == null && interfaceType == null) return;
        //        var _interfaceType = item.InterfaceType == null ? interfaceType : item.InterfaceType;
        //        if (!dic.ContainsKey(_interfaceType))
        //        {
        //            dic.Add(_interfaceType, item.PropertyType);
        //        }
        //    }
        //    public override bool CanConvert(Type objectType)
        //    {
        //        if (objectType == null)
        //        {
        //            throw new ArgumentNullException("objectType");
        //        }
        //        bool nullable = (objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Nullable<>)));
        //        Type t = (nullable) ? Nullable.GetUnderlyingType(objectType) : objectType;
        //        if (t.IsAbstract || t.IsInterface || t == typeof(object))
        //            return true;
        //        else
        //            return false;
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        if (objectType == null)
        //        {
        //            throw new ArgumentNullException("objectType");
        //        }
        //        bool nullable = (objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Nullable<>)));
        //        Type t = (nullable) ? Nullable.GetUnderlyingType(objectType) : objectType;

        //        if (t.IsAbstract || t.IsInterface)
        //        {
        //            var ss = dic.Where(f => f.Key == t).Select(f => f.Value).FirstOrDefault();
        //            if (ss != null)
        //            {
        //                var value = Activator.CreateInstance(ss, true);
        //                serializer.Populate(reader, value);
        //                return value;
        //            }
        //        }
        //        else if (t == typeof(object))
        //        {
        //            //var value = new Newtonsoft.Json.Linq.JObject();
        //            var value = serializer.Deserialize(reader);
        //            return DynamicDictionary.ToDynamic(value);
        //        }
        //        return null;

        //        //if (t == typeof(System.DateTimeOffset))
        //        //{
        //        //    if (reader.Value is long)
        //        //        return new System.DateTimeOffset(new System.DateTime(Convert.ToInt64(reader.Value)));
        //        //    else if (reader.Value is string)
        //        //    {
        //        //        string dateText = reader.Value as string;
        //        //        if (!string.IsNullOrEmpty(this.DateTimeOffsetFormat))
        //        //            return DateTimeOffset.ParseExact(dateText, this.DateTimeOffsetFormat, this.Culture, this.DateTimeStyles);
        //        //        else
        //        //            return DateTimeOffset.Parse(dateText, Culture, this.DateTimeStyles);
        //        //    }
        //        //}
        //        //else if (t == typeof(System.DateTime))
        //        //{
        //        //    if (reader.Value is long)
        //        //        return new System.DateTime(Convert.ToInt64(reader.Value));
        //        //    else if (reader.Value is string)
        //        //    {
        //        //        string dateText = reader.Value as string;
        //        //        if (!string.IsNullOrEmpty(this.DateTimeFormat))
        //        //            return DateTime.ParseExact(dateText, this.DateTimeFormat, this.Culture, this.DateTimeStyles);
        //        //        else
        //        //            return DateTime.Parse(dateText, Culture, this.DateTimeStyles);
        //        //    }
        //        //}
        //        //else if (t == typeof(System.TimeSpan))
        //        //{
        //        //    if (reader.Value is long)
        //        //        return new System.TimeSpan(Convert.ToInt64(reader.Value));
        //        //    else if (reader.Value is string)
        //        //    {
        //        //        string dateText = reader.Value as string;
        //        //        if (!string.IsNullOrEmpty(this.TimeSpanFormat))
        //        //            return TimeSpan.ParseExact(dateText, this.TimeSpanFormat, this.Culture);
        //        //        else
        //        //            return TimeSpan.Parse(dateText, Culture);
        //        //    }
        //        //}
        //        //return null;
        //    }

        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //    }
        //}
    }
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    //public class InterfacePropertyAttribute : Attribute
    //{
    //    public InterfacePropertyAttribute(Type propertyType)
    //    {
    //        PropertyType = propertyType;
    //    }
    //    public InterfacePropertyAttribute(Type interfaceType, Type propertyType)
    //    {
    //        PropertyType = propertyType;
    //        InterfaceType = interfaceType;
    //    }
    //    public Type InterfaceType { get; set; }
    //    public Type PropertyType { get; set; }
    //}
}
