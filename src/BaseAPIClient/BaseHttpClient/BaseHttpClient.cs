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

namespace SDK.BaseAPI
{
    public class BaseHttpClient
    {
        #region 构造函数
        public BaseHttpClient(MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null) : this(string.Empty, mediaType, supportedMediaTypes) { }
        public BaseHttpClient(string url, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null) : this(string.IsNullOrWhiteSpace(url) ? null : new Uri(url), mediaType, supportedMediaTypes) { }
        public BaseHttpClient(Uri url, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null, bool ifNullRemove = true) : this(
            new HttpClientHandler
            {
                UseCookies = true,
                AllowAutoRedirect = true,
                //Proxy = System.Net.WebRequest.DefaultWebProxy;
                //Proxy = System.Net.WebRequest.DefaultWebProxy;
            },
            url,
            mediaType, supportedMediaTypes, ifNullRemove)
        { }
        public BaseHttpClient(HttpClientHandler handler, Uri url = null, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null, bool ifNullRemove = true)
        {
            Client = new HttpClientExtend(handler, url, mediaType, supportedMediaTypes, ifNullRemove);
            Client.SetErrorType(this.ErrorType);
            Client.SetErrorHandle(this.ErrorHandle);
            Client.SetFormatter(this.Formatter);
            Client.SetFormatters(this.Formatters);
            Client.SetMediaTypeFormatter(this.GetMediaTypeFormatter);
        }
        #endregion 构造函数

        protected internal HttpClientExtend Client { get; } = null;
        protected internal virtual Type ErrorType { get; } = null;
        protected internal virtual object ErrorHandle(Type type, HttpResponseMessage message) { return null; }
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter Formatter { get; } = null;
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter[] Formatters { get; } = null;
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter GetMediaTypeFormatter(MediaType mediaType, params string[] supportedMediaTypes)
        {
            return null;
        }
    }
}
