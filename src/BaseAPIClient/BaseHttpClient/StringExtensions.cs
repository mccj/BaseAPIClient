using System.Text;

namespace SDK.BaseAPI.Linq
{
    /// <summary>
    /// 异常操作扩展
    /// </summary>
    public static class StringExtensions
    {
        public static string EncodeUrl(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return System.Uri.EscapeDataString(str);
        }
    }
}
