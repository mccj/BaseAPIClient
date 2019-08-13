using Newtonsoft.Json;
using System;

namespace SDK.BaseAPI
{
    public class TimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            bool nullable = (objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Nullable<>)));
            Type t = (nullable) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (
                t == typeof(System.DateTimeOffset) ||
                t == typeof(System.DateTime) ||
                t == typeof(System.TimeSpan)
            )
                return true;
            else
                return false;
        }
        public override bool CanWrite => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            bool nullable = (objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Nullable<>)));
            Type t = (nullable) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (t == typeof(System.DateTimeOffset))
            {
                if (reader.Value is long)
                    return new System.DateTimeOffset(ConvertStringToDateTime(Convert.ToInt64(reader.Value)));
                else if (reader.Value is string)
                {
                    string dateText = reader.Value as string;
                    if (!string.IsNullOrEmpty(this.DateTimeOffsetFormat))
                        return DateTimeOffset.ParseExact(dateText, this.DateTimeOffsetFormat, this.Culture, this.DateTimeStyles);
                    else
                        return DateTimeOffset.Parse(dateText, Culture, this.DateTimeStyles);
                }
            }
            else if (t == typeof(System.DateTime))
            {
                if (reader.Value is long)
                    return ConvertStringToDateTime(Convert.ToInt64(reader.Value));
                else if (reader.Value is string)
                {
                    string dateText = reader.Value as string;
                    if (!string.IsNullOrEmpty(this.DateTimeFormat))
                        return DateTime.ParseExact(dateText, this.DateTimeFormat, this.Culture, this.DateTimeStyles);
                    else
                        return DateTime.Parse(dateText, Culture, this.DateTimeStyles);
                }
            }
            else if (t == typeof(System.TimeSpan))
            {
                if (reader.Value is long)
                    return new System.TimeSpan(Convert.ToInt64(reader.Value) * 10000);
                else if (reader.Value is string)
                {
                    string dateText = reader.Value as string;
                    if (!string.IsNullOrEmpty(this.TimeSpanFormat))
                        return TimeSpan.ParseExact(dateText, this.TimeSpanFormat, this.Culture);
                    else
                        return TimeSpan.Parse(dateText, Culture);
                }
            }
            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //writer.WriteValue("");
        }
        public string TimeSpanFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public string DateTimeOffsetFormat { get; set; }
        public System.Globalization.DateTimeStyles DateTimeStyles
        {
            get { return _dateTimeStyles; }
            set { _dateTimeStyles = value; }
        }
        private System.Globalization.DateTimeStyles _dateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind;
        public System.Globalization.CultureInfo Culture
        {
            get { return _culture ?? System.Globalization.CultureInfo.CurrentCulture; }
            set { _culture = value; }
        }
        private System.Globalization.CultureInfo _culture;
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime ConvertStringToDateTime(long timeStamp)
        {
            //return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(timeStamp * 10000));
            return new DateTime(1970, 1, 1).Add(TimeZoneInfo.Local.BaseUtcOffset).Add(new TimeSpan(timeStamp * 10000));
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        private long ConvertDateTimeToInt(System.DateTime time)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            System.DateTime startTime = new DateTime(1970, 1, 1).Add(TimeZoneInfo.Local.BaseUtcOffset);
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}
