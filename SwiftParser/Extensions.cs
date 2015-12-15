using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public static class Extensions
    {
        public static string div(string i0, long i1)
        {
            var i0_buf = new StringBuilder(3);
            var i0_buf_int = 0L;
            var i0_1_mod_int = 0L;
            //var result = string.Empty;

            for (int i0_pos = 0, i0_length = i0.Length; i0_pos < i0_length; i0_pos++)
            {
                i0_buf.Append(i0[i0_pos]);

                i0_buf_int = long.Parse(i0_buf.ToString());
                i0_1_mod_int = i0_buf_int % i1;

                if (i0_1_mod_int != i0_buf_int)
                {
                    //result += ((i0_buf_int - i0_1_mod_int) / i1).ToString();
                    i0_buf.Length = 0;
                    if (0 != i0_1_mod_int)
                        i0_buf.Append(i0_1_mod_int.ToString());
                }
                //else
                //{
                //    if ((0 != i0_pos) && (i0_pos < i0_length))
                //        result += "0";
                //}
            }

            if (i0_buf.Length == 0)
                i0_buf.Append("0");

            return i0_buf.ToString();
        }

        public static DateTime SwiftToDateTime(this string date, string time = null)
        {
            var year = int.Parse(date.Substring(0, 2));
            date = (year <= 70 ? "20" : "19") + date;
            return DateTime.ParseExact(date + time, string.IsNullOrEmpty(time) ? "yyyyMMdd" : "yyyyMMddHHmm", null);
        }

        public static TimeSpan SwiftToTime(this string time)
        {
            return TimeSpan.ParseExact(time, "hhmm", null);
        }

        public static decimal SwiftToDecimal(this string amount)
        {
            var separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            amount = amount.Replace(',', separator);
            if (separator == amount[amount.Length - 1])
            {
                amount += "00";
            }
            return decimal.Parse(amount);
        }

        public static bool SwiftIsIBAN(this string iban)
        {
            if (string.IsNullOrEmpty(iban) || iban.Length > 34 || iban.Length <= 4)
                return false;

            iban = iban.ToUpper();

            //ф1. Първите четири знака (кодът на държавата и "00" като контролно число) се преместват в края на IBAN
            iban = iban.Substring(4) + iban.Substring(0, 4);

            //ф2. Буквите се преобразуват в числа съгласно таблицата за преобразуване
            //A = 10, B=11 .... Z=35
            StringBuilder tmp = new StringBuilder(22);
            int charA = (int)'A';
            foreach (char c in iban)
            {
                if (char.IsDigit(c))
                {
                    tmp.Append(c);
                }
                else
                {
                    if ('A' <= c && c <= 'Z')
                    {
                        tmp.Append(10 + (((int)c) - charA));
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            //ф3. Изчислява се контролно число по модул 97, като:
            //    * полученото число от фаза 2 се разделя на 97;
            //    * остатъкът след деленето се изважда от 98;
            //    * ако резултатът е една цифра пред нея се поставя нула.
            return "1" == div(tmp.ToString(), 97L);
        }

        public static Tag SwiftFirst(this IEnumerable<Tag> _this, params string[] ids)
        {
            return _this.Where(i => Array.IndexOf(ids, i.Id) > -1).First();
        }

        public static Tag SwiftFirstOrDefault(this IEnumerable<Tag> _this, params string[] ids)
        {
            return _this.Where(i => Array.IndexOf(ids, i.Id) > -1).FirstOrDefault();
        }

        public static TagValue SwiftFirst(this IEnumerable<TagValue> _this, params string[] ids)
        {
            return _this.Where(i => Array.IndexOf(ids, i.Id) > -1).First();
        }

        public static TagValue SwiftFirstOrDefault(this IEnumerable<TagValue> _this, params string[] ids)
        {
            return _this.Where(i => Array.IndexOf(ids, i.Id) > -1).FirstOrDefault();
        }

        public static string SwiftConcat(this IEnumerable<TagValue> _this, string[] ids, string separator = " ", string defaultValue = null)
        {
            var buffer = new StringBuilder();
            foreach (var item in _this.Where(i => Array.IndexOf(ids, i.Id) > -1))
            {
                if (buffer.Length > 0)
                    buffer.Append(separator);

                buffer.Append(item.Value);
            }
            var val = buffer.ToString().Trim().Trim(separator.ToCharArray());
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }

        public static string SwiftConcat(this IEnumerable<TagValue> _this, string id, string separator = " ", string defaultValue = null)
        {
            var buffer = new StringBuilder();
            foreach (var item in _this.Where(i => i.Id == id))
            {
                if (buffer.Length > 0)
                    buffer.Append(separator);

                buffer.Append(item.Value);
            }

            var val = buffer.ToString().Trim().Trim(separator.ToCharArray());
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }

        public static string SwiftConcat(this IEnumerable<TagValue.TagValueItem> _this, string[] ids, string separator = " ", string defaultValue = null)
        {
            var buffer = new StringBuilder();
            foreach (var item in _this.Where(i => Array.IndexOf(ids, i.Id) > -1))
            {
                if (buffer.Length > 0)
                    buffer.Append(separator);

                buffer.Append(item.Value);
            }
            var val = buffer.ToString().Trim().Trim(separator.ToCharArray());
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }

        public static string SwiftConcat(this IEnumerable<TagValue.TagValueItem> _this, string id, string separator = " ", string defaultValue = null)
        {
            var buffer = new StringBuilder();
            foreach (var item in _this.Where(i => i.Id == id))
            {
                if (buffer.Length > 0)
                    buffer.Append(separator);

                buffer.Append(item.Value);
            }

            var val = buffer.ToString().Trim().Trim(separator.ToCharArray());
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }

        public static int SwiftSafeLength(this string value)
        {
            if (value == null)
                return 0;

            return value.Length;
        }

        public static string SwiftSafeLength(this string value, int length)
        {
            if (value == null)
                return value;

            return value.Substring(0, Math.Min(length, value.Length));
        }
    }
}
