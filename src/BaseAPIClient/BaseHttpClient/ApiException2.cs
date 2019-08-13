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
    public class ApiException2 : Exception
    {
        public ApiException2(string message) : base(message) { }
        public ApiException2(IErrorResponse error) : base(error.ErrorMessage)
        {
            this.Error = error;
        }
        public IErrorResponse Error { get; }
    }
}
