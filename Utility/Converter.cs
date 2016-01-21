/*
 源码己托管:https://github.com/v5bep7/Utility
 */
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 类型转换辅助类
    /// 该类里面皆为拓展方法
    /// </summary>
    /// <remarks>
    /// 2016/1/15 16:49:44 Created By Devin
    /// </remarks>
    public static class Converter
    {
        #region String 转其他类型

        /// <summary>
        /// string to bool
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>bool</returns>
        public static bool ToBoolean(this string str, bool def = default(bool))
        {
            bool result;
            return bool.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// stirng to byte 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>byte</returns>
        public static byte ToByte(this string str, byte def = default(byte))
        {
            byte result;
            return byte.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to char
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>char</returns>
        public static char ToChar(this string str, char def = default(char))
        {
            char result;
            return char.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to short
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>short</returns>
        public static short ToShort(this string str, short def = default(short))
        {
            short result;
            return short.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to int
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>int</returns>
        public static int ToInt(this string str, int def = default(int))
        {
            int result;
            return int.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to long 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>long</returns>
        public static long ToLong(this string str, long def = default(long))
        {
            long result;
            return long.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to decimal 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this string str, decimal def = default(decimal))
        {
            decimal result;
            return decimal.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to float 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>float</returns>
        public static float ToFloat(this string str, float def = default(float))
        {
            float result;
            return float.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to double 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>double</returns>
        public static double ToDouble(this string str, double def = default(double))
        {
            double result;
            return double.TryParse(str, out result) ? result : def;
        }

        /// <summary>
        /// string to DateTime 
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="def">默认值</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this string str, string format, DateTime def = default(DateTime))
        {
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = format;
            try
            {
                DateTime dt = Convert.ToDateTime(str, dtFormat);
                return dt;
            }
            catch (Exception e)
            {
                return def;
            }
        }

        #endregion

        #region DateTime 转 String

        private const string DateFormat = "yyyy-MM-dd HH:ss";

        /// <summary>
        /// DataTime转换为String,格式为 "yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">this</param>
        /// <returns>string</returns>
        public static string ToString(this DateTime dateTime)
        {
            return dateTime.ToString(DateFormat);
        }

        /// <summary>
        /// 将DataTime类型转换为中文 
        /// 例如  2012-12-21 12:12:21.012 → 1月前 
        /// </summary>
        /// <param name="dateTime">this</param>
        /// <returns>string</returns>
        /// <example>
        /// 2012-12-21 12:12:21.012 → 1月前
        /// 2011-12-21 12:12:21.012 → 1年前
        /// </example>
        public static string ToChsStr(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            if ((int)ts.TotalDays >= 365)
                return (int)ts.TotalDays / 365 + "年前";
            if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
                return (int)ts.TotalDays / 30 + "月前";
            if ((int)ts.TotalDays == 1)
                return "昨天";
            if ((int)ts.TotalDays == 2)
                return "前天";
            if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
                return (int)ts.TotalDays + "天前";
            if ((int)ts.TotalDays != 0) return dateTime.ToString("yyyy年MM月dd日");
            if ((int)ts.TotalHours != 0)
                return (int)ts.TotalHours + "小时前";
            if ((int)ts.TotalMinutes == 0)
                return "刚刚";
            return (int)ts.TotalMinutes + "分钟前";
        }

        #endregion

        #region Object拓展方法

        /// <summary>
        /// 判断一个对象是否为NULL
        /// </summary>
        /// <param name="input">判断对象</param>
        /// <returns>是否为NULL</returns>
        public static bool IsNull(this object input)
        {
            return input == null;
        }

        #endregion
    }
}
