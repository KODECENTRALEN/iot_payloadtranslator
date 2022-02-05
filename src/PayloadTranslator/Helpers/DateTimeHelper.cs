using System;
using System.Globalization;

namespace PayloadTranslator.Entities
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertToDateTime(string value)
        {
            try
            {
                var dateTime = DateTime.Parse(value, CultureInfo.CreateSpecificCulture("da-DK"));
                return dateTime;
            }
            catch
            {
            }

            try
            {
                var ecpochSeconds = long.Parse(value);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(ecpochSeconds);
                return dateTimeOffset.DateTime;
            }
            catch
            {
            }

            try
            {
                var ecpochMilliSeconds = long.Parse(value);
                DateTimeOffset dateTimeOffset2 = DateTimeOffset.FromUnixTimeMilliseconds(ecpochMilliSeconds);
                return dateTimeOffset2.DateTime;
            }
            catch
            {
            }

            return DateTime.MinValue;
        }
    }
}
