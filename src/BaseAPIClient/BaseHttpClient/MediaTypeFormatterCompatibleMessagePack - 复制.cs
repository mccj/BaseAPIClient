using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SDK.BaseAPI
{
    public class ParamR
    {
        private string boundary = "------------Boundary" + System.Guid.NewGuid().ToString("N");
        private Dictionary<string, object> _paramDic = null;
        public ParamR() : this(string.Empty) { }
        public ParamR(string query)
        {
            var _query = System.Web.HttpUtility.ParseQueryString(query);
            _paramDic = new Dictionary<string, object>();
            foreach (var item in _query.AllKeys)
            {
                _paramDic.Add(item, _query[item]);
            }
        }
        public ParamR(Dictionary<string, object> paramDic)
        {
            _paramDic = paramDic == null ? new Dictionary<string, object>() : paramDic.Where(f => !string.IsNullOrWhiteSpace(f.Key) && f.Value != null && !string.IsNullOrWhiteSpace(f.Value.ToString())).ToDictionary(f => f.Key, f => f.Value);
        }
        public string QueryString()
        {
            return string.Join("&", _paramDic.Where(f => !(f.Value is byte[])).Select(f => f.Key + "=" + f.Value));
        }
        public byte[] ToBytes(Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            if (this.IsBytes)
            {
                MemoryStream _stream = new MemoryStream();
                foreach (var item in _paramDic)
                {
                    if (item.Value is byte[])
                        AddContent(_stream, encoding, boundary, item.Key, item.Key, item.Value as byte[]);
                    else
                        AddContent(_stream, encoding, boundary, item.Key, Convert.ToString(item.Value));
                }
                return _stream.ToArray();
            }
            else
                return encoding.GetBytes(this.QueryString());
        }
        private void AddContent(Stream _stream, Encoding encoding, string _boundary, string name, string value)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var sp = string.Format("\r\n--{0}\r\n", _boundary);
            sp += string.Format("Content-Disposition: form-data; name=\"{0}\"; \r\n\r\n{1}", name, value);
            var data = encoding.GetBytes(sp);
            _stream.Write(data, 0, data.Length);
        }
        private void AddContent(Stream _stream, Encoding encoding, string _boundary, string name, string fileName, byte[] fileData)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var sp = string.Format("\r\n--{0}\r\n", _boundary);
            sp += string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", name, fileName);
            var data = encoding.GetBytes(sp);
            _stream.Write(data, 0, data.Length);
            _stream.Write(fileData, 0, fileData.Length);
            data = encoding.GetBytes("\r\n");
            _stream.Write(data, 0, data.Length);
        }
        public override string ToString()
        {
            return this.QueryString();
        }
        public bool IsBytes
        {
            get
            {
                return _paramDic.Any(f => (f.Value is byte[]));
            }
        }
        public string ContentType
        {
            get
            {
                //string boundary = "------------7d930d1a850658";
                return this.IsBytes ? ("multipart/form-data; boundary=" + boundary) : "application/x-www-form-urlencoded";
            }
        }
        public Dictionary<string, object> ToDictionary()
        {
            return _paramDic;
        }
    }
}