using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;
using SDK.BaseAPI;

namespace System.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static string GetResultContent(this HttpResponseMessage task)
        {
            return task.Content.ReadAsStringAsync().Result;
        }
        //public static T GetResultContent<T>(this Task<HttpResponseMessage> task, System.Net.Http.Formatting.MediaTypeFormatter[] mediaTypeFormatters)
        //{
        //    return task.Result.GetResultContent<T>(mediaTypeFormatters);

        //    //if (task.IsCompleted)
        //    //{
        //    //}
        //    //else
        //    //{
        //    //    //return task.Exception.Message;
        //    //    return default(T);
        //    //    //task.Exception;
        //    //}
        //}
        public static string GetResultContent(this Task<HttpResponseMessage> task)
        {
            return task.Result.GetResultContent();
        }
        ///// <summary>
        ///// 序列化
        ///// </summary>
        ///// <param name="formatter"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static string Serialize(this HttpContent httpContent)
        //{
        //    var ms = new System.IO.MemoryStream();
        //    var task = httpContent.WriteToStreamAsync(value.GetType(), value, ms, null, null);
        //    task.Wait();
        //    return formatter.SupportedEncodings.First().GetString(ms.ToArray());
        //}
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(this System.Net.Http.Formatting.MediaTypeFormatter formatter, object value)
        {
            if (value == null) return string.Empty;
            var ms = new System.IO.MemoryStream();
            var task = formatter.WriteToStreamAsync(value.GetType(), value, ms, null, null);
            task.Wait();
            return formatter.SupportedEncodings.First().GetString(ms.ToArray());
        }
        public static T Deserialize<T>(this System.Net.Http.Formatting.MediaTypeFormatter formatter, string value)
        {
            return (T)Deserialize(formatter, typeof(T), value);
        }
        public static object Deserialize(this System.Net.Http.Formatting.MediaTypeFormatter formatter, Type type, string value)
        {
            if (value == null) return null;
            var bytes = formatter.SupportedEncodings.First().GetBytes(value.Trim('\0'));
            var ms = new System.IO.MemoryStream(bytes);
            var task = formatter.ReadFromStreamAsync(type, ms, null, null);
            return task.Result;
        }
        public static T Deserialize<T>(this IEnumerable<System.Net.Http.Formatting.MediaTypeFormatter> formatters, string value)
        {
            return (T)Deserialize(formatters, typeof(T), value);
        }
        public static object Deserialize(this IEnumerable<System.Net.Http.Formatting.MediaTypeFormatter> formatters, Type type, string value)
        {
            if (value == null) return null;
            foreach (var formatter in formatters)
            {
                try
                {
                    return formatter.Deserialize(type, value);
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return null;
        }
    }
}
