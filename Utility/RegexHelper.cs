using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility
{
    /// <summary>
    /// 正则表达式的辅助类
    /// </summary>
    /// <remarks>
    /// FileName: 	RegexHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/22 16:14:40
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public class RegexHelper
    {
        #region 判断功能

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">匹配模式</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        #endregion

        #region 替换功能

        /// <summary>
        /// 在输入字符串(input)内,将正则表达式(pattern)匹配到的所有字符串替换为替换字符串(replacement)
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="replacement">替换字符串</param>
        /// <returns>替换完成后的字符串</returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// 在输入字符串(input)内,将正则表达式(pattern)匹配到的所有字符串替换为替换字符串(replacement)
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="replacement">替换字符串</param>
        /// <param name="options">匹配模式</param>
        /// <returns>替换完成后的字符串</returns>
        public static string Replace(string input, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        #endregion

        #region 提取功能

        /// <summary>
        /// 获取在字符串中用正则表达式匹配到的第一个值
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>匹配到的第一个值</returns>
        public static string Match(string input, string pattern)
        {
            string result = string.Empty;

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                result = match.Value;
            }
            return result;
        }

        /// <summary>
        /// 获取在字符串中用正则表达式匹配到的第一个值
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">匹配选项</param>
        /// <returns>匹配到的第一个值</returns>
        public static string Match(string input, string pattern, RegexOptions options)
        {
            string result = string.Empty;

            Match match = Regex.Match(input, pattern, options);
            if (match.Success)
            {
                result = match.Value;
            }
            return result;
        }

        /// <summary>
        /// 获取在字符串中所有匹配到的值
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>匹配结果</returns>
        public static List<string> Matches(string input, string pattern)
        {
            List<string> result = new List<string>();
            MatchCollection matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
            {
                result.AddRange(from Match match in matches select match.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取在字符串中所有匹配到的值
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">匹配选项</param>
        /// <returns>匹配结果</returns>
        public static List<string> Matches(string input, string pattern, RegexOptions options)
        {
            List<string> result = new List<string>();
            MatchCollection matches = Regex.Matches(input, pattern, options);
            if (matches.Count > 0)
            {
                result.AddRange(from Match match in matches select match.Value);
            }
            return result;
        }

        /// <summary>
        /// 根据组索引从被正则表达式匹配到的项中提取组
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="groupIndex">组对应的索引</param>
        /// <returns>提取到的组的值</returns>
        public static string GetGroup(string input,string pattern,int groupIndex)
        {
            return Regex.Match(input,pattern).Groups[groupIndex].Value;
        }

        /// <summary>
        /// 根据组索引 从被正则表达式匹配到的项中 提取组
        /// </summary>
        /// <param name="input">要匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="groupIndex">组对应的索引</param>
        /// <param name="options">匹配选项</param>
        /// <returns>提取到的组的值</returns>
        public static string GetGroup(string input, string pattern, int groupIndex, RegexOptions options)
        {
            return Regex.Match(input, pattern,options).Groups[groupIndex].Value;
        }

        /// <summary>
        /// 提取被正则表达式匹配到的项的所有组
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="groupIndex"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<string> GetGroups(string input, string pattern, int groupIndex, RegexOptions options)
        {
            //return (from Group g in Regex.Match(input, pattern, options).Groups select g.Value).ToList();
            return (from Match m in Regex.Matches(input, pattern, options) select m.Groups[groupIndex].Value).ToList();
        }

        /// <summary>
        /// 提取被正则表达式匹配到的项的所有组
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="groupIndex"></param>
        /// <returns></returns>
        public static List<string> GetGroups(string input, string pattern, int groupIndex)
        {
            return (from Match m in Regex.Matches(input, pattern) select m.Groups[groupIndex].Value).ToList();
        }

        #endregion

        #region 分割

        /// <summary>
        /// 根据正则表达式分割字符串
        /// </summary>
        /// <param name="input">要分割的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">匹配模式</param>
        /// <returns>分割后的string</returns>
        public static string[] Split(string input, string pattern, RegexOptions options)
        {
            return Regex.Split(input, pattern, options);
        }

        /// <summary>
        /// 根据正则表达式分割字符串
        /// </summary>
        /// <param name="input">要分割的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>分割后的string</returns>
        public static string[] Split(string input, string pattern)
        {
            return Regex.Split(input, pattern);
        }

        #endregion
    }
}
