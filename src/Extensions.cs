namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    public static class Extensions
    {
        public static byte[] AsByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string AsJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string AsString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static DateTime FromUnixTimestamp(this double dateTime)
        {
            DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(dateTime).ToLocalTime();
            return datetime;
        }

        public static double ToUnixTimestamp(this DateTime d)
        {
            TimeSpan duration = d.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return duration.TotalMilliseconds;
        }

        public static string TruncateString(this string value, int len)
        {
            return value.Length < len ? value : value.Substring(0, len);
        }
    }
}