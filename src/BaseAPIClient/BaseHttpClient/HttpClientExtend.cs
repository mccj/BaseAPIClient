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
    public class HttpClientExtend : HttpClient
    {
        private readonly bool _ifNullRemove;
        #region 构造函数
        public HttpClientExtend(MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null) : this(string.Empty, mediaType, supportedMediaTypes) { }
        public HttpClientExtend(string url, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null) : this(string.IsNullOrWhiteSpace(url) ? null : new Uri(url), mediaType, supportedMediaTypes) { }
        public HttpClientExtend(Uri url, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null, bool ifNullRemove = true) : this(
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
        public HttpClientExtend(HttpClientHandler handler, Uri url = null, MediaType mediaType = MediaType.Json, string[] supportedMediaTypes = null, bool ifNullRemove = true) : base(handler)
        {
            this._ifNullRemove = ifNullRemove;
            if (System.Net.ServicePointManager.ServerCertificateValidationCallback == null)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((sender, certificate, chain, errors) => { return true; });
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            this.BaseAddress = url;
            Formatter = GetMediaTypeFormatter(mediaType, supportedMediaTypes);
            //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("zh-cn"));
            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            //httpClient.DefaultRequestHeaders.Add("Authentication", "123");
            //httpClient.MaxResponseContentBufferSize = 256000;
            //httpClient.Timeout = new TimeSpan(0,1,0);

            //System.Net.Http.ByteArrayContent;
            //System.Net.Http.ByteRangeStreamContent;
            //System.Net.Http.FormUrlEncodedContent;
            //System.Net.Http.HttpContent;
            //System.Net.Http.HttpMessageContent;
            //System.Net.Http.MultipartContent;
            //System.Net.Http.MultipartFormDataContent;
            //System.Net.Http.ObjectContent;
            //System.Net.Http.ObjectContent<object>;
            //System.Net.Http.PushStreamContent;
            //System.Net.Http.StreamContent;
            //System.Net.Http.StringContent;

            //System.Net.Http.Formatting.XmlMediaTypeFormatter;
            //System.Net.Http.Formatting.MediaTypeFormatter;
            //System.Net.Http.Formatting.JsonMediaTypeFormatter;
            //System.Net.Http.Formatting.BufferedMediaTypeFormatter;
            //System.Net.Http.Formatting.BsonMediaTypeFormatter;
            //System.Net.Http.Formatting.BaseJsonMediaTypeFormatter;


            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application /json"));
            //_client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application /xml"));
            //_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", this.ApiToken);
            //_client.DefaultRequestHeaders.Add("SunYou -Token", "wwXWyCF9Dz/pXL6EsMDkag== ");
            // 为JSON格式添加一个Accept报头
            //System.Net.Http.Formatting.JsonMediaTypeFormatter formatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter { Indent = false, UseDataContractJsonSerializer = false };
            ////System.Net.Http.Formatting.XmlMediaTypeFormatter formatter = new System.Net.Http.Formatting.XmlMediaTypeFormatter { UseXmlSerializer = true, Indent = true };
            //System.Net.Http.Formatting.MediaTypeFormatter[] formatters { get { return new System.Net.Http.Formatting.MediaTypeFormatter[] { formatter }; } }
        }
        #endregion 构造函数
        #region 属性
        /// <summary>
        /// 请求客户端
        /// </summary>
        //protected HttpClient httpClient { get; }
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter Formatter { get; private set; }// = new System.Net.Http.Formatting.JsonMediaTypeFormatter { Indent = false, UseDataContractJsonSerializer = false };
        //protected virtual System.Net.Http.Formatting.MediaTypeFormatter Formatter { get; } = new System.Net.Http.Formatting.XmlMediaTypeFormatter { UseXmlSerializer = true, Indent = true };
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter[] Formatters { get { return _formatters ?? new[] { Formatter }; } private set { _formatters = value; } }
        protected System.Net.Http.Formatting.MediaTypeFormatter[] _formatters = null;
        #endregion 属性
        #region 方法
        public string TestSerialize(object o)
        {
            //return SerializationHelper.SerializeToXml(o);
            //return new System.Net.Http.ObjectContent(o.GetType(), o, formatter).ReadAsStringAsync().Result;
            return Formatter.Serialize(o);
        }
        public T TestDeserialize<T>(string str)
        {
            //return SerializationHelper.DeserializeFromXml<T>(str);
            //var ddd1 = new System.Net.Http.StringContent(str, null, "text/xml").ReadAsAsync<T>(formatters).Result;
            return Formatter.Deserialize<T>(str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public string Encode(object o, MediaType? mediaType = null)
        {
            var formatter = mediaType == null ? this.Formatter : GetMediaTypeFormatter(mediaType.Value);
            return formatter.Serialize(o);
        }
        public T Decode<T>(string str, MediaType? mediaType = null)
        {
            var formatter = mediaType == null ? this.Formatter : GetMediaTypeFormatter(mediaType.Value);
            return formatter.Deserialize<T>(str);
        }
        #region Get
        public TResponse Get<TResponse>(string requestUri)
        {
            return this.GetAsync<TResponse>(requestUri).Result;
        }
        public TResponse Get<TResponse>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            return this.GetAsync<TResponse>(requestUri, nameValueCollection).Result;
        }
        public Task<TResponse> GetAsync<TResponse>(string requestUri)
        {
            var task = this.GetAsync(requestUri);
            return GetResultContentAsync<TResponse>(task);
            //return HttpClientExtensions.PostAsync(this, requestUri, value, Formatter);
        }
        public Task<TResponse> GetAsync<TResponse>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            var dic = _ifNullRemove ? (nameValueCollection.Where(f => !string.IsNullOrWhiteSpace(f.Key) && f.Value != null && !string.IsNullOrWhiteSpace(f.Value.ToString()))) : nameValueCollection;
            var _requestUri = UrlAddQuery(requestUri, string.Join("&", dic.Select(f => f.Key + "=" + f.Value)));
            return this.GetAsync<TResponse>(_requestUri);
        }
        #endregion Get
        #region Post
        public TResponse PostFormUrl<TResponse>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            return PostFormUrlAsync<TResponse>(requestUri, nameValueCollection).Result;
        }
        public TResponse PostBytes<TResponse>(string requestUri, byte[] bytes, string mediaType = null)
        {
            return PostBytesAsync<TResponse>(requestUri, bytes, mediaType).Result;
        }
        public HttpResponseMessage PostBytes(string requestUri, byte[] bytes, string mediaType = null)
        {
            return PostBytesAsync<HttpResponseMessage>(requestUri, bytes, mediaType).Result;
        }
        public TResponse PostString<TResponse>(string requestUri, string str, Encoding encoding = null, string mediaType = null)
        {
            return PostStringAsync<TResponse>(requestUri, str, encoding, mediaType).Result;
        }
        public TResponse PostStream<TResponse>(string requestUri, Stream content)
        {
            return PostStreamAsync<TResponse>(requestUri, content).Result;
        }

        public HttpResponseMessage Post<TRequest>(string requestUri, TRequest value, string mediaType = null)
        {
            return PostAsync<TRequest>(requestUri, value, mediaType).Result;
        }
        public HttpResponseMessage Post<TRequest>(Uri requestUri, TRequest value, string mediaType = null)
        {
            return PostAsync<TRequest>(requestUri, value, mediaType).Result;
        }
        public TResponse Post<TRequest, TResponse>(string requestUri, TRequest value, string mediaType = null)
        {
            return PostAsync<TRequest, TResponse>(requestUri, value, mediaType).Result;
        }
        public TResponse Post<TRequest, TResponse>(Uri requestUri, TRequest value, string mediaType = null)
        {
            return PostAsync<TRequest, TResponse>(requestUri, value, mediaType).Result;
        }





        public Task<TResponse> PostFormUrlAsync<TResponse>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            var dic = _ifNullRemove ? (nameValueCollection.Where(f => !string.IsNullOrWhiteSpace(f.Key) && f.Value != null && !string.IsNullOrWhiteSpace(f.Value.ToString()))) : nameValueCollection;
            var task = base.PostAsync(requestUri, new FormUrlEncodedContent(dic));
            return GetResultContentAsync<TResponse>(task);
        }
        //public TResponse PostFormUrlAsync<TResponse>(string requestUri, string query)
        //{
        //   var q= System.Web.HttpUtility.ParseQueryString(query);

        //}
        public Task<TResponse> PostBytesAsync<TResponse>(string requestUri, byte[] bytes, string mediaType = null)
        {
            var task = PostBytesAsync(requestUri, bytes, mediaType);
            return GetResultContentAsync<TResponse>(task);
        }
        public Task<HttpResponseMessage> PostBytesAsync(string requestUri, byte[] bytes, string mediaType = null)
        {
            var content = new ByteArrayContent(bytes);
            if (!string.IsNullOrWhiteSpace(mediaType))
            {
                content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(mediaType);
            }
            var task = base.PostAsync(requestUri, content);
            return task;
        }

        public Task<TResponse> PostStringAsync<TResponse>(string requestUri, string str, Encoding encoding = null, string mediaType = null)
        {
            var task = base.PostAsync(requestUri, new StringContent(str, encoding, mediaType));
            return GetResultContentAsync<TResponse>(task);
        }
        public Task<TResponse> PostStreamAsync<TResponse>(string requestUri, Stream content)
        {
            var task = base.PostAsync(requestUri, new StreamContent(content));
            return GetResultContentAsync<TResponse>(task);
        }

        public Task<HttpResponseMessage> PostAsync<TRequest>(string requestUri, TRequest value, string mediaType = null)
        {
            return PostAsync(new Uri(requestUri), value, mediaType);
        }
        public Task<HttpResponseMessage> PostAsync<TRequest>(Uri requestUri, TRequest value, string mediaType = null)
        {
            if (value is HttpContent)
            {
                return base.PostAsync(requestUri, value as HttpContent);
            }
            else
            {
                return HttpClientExtensions.PostAsync(this, requestUri, value, Formatter, mediaType);
            }
        }
        public Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value, string mediaType = null)
        {
            if (value is HttpContent)
            {
                var task = base.PostAsync(requestUri, value as HttpContent);
                return GetResultContentAsync<TResponse>(task);
            }
            else
            {
                var task = HttpClientExtensions.PostAsync(this, requestUri, value, Formatter, mediaType);
                return GetResultContentAsync<TResponse>(task);
                //return this.PostAsync<string, string>("api/WebErp.Orders/Order/GetOrdersByExpression?count={count}".Replace("{count}", System.Convert.ToString(count)), predicate);//?.Result?.GetResultContent<Order[]>(this.MediaTypeFormatters);
            }
        }
        public Task<TResponse> PostAsync<TRequest, TResponse>(Uri requestUri, TRequest value, string mediaType = null)
        {
            if (value is HttpContent)
            {
                var task = base.PostAsync(requestUri, value as HttpContent);
                return GetResultContentAsync<TResponse>(task);
            }
            else
            {
                var task = HttpClientExtensions.PostAsync(this, requestUri, value, Formatter, mediaType);
                return GetResultContentAsync<TResponse>(task);
            }
        }
        #endregion Post
        #region Put
        public HttpResponseMessage Put<TRequest>(string requestUri, TRequest value, string mediaType = null)
        {
            return HttpClientExtensions.PutAsync(this, requestUri, value, Formatter, mediaType).Result;
        }
        public HttpResponseMessage Put<TRequest>(Uri requestUri, TRequest value, string mediaType = null)
        {
            return HttpClientExtensions.PutAsync(this, requestUri, value, Formatter, mediaType).Result;
        }
        public Task<HttpResponseMessage> PutAsync<TRequest>(string requestUri, TRequest value, string mediaType = null)
        {
            return HttpClientExtensions.PutAsync(this, requestUri, value, Formatter, mediaType);
        }
        public Task<HttpResponseMessage> PutAsync<TRequest>(Uri requestUri, TRequest value, string mediaType = null)
        {
            return HttpClientExtensions.PutAsync(this, requestUri, value, Formatter, mediaType);
        }
        #endregion Put
        #endregion 方法
        #region 内容解析方法
        protected internal virtual Type ErrorType { get; private set; } = null;
        //protected internal virtual object ErrorHandle(Type type, HttpResponseMessage message) { return null; }
        protected internal virtual Func<Type, HttpResponseMessage, object> ErrorHandle { get; private set; } = new Func<Type, HttpResponseMessage, object>((type, message) => null);

        protected internal void SetErrorType(Type type) => this.ErrorType = type;
        protected internal void SetErrorHandle(Func<Type, HttpResponseMessage, object> errorHandle) => this.ErrorHandle = errorHandle;
        protected internal void SetFormatter(System.Net.Http.Formatting.MediaTypeFormatter formatter) { if (formatter != null) this.Formatter = formatter; }
        protected internal void SetFormatters(System.Net.Http.Formatting.MediaTypeFormatter[] formatters) { if (formatters != null) this.Formatters = formatters; }
        protected internal void SetMediaTypeFormatter(Func<MediaType, string[], System.Net.Http.Formatting.MediaTypeFormatter> func) { if (func != null) this.getMediaTypeFormatterFunc = func; }
        private async Task<T> GetResultContentAsync<T>(Task<HttpResponseMessage> task)
        {
            try
            {
                var message = task.Result;
                try
                {
                    //1.TContent 是否继承自错误基类，是则使用TContent解析返回的正常及错误数据，不会抛出异常
                    if (message != null && (message.IsSuccessStatusCode || typeof(IErrorResponse).IsAssignableFrom(typeof(T))))
                    {
                        try
                        {
                            var result = await message.Content?.ReadAsAsync<T>(this.Formatters);//解析响应体。阻塞！
                            return result;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (AggregateException)
                {

                }
                catch (Exception)
                {
                    //throw;
                }
                if (typeof(T) == typeof(string))
                {
                    var result = await message.Content?.ReadAsStringAsync();//解析响应体。阻塞！
                    return (T)(result as object);
                }
                else
                {
                    //throw;
                }
                try
                {
                    //2.是否指定异常错误类，是则尝试使用异常错误类解析错误，并且抛出错误
                    if (message != null && !message.IsSuccessStatusCode && this.ErrorType != null)
                    {
                        if (!typeof(IErrorResponse).IsAssignableFrom(this.ErrorType))
                        {
                            throw new Exception("错误类必须继承自 " + nameof(IErrorResponse));
                        }
                        //var str = message.Content?.ReadAsStringAsync()?.Result;//解析响应体。阻塞！
                        var result = await message.Content?.ReadAsAsync(this.ErrorType, this.Formatters);//解析响应体。阻塞！
                        var error = result as IErrorResponse;
                        throw new ApiException(error);
                    }
                }
                catch (System.Net.Http.UnsupportedMediaTypeException)
                {

                }
                catch (ApiException ex)
                {
                    throw ex;
                }
                catch (Exception)
                {
                    //throw;
                }
                //3.是否实现了自定义错误处理方法，有则执行错误处理方法
                {
                    //var str = message.Content?.ReadAsStringAsync()?.Result;//解析响应体。阻塞！
                    var result = this.ErrorHandle?.Invoke(typeof(T), message);
                    if (result != null)
                        return (T)result;
                    else
                    {
                        //抛出Exception异常
                        //message.EnsureSuccessStatusCode();
                        var resultStr = await message.Content?.ReadAsStringAsync();//解析响应体。阻塞！
                        var reg = new System.Text.RegularExpressions.Regex("<title>(?<ErrMsg>.*)</title>");
                        var errMsg = reg.Match(resultStr)?.Groups["ErrMsg"]?.Value;
                        var error = new Exception(resultStr);
                        if (string.IsNullOrWhiteSpace(errMsg))
                        {
                            throw new Exception("系统内部错误 " + message.ReasonPhrase, error);
                        }
                        else
                        {
                            throw new Exception(message.ReasonPhrase + " " + errMsg, error);
                        }
                    }
                }
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (AggregateException ex)
            {
                throw new Exception(ex.DetailMessage1(), ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 内容解析方法

        protected internal string UrlAddQuery(string url, string paramQuery)
        {
            //return UrlAddQuery(new Uri(url), paramQuery).AbsoluteUri;
            return UrlAddQuery(new Uri(url), paramQuery).AbsoluteUri;

        }
        protected internal Uri UrlAddQuery(Uri url, string paramQuery)
        {
            var query = System.Web.HttpUtility.ParseQueryString(url.Query);
            var param = System.Web.HttpUtility.ParseQueryString(paramQuery);
            foreach (var item in param.AllKeys)
            {
                query[item] = param[item];
            }
            return new Uri(url, "?" + query.ToString());
        }
        public string ToBase64(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public string FromBase64(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
        private Func<MediaType, string[], System.Net.Http.Formatting.MediaTypeFormatter> getMediaTypeFormatterFunc = null;
        protected internal virtual System.Net.Http.Formatting.MediaTypeFormatter GetMediaTypeFormatter(MediaType mediaType, params string[] supportedMediaTypes)
        {
            var rr = getMediaTypeFormatterFunc?.Invoke(mediaType, supportedMediaTypes);
            if (rr != null) return rr;


            //bool setPropertySettingsFromAttributes = true, isContractNHibernateProxy = false;
            System.Net.Http.Formatting.MediaTypeFormatter formatter = null;
            switch (mediaType)
            {
                case MediaType.Json:
                    formatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter
                    {
                        Indent = true,
                        SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                        {
                            Formatting = Newtonsoft.Json.Formatting.Indented,//是否格式化缩进
                            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,//空值忽略
                            DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,//序列化和反序列化时,包含默认值
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,//避免循环引用
                            PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,//序列化时保留引用。
                            DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,//将所有日期转换成UTC格式
                            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,//JSON日期格式
                            Converters = new Newtonsoft.Json.JsonConverter[] {
                                //new CamelCasePropertyNamesContractResolver(),//驼峰式大小写转换
                                new Newtonsoft.Json.Converters.StringEnumConverter(),//枚举转换成枚举名称字符串
                                //new Orchard.Utility.Json.LocalDateTimeConverter(),
                                //new Orchard.Utility.Json.InterfaceConverter(),
                                new Utilities.ExpandoObject2.Converters.ExpandoObject2Converter(),
                                new Utilities.ExpandoObject2.Converters.DynamicJsonConverter(),
                                //////new Newtonsoft.Json.Converters.MicrosoftJavaScriptDateTimeConverter(),
                                //new TimestampConverter()
                                //new Orchard.Utility.Json.NHibernateProxyConverter(isContractNHibernateProxy),
                                //new Orchard.Utility.Json.EncodeNHibernateContractResolver(setPropertySettingsFromAttributes, isContractNHibernateProxy)
                            },

                            Error = (currentObject, errorContext) =>
                                    {

                                    }
                        }
                    };
                    this.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    break;
                case MediaType.Xml:
                    formatter = new System.Net.Http.Formatting.XmlMediaTypeFormatter { Indent = true, UseXmlSerializer = true };
                    this.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                    break;
                case MediaType.Bson:
                    formatter = new System.Net.Http.Formatting.BsonMediaTypeFormatter { };
                    break;
                default:
                    break;
            }
            foreach (var item in supportedMediaTypes ?? new string[] { })
            {
                try
                {
                    formatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue(item));
                    //formatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
                }
                catch (Exception)
                {
                    //    throw;
                }
            }
            return formatter;
        }
    }

    public enum MediaType
    {
        Json, Xml, Bson
    }
}
