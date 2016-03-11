using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Utility
{
    /// <summary>
    /// Http请求工具类.该类不能静态类,直接使用即可
    /// </summary>
    /// <remarks>
    /// FileName: 	HttpHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/21 21:06:48
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class HttpClient
    {
        #region Get请求

        /// <summary>
        /// 以GET方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="queryString">参数</param>
        /// <returns>响应内容</returns>
        public static string Get(string url, string queryString)
        {
            return Get(url, queryString, Encoding.UTF8);
        }

        /// <summary>
        /// 以GET方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="queryString">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns>响应内容</returns>
        public static string Get(string url, string queryString, Encoding encoding)
        {
            string result = string.Empty;
            queryString = string.IsNullOrEmpty(queryString) ? string.Empty : "?" + queryString;
            HttpWebRequest request = WebRequest.Create(string.Format("{0}{1}", url, queryString)) as HttpWebRequest;
            if (request == null)
            {
                return result;
            }
            request.Timeout = 19600;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

            HttpWebResponse response = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, encoding))
                    {
                        if (1 != reader.Peek())
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 以GET方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="data">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns>响应内容</returns>
        public static string Get(string url, object data, Encoding encoding)
        {
            var queryString = string.Empty;
            if (data != null)
            {
                queryString = string.Join("&",
                   from p in data.GetType().GetProperties()
                   select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString()));
            }
            return Get(url, queryString, encoding);
        }

        /// <summary>
        /// 以GET方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="data">参数</param>
        /// <returns>响应内容</returns>
        public static string Get(string url, object data)
        {
            return Get(url, data, Encoding.UTF8);
        }

        /// <summary>
        /// 以Get方式请求Url,返回响应数据流
        /// </summary>
        /// <param name="url">请求的Url</param>
        /// <param name="queryString">参数</param>
        /// <returns>响应的流</returns>
        public static Stream GetStream(string url, string queryString)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //if (data.Contains("{"))
                //    data = data.TrimStart('{').TrimEnd('}').Replace(":", "=").Replace(",", "&").Replace(" ", string.Empty);
                queryString = string.IsNullOrEmpty(queryString) ? string.Empty : "?" + queryString;
                request = WebRequest.Create(string.Format("{0}{1}", url, queryString)) as HttpWebRequest;
                if (request == null) return null;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ServicePoint.ConnectionLimit = 300;
                request.Referer = url;
                request.Accept =
                    "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
                request.UserAgent =
                    "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                return response == null ? null : response.GetResponseStream();
            }
            finally
            {
                if (request != null)
                    request.Abort();

                if (response != null)
                    response.Close();
            }
        }

        /// <summary>
        /// 以Get方式请求Url,返回响应数据流
        /// </summary>
        /// <param name="url">请求的Url</param>
        /// <param name="data">参数</param>
        /// <returns>响应的流</returns>
        public static Stream GetStream(string url, object data)
        {
            var queryString = string.Empty;
            if (data != null)
            {
                queryString = string.Join("&",
                              from p in data.GetType().GetProperties()
                              select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString()));
            }
            return GetStream(url, queryString);
        }

        #endregion

        #region Post请求

        /// <summary>
        /// 以POST方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="queryString">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns>响应内容</returns>
        public static string Post(string url, string queryString, Encoding encoding)
        {
            //结果
            string result = string.Empty;
            //参数
            byte[] buffer = encoding.GetBytes(queryString);
            //设置请求头
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                return result;
            }
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            //设置请求体
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(buffer, 0, buffer.Length);
            }

            HttpWebResponse response = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                if (response == null)
                {
                    return result;
                }
                using (Stream readerStream = response.GetResponseStream())
                {
                    if (readerStream != null)
                    {
                        using (StreamReader reader = new StreamReader(readerStream, encoding))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }

            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 以POST方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="queryString">参数</param>
        /// <returns>响应内容</returns>
        public static string Post(string url, string queryString)
        {
            return Post(url, queryString, Encoding.UTF8);
        }

        /// <summary>
        /// 以POST方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="data">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns>响应内容</returns>
        public static string Post(string url, object data, Encoding encoding)
        {
            string queryString = string.Empty;
            if (data!=null)
            {
                queryString = string.Join("&",
                from p in data.GetType().GetProperties() select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString()));
            }
            return Post(url, queryString, encoding);
        }

        /// <summary>
        /// 以POST方式请求指定Url
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="data">参数</param>
        /// <returns>响应内容</returns>
        public static string Post(string url, object data)
        {
            return Post(url, data, Encoding.UTF8);
        }

        #endregion
    }
}
