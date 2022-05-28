using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzollutionPro_BAL
{
    public static class Extensions
    {
        public static string FormatDate(this DateTime? Date)
        {
            if (Date == null)
                return "";
            else
                return Date.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public static string FormatDate(this DateTime Date)
        {
            if (Date == null)
                return "";
            else
                return Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public static string FormatDateTime(this DateTime? Date)
        {
            if (Date == null)
                return "";
            else
                return Date.Value.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        }

        public static DateTime? ConvertDate(this string Date)
        {
            if (string.IsNullOrEmpty(Date))
                return null;
            else
                return DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
