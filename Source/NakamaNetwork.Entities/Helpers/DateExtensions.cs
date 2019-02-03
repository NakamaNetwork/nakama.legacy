using System;

namespace NakamaNetwork.Entities.Helpers
{
    public static class DateExtensions
    {
        public static long ToUnixEpochDate(this DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        public static long ToUnixEpochDate(this DateTimeOffset date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        public static DateTimeOffset FromUnixEpochDate(this long date)
        {
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).Add(TimeSpan.FromSeconds(date));
        }
    }
}
