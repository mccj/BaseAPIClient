using System;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var rr = new SDK.BaseAPI.HttpClientExtend();
            rr.PostContentAsync<string>("", rr.CreateHttpContent<string>("",""));
        }
    }
}
