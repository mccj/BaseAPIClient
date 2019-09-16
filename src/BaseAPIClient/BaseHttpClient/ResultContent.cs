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
    //public interface IError
    //{
    //    IErrorResponse Error { get; }
    //}
    public interface IResultContent
    {
        Task<T> GetResultContentAsync<T>(Task<HttpResponseMessage> task, System.Net.Http.Formatting.MediaTypeFormatter[] formatters = null, Type errorType = null, Func<Type, HttpResponseMessage, object> errorHandle = null);
        Task<T> GetResultContentAsync<T>(HttpResponseMessage message, System.Net.Http.Formatting.MediaTypeFormatter[] formatters = null, Type errorType = null, Func<Type, HttpResponseMessage, object> errorHandle = null);
    }
    public class ResultContent : IResultContent
    {
        public System.Net.Http.Formatting.MediaTypeFormatter[] Formatters { get; set; }
        public System.Net.Http.Formatting.MediaTypeFormatter[] DefaultFormatters { get; } = new System.Net.Http.Formatting.MediaTypeFormatter[] {
            new System.Net.Http.Formatting.JsonMediaTypeFormatter(),
            new System.Net.Http.Formatting.XmlMediaTypeFormatter(),
            new System.Net.Http.Formatting.BsonMediaTypeFormatter(),
            new  System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter()
        };

        public async Task<T> GetResultContentAsync<T>(Task<HttpResponseMessage> task, System.Net.Http.Formatting.MediaTypeFormatter[] formatters = null, Type errorType = null, Func<Type, HttpResponseMessage, object> errorHandle = null)
        {
            var message = await task;
            return await GetResultContentAsync<T>(message, formatters, errorType, errorHandle);
        }
        public async Task<T> GetResultContentAsync<T>(HttpResponseMessage message, System.Net.Http.Formatting.MediaTypeFormatter[] formatters = null, Type errorType = null, Func<Type, HttpResponseMessage, object> errorHandle = null)
        {
            try
            {
                #region 正常解析
                try
                {
                    //1.TContent 是否继承自错误基类，是则使用TContent解析返回的正常及错误数据，不会抛出异常
                    if (message != null && (message.IsSuccessStatusCode || typeof(IErrorResponse).IsAssignableFrom(typeof(T))))
                    {
                        try
                        {
                            var result = await message.Content?.ReadAsAsync<T>(formatters ?? this.Formatters ?? this.DefaultFormatters);//解析响应体。阻塞！
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
                #endregion 正常解析
                #region 解析字符串
                if (typeof(T) == typeof(string))
                {
                    var result = await message.Content?.ReadAsStringAsync();//解析响应体。阻塞！
                    return (T)(result as object);
                }
                else
                {
                    //throw;
                }
                #endregion 解析字符串
                #region ErrorResponse解析
                try
                {
                    //2.是否指定异常错误类，是则尝试使用异常错误类解析错误，并且抛出错误
                    if (message != null && !message.IsSuccessStatusCode && errorType != null)
                    {
                        if (!typeof(IErrorResponse).IsAssignableFrom(errorType))
                        {
                            throw new Exception("错误类必须继承自 " + nameof(IErrorResponse));
                        }
                        //var str = message.Content?.ReadAsStringAsync()?.Result;//解析响应体。阻塞！
                        var result = await message.Content?.ReadAsAsync(errorType, formatters ?? this.Formatters ?? this.DefaultFormatters);//解析响应体。阻塞！
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
                #endregion ErrorResponse解析
                #region 自定义错误处理
                //3.是否实现了自定义错误处理方法，有则执行错误处理方法
                {
                    //var str = message.Content?.ReadAsStringAsync()?.Result;//解析响应体。阻塞！
                    var result = errorHandle?.Invoke(typeof(T), message);
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
                #endregion 自定义错误处理
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
    }
}
