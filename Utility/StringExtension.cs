using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// 字符串的拓展方法
    /// </summary>
    /// <remarks>
    /// FileName: 	StringExtension.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/22 13:15:25
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class StringExtension
    {
        #region 判断功能

        #region 判断是否是合法的邮箱

        /// <summary>
        /// 判断该字符串是否是合法的邮箱
        /// </summary>
        /// <param name="str">当前字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion

        #region 判断是否是合法的IP地址

        /// <summary>
        /// 判断该字符串是否是合法的IP地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIpAddress(this string input)
        {
            return Regex.IsMatch(input, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion

        #region 判断是否是合法的手机号

        /// <summary>
        /// 判断该字符串是否是合法的手机号(国内)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinaPhoneNumber(this string input)
        {
            return Regex.IsMatch(input, @"^(0?(13|14|15|18)[0-9]{9})$");
        }

        #endregion

        #region 判断是否是合法的身份证号

        /// <summary>
        /// 判断该字符串是否是合法的身份证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string input)
        {
            return Regex.IsMatch(input, @"^(([1-9]{2}\d{15}[0-9xX])|([1-9]{2}\d{13}))$");
        }

        #endregion

        #region 判断是否是合法的日期格式

        /// <summary>
        /// 判断该字符串是否为合法的日期格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dt;
            return DateTime.TryParse(input, out dt);
        }

        #endregion

        #region 判断是否是为数字

        /// <summary>
        /// 判断该字符串是否是合法的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            return !string.IsNullOrEmpty(str) && Regex.IsMatch(str, @"^-?\d+$");
        }

        #endregion

        #region 判断是否为Url地址

        /// <summary>
        /// 判断该字符串是否是合法的Url地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(this string str)
        {
            return Regex.IsMatch(str, @"(http(s)?|ftp)://[a-zA-Z0-9\.\-]+\.([a-zA-Z]{2,4})(:\d+)?(/[a-zA-Z0-9\.\-~!@#$%^&*+?:_/=<>]*)?");
        }

        #endregion

        #region 判断字符串是否为空或者Null

        /// <summary>
        /// 判断该字符串是否是为空或者Null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        #endregion
        #endregion

        #region 编码功能

        #region Html编码

        /// <summary>
        /// 对该字符串进行Html编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string content)
        {
            return HttpUtility.HtmlEncode(content);
        }

        #endregion

        #region Html解码

        /// <summary>
        /// 对该字符串进行Html解码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string content)
        {
            return HttpUtility.HtmlDecode(content);
        }

        #endregion

        #region Url编码

        /// <summary>
        /// 对该字符串进行Url编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string UrlEncode(this string content)
        {
            return HttpUtility.UrlEncode(content);
        }

        #endregion

        #region Url解码

        /// <summary>
        /// 对该字符串进行Url解码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string UrlDecode(this string content)
        {
            return HttpUtility.UrlDecode(content);
        }

        #endregion

        #region 去除Html标签

        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns>返回移除了Html标签的字符串</returns>
        public static string TrimHtml(this string html)
        {
            // 删除脚本和嵌入式CSS   
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<style[^>]*?>.*?</style>", string.Empty, RegexOptions.IgnoreCase);

            // 删除HTML   
            var regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            html = regex.Replace(html, string.Empty);
            html = Regex.Replace(html, @"<(.[^>]*)>", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);

            //Html解码
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);

            return html.Replace("<", string.Empty).Replace(">", string.Empty).Replace("\r\n", string.Empty);
        }

        #endregion

        #endregion

        #region 字符串风格转换

        #region 转换成帕斯卡命名风格

        /// <summary>
        /// 将该字符串转成帕斯卡风格.例如:hello world -> HelloWorld
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (str.Length < 2)
            {
                return str.ToUpper();
            }

            string[] words = str.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var result = new StringBuilder();
            foreach (string word in words)
            {
                result.Append(word.Substring(0, 1).ToUpper()).Append(word.Substring(1));
            }

            return result.ToString();
        }

        #endregion

        #region 转换成骆驼命名风格

        /// <summary>
        /// 将该字符串转成骆驼风格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (str.Length < 2)
            {
                return str.ToLower();
            }
            string[] words = str.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder(words[0].ToLower());
            for (int i = 1; i < words.Length; i++)
            {
                result.Append(words[i].Substring(0, 1).ToUpper()).Append(words[i].Substring(1).ToLower());
            }
            return result.ToString();
        }

        #endregion

        #region 转换成下划线格式

        /// <summary>
        /// 将该字符串中的单词用下划线分割
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToUnderlineCase(this string input)
        {
            // 帕斯卡格式的单词边界的正则表达式
            Regex pascalCaseWordBoundaryRegex = new Regex(@"
                (?# word to word, number or acronym)
                (?<=[a-z])(?=[A-Z0-9])|
                (?# number to word or acronym)
                (?<=[0-9])(?=[A-Za-z])|
                (?# acronym to number)
                (?<=[A-Z])(?=[0-9])|
                (?# acronym to word)
                (?<=[A-Z])(?=[A-Z][a-z])
                ", RegexOptions.IgnorePatternWhitespace);

            var result = pascalCaseWordBoundaryRegex
                .Split(input)                           //根据单词边界分割单词
                .Select(word =>                         //转成小写
                    word.ToCharArray().All(char.IsUpper) && word.Length > 1
                        ? word
                        : word.ToLower())
                .Aggregate((res, word) => res + " " + word);    //用" "连接起来

            // result = Char.ToUpper(result[0]) + result.Substring(1, result.Length - 1);
            // return result.ToLower();//.Replace(" i ", " I "); // I is an exception
            // if (input == null) return input;
            // if (input.Length < 2) return input.ToLower();

            //// Split the string into words.
            //根据" "分割,转小写
            var words = result.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower());
            //用"_"连接,返回
            return string.Join("_", words).ToLower();
        }

        #endregion
        #endregion

        #region 将字符串中的占位符替换成指定数组中的字符串表现形式

        /// <summary>
        /// 将字符串中的占位符替换成指定数组中的字符串表现形式
        /// </summary>
        /// <param name="str">该字符串</param>
        /// <param name="args">参数对应的值</param>
        /// <returns></returns>
        public static string Formatter(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        #endregion
    }
}
