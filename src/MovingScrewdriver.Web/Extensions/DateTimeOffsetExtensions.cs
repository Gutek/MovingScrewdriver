using System;

namespace MovingScrewdriver.Web.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToLocalDate(this DateTimeOffset @this)
        {
            return @this.LocalDateTime.ToShortDateString();
        }

        public static string ToLocalDateTime(this DateTimeOffset @this)
        {
            return "{0} {1}".FormatWith(@this.LocalDateTime.ToShortDateString(), @this.LocalDateTime.ToShortTimeString());
        }

        public static string ToZuluDateTime(this DateTimeOffset @this)
        {
            return @this.ToString("u");
        }

        public static DateTimeOffset AsMinutes(this DateTimeOffset @this)
        {
            return new DateTimeOffset(@this.Year, @this.Month, @this.Day, @this.Hour, @this.Minute, 0, 0, @this.Offset);
        }

        public static DateTimeOffset ConvertFromUnixTimestamp(long timestamp)
        {
            var origin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, DateTimeOffset.Now.Offset);
            return origin.AddSeconds(timestamp);
        }

        public static DateTimeOffset ConvertFromJsTimestamp(long timestamp)
        {
            var origin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, DateTimeOffset.Now.Offset);
            return origin.AddMilliseconds(timestamp);
        }
    }
}