using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Utility
{
    /// <summary>
    /// Cookie操作类
    /// </summary>
    /// <remarks>
    /// FileName: 	CookieHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/25 17:42:09
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class CookieHelper
    {
        /// <summary>
        /// key前缀,防止和其他部门或者模块冲突
        /// </summary>
        private static readonly string CookieKeyPrefix = string.Empty;

        /// <summary>
        /// 当前的HttpContext对象
        /// </summary>
        private static HttpContext Context
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null)
                    throw new NullReferenceException("无法获取当前的HttpContext对象!");
                return context;
            }
        }

        #region Get
        /// <summary>
        /// 通过key获取Cookie
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Cookie的值</returns>
        public static string Get(string key)
        {
            key = CookieKeyPrefix + key;
            var value = string.Empty;
            var cookie = Context.Request.Cookies[key];
            if (cookie != null)
                value = HttpUtility.UrlDecode(cookie.Value);
            return value;
        }

        /// <summary>
        /// 通过Key获取Cookie的值,并转换为对应的对象
        /// </summary>
        /// <typeparam name="T">要获取的类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>Cookie里存放的对象</returns>
        public static T Get<T>(string key) where T : class
        {
            var value = Get(key);
            if (string.IsNullOrEmpty(value)) return null;
            return JsonConvert.DeserializeObject(value) as T;
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置Cookie的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expires">cookie过期时间</param>
        /// <param name="domain">cookie的域</param>
        /// <param name="path">cookie的限制地址</param>
        /// <param name="secure">该cookie是否只适用于HTTPS</param>
        public static void Set(string key, string value, DateTime? expires = null, string domain = "", string path = "/", bool secure = false)
        {
            key = CookieKeyPrefix + key;
            var cookie = new HttpCookie(key)
            {
                Value = HttpUtility.UrlEncode(value),       // Cookie值
                Domain = domain,                            // 作用域名
                Path = path,                                // 作用路径
                Secure = secure,                            // 是否只作用于HTTPS
            };
            if (expires != null)
                cookie.Expires = (DateTime)expires;

            var cookies = Context.Response.Cookies;
            cookies.Remove(key);
            cookies.Add(cookie);
        }

        /// <summary>
        /// 设置Cookie的值,该值可以是一个object
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expires">cookie过期时间</param>
        /// <param name="domain">cookie的域</param>
        /// <param name="path">cookie的限制地址</param>
        /// <param name="secure">该cookie是否只适用于HTTPS</param>
        public static void Set(string key, object value, DateTime? expires = null, string domain = "", string path = "/", bool secure = false)
        {
            string val = value == null ? string.Empty : JsonConvert.SerializeObject(value);
            Set(key, val, expires, domain, path, secure);
        }


        #endregion

        #region Remove

        /// <summary>
        /// 移除指定cookie
        /// </summary>
        /// <param name="key">要移除的cookie的key</param>
        public static void Remove(string key)
        {
            key =CookieKeyPrefix + key;
            HttpCookie cookie = Context.Request.Cookies[key];
            if (cookie == null) return;

            cookie.Expires = DateTime.Now.AddDays(-7);
            Context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 移除客户端所有Cookie 
        /// </summary>
        public static void Clear()
        {
            HttpCookieCollection cookies = Context.Request.Cookies;
            if (cookies.Count>0)
            {
                foreach (HttpCookie cookie in cookies)
                {
                    cookie.Expires = DateTime.Now.AddDays(-7);
                    Context.Response.Cookies.Add(cookie);
                }
            }
        }

        #endregion
    }
}
