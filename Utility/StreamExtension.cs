using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// 文件流的拓展方法
    /// </summary>
    /// <remarks>
    /// FileName: 	StreamExtension.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/22 14:27:05
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class StreamExtension
    {

        #region 获取数据流的内容MD5

        /// <summary>
        /// 获取数据流的内容MD5
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>MD5</returns>
        public static string GetMD5(this Stream stream)
        {
            var oMd5Hasher = new MD5CryptoServiceProvider();
            var arrbytHashValue = oMd5Hasher.ComputeHash(stream);

            #region 重构前

            //// 由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            //string strHashData = BitConverter.ToString(arrbytHashValue);

            //// 替换-
            //return strHashData.Replace("-", string.Empty).ToLower();

            #endregion

            var sbd = new StringBuilder();
            foreach (var item in arrbytHashValue)
            {
                sbd.Append(item.ToString("x2"));
            }
            return sbd.ToString();

        }

        #endregion

    }
}
