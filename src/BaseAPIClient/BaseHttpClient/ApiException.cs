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
    public interface IErrorResponse
    {
        string ErrorMessage { get; }
    }
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
        public ApiException(IErrorResponse error) : base(error.ErrorMessage)
        {
            this.Error = error;
        }
        public IErrorResponse Error { get; }

        public ApiException()
        {
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
