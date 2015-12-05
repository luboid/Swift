using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    static class SwiftUtil
    {
        public static DateTime ToDateTime(string date, string time = null)
        {
            date = (int.Parse(date.Substring(0, 2)) <= 70 ? "20" : "19") + date;
            date += time;
            return DateTime.ParseExact(date, string.IsNullOrEmpty(time) ? "yyyyMMdd" : "yyyyMMddHHmm", null);
        }

        public static decimal ToDecimal(string amount)
        {
            amount = amount.Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            return decimal.Parse(amount);
        }
    }
}
