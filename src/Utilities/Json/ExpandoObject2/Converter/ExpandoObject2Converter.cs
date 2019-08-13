using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

namespace Utilities.ExpandoObject2.Converters
{
    /// <summary>
    /// Converts an ExpandoObject2 to and from JSON.
    /// </summary>
    public class ExpandoObject2Converter : Newtonsoft.Json.JsonConverter
    {
        public ExpandoObject2Converter() { }
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // can write is set to false
        }
        #region Read
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadValue(reader);
        }

        private object ReadValue(JsonReader reader)
        {
            if (!reader.MoveToContent())
            {
                throw JsonSerializationException2.Create(reader, "Unexpected end when reading ExpandoObject2.");
            }

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ReadObject(reader);
                case JsonToken.StartArray:
                    return ReadList(reader);
                default:
                    if (IsPrimitiveToken(reader.TokenType))
                    {
                        return reader.Value;
                    }

                    throw JsonSerializationException2.Create(reader, "Unexpected token when converting ExpandoObject2: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }
        }

        private object ReadList(JsonReader reader)
        {
            IList<object> list = new List<object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    default:
                        object v = ReadValue(reader);
                        list.Add(v);
                        break;
                    case JsonToken.EndArray:
                        {
                            //return list;
                            var types = list.Select(f => f.GetType()).Distinct().ToArray();
                            if (types.Length == 1)
                            {
                                var type = types.Single();
                                var ofTypeMethod = typeof(ExpandoObject2Converter).GetMethod(nameof(ExpandoObject2Converter.objectToT), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(type);
                                var value = ofTypeMethod.Invoke(this, new object[] { list });
                                return value;
                                //return objectToT<string>(list);
                            }
                            else
                            {
                                return list.ToArray();
                            }
                        }
                }
            }

            throw JsonSerializationException2.Create(reader, "Unexpected end when reading ExpandoObject2.");
        }
        private T[] objectToT<T>(IList<object> list)
        {
            return list.OfType<T>().ToArray();
        }
        private object ReadObject(JsonReader reader)
        {
            dynamic expandoObject = new System.Dynamic.ExpandoObject2();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        string propertyName = reader.Value.ToString();

                        if (!reader.Read())
                        {
                            throw JsonSerializationException2.Create(reader, "Unexpected end when reading ExpandoObject2.");
                        }

                        object v = ReadValue(reader);

                        expandoObject[propertyName] = v;
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        return expandoObject;
                }
            }
            throw JsonSerializationException2.Create(reader, "Unexpected end when reading ExpandoObject2.");
        }
        internal static bool IsPrimitiveToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return true;
                default:
                    return false;
            }
        }
        #endregion Read

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(System.Dynamic.ExpandoObject2));
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter"/> can write JSON.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this <see cref="JsonConverter"/> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite
        {
            get { return false; }
        }
    }
    static class ExpandoObject2ConverterExtensions
    {
        private static BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public static bool MoveToContent(this JsonReader reader)
        {
            var member = reader.GetType().GetMethod(nameof(MoveToContent), bindingFlags);
            return (bool)member.Invoke(reader, new object[] { });
        }
        public static string FormatWith(this string format, IFormatProvider provider, object arg0)
        {
            return format.FormatWith(provider, new[] { arg0 });
        }
        public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
        {
            return format.FormatWith(provider, new[] { arg0, arg1 });
        }
        public static bool EndsWith(this string source, char value)
        {
            return (source.Length > 0 && source[source.Length - 1] == value);
        }
    }
    class JsonSerializationException2
    {
        internal static JsonSerializationException Create(JsonReader reader, string message)
        {
            return Create(reader, message, null);
        }
        internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
        {
            return Create(reader as IJsonLineInfo, reader.Path, message, ex);
        }
        internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
        {
            message = FormatMessage(lineInfo, path, message);

            return new JsonSerializationException(message, ex);
        }
        internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
        {
            // don't add a fullstop and space when message ends with a new line
            if (!message.EndsWith(System.Environment.NewLine, StringComparison.Ordinal))
            {
                message = message.Trim();

                if (!message.EndsWith('.'))
                {
                    message += ".";
                }

                message += " ";
            }

            message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);

            if (lineInfo != null && lineInfo.HasLineInfo())
            {
                message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
            }

            message += ".";

            return message;
        }

    }
}