using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    /// <summary>
    /// 加密辅助类,该类里面皆为string的拓展方法
    /// </summary>
    /// <remarks>
    /// 2016/1/15 17:36:59 Created By Devin
    /// </remarks>
    public static class EncryptHelper
    {
        #region 不可逆加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">this</param>
        /// <returns>MD5值</returns>
        public static string Md5Encrypt(this string str)
        {
            var md5 = MD5.Create();

            // 计算字符串的散列值
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sbd = new StringBuilder();
            foreach (var item in bytes)
            {
                sbd.Append(item.ToString("x2"));
            }
            return sbd.ToString();
        }

        /// <summary>
        /// 基于MD5和秘钥的加密算法
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="key">秘钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(this string str, string key = "h%12D;](6")
        {
            var md5 = MD5.Create();

            // 计算字符串的散列值
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var eKey = new StringBuilder();
            foreach (var item in bytes)
            {
                eKey.Append(item.ToString("x"));
            }

            // 字符串散列值+密钥再次计算散列值
            bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key + eKey));
            var pwd = new StringBuilder();
            foreach (var item in bytes)
            {
                pwd.Append(item.ToString("x"));
            }

            return pwd.ToString();
        }

        #endregion

        #region 可逆加密

        /// <summary>
        /// 加密(可逆,不固定)
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="key">秘钥</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptStr(this string str, string key = "s?]8!sj;d")
        {
            var des = DES.Create();

            // var timestamp = DateTime.Now.ToString("HHmmssfff");
            var inputBytes = Encoding.UTF8.GetBytes(MixUp(str));
            var keyBytes = Encoding.UTF8.GetBytes(key);
            SHA1 ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyBytes);
            var sKey = new byte[8];
            var sIv = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIv[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIv;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    var ret = new StringBuilder();
                    foreach (var b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }

                    return ret.ToString();
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str">this</param>
        /// <param name="key">秘钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptStr(this string str, string key = "s?]8!sj;d")
        {
            var des = DES.Create();
            var inputBytes = new byte[str.Length / 2];
            for (var x = 0; x < str.Length / 2; x++)
            {
                inputBytes[x] = (byte)System.Convert.ToInt32(str.Substring(x * 2, 2), 16);
            }
            var keyByteArray = Encoding.UTF8.GetBytes(key);
            var ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyByteArray);
            var sKey = new byte[8];
            var sIv = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIv[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIv;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return ClearUp(Encoding.UTF8.GetString(ms.ToArray()));
                }
            }
        }

        #endregion

        #region 混淆与反混淆

        private const int TimestampLength = 36;

        /// <summary>
        /// 混淆字符串
        /// </summary>
        /// <param name="str">this</param>
        /// <returns>混淆后的字符串</returns>
        public static string MixUp(string str)
        {
            // var timestamp = DateTime.Now.ToString(TimestampFormat);
            var timestamp = Guid.NewGuid().ToString();
            var count = str.Length + TimestampLength;
            var sbd = new StringBuilder(count);
            int j = 0;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                if (j < TimestampLength && k < str.Length)
                {
                    if (i % 2 == 0)
                    {
                        sbd.Append(str[k]);
                        k++;
                    }
                    else
                    {
                        sbd.Append(timestamp[j]);
                        j++;
                    }
                }
                else if (j >= TimestampLength)
                {
                    sbd.Append(str[k]);
                    k++;
                }
                else if (k >= str.Length)
                {
                    break;

                    // sbd.Append(timestamp[j]);
                    // j++;
                }
            }

            return sbd.ToString();
        }

        /// <summary>
        /// 反混淆字符串
        /// </summary>
        /// <param name="str">this</param>
        /// <returns>反混淆后的字符串</returns>
        public static string ClearUp(string str)
        {
            var sbd = new StringBuilder();
            int j = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (i % 2 == 0)
                {
                    sbd.Append(str[i]);
                }
                else
                {
                    j++;
                }

                if (j > TimestampLength)
                {
                    sbd.Append(str.Substring(i));
                    break;
                }
            }

            return sbd.ToString();
        }

        #endregion

    }
}
