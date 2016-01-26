using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility
{
    /// <summary>
    /// Session操作类
    /// </summary>
    /// <remarks>
    /// FileName: 	SessionHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/26 9:54:42
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class SessionHelper
    {
        /// <summary>
        /// key前缀,防止和其他部门或者模块冲突
        /// </summary>
        private static readonly string SessionKeyPrefix = string.Empty;

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
        /// 获取Session的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static object Get(string key)
        {
            key = SessionKeyPrefix + key;
            var value = Context.Session[key];
            return value;
        }

        /// <summary>
        /// 获取Session的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static T Get<T>(string key) where T : class
        {
            key = SessionKeyPrefix + key;
            var value = Context.Session[key];
            return value as T;
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置Session的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void Set(string key, object value)
        {
            key = SessionKeyPrefix + key;
            Context.Session.Add(key, value);
        }

        #endregion

        #region Remove

        /// <summary>
        /// 移除指定的Session值
        /// </summary>
        /// <param name="key">key</param>
        public static void Set(string key)
        {
            key = SessionKeyPrefix + key;
            Context.Session.Remove(key);
        }

        /// <summary>
        /// 移除所有Session
        /// </summary>
        public static void Clear()
        {
            Context.Session.RemoveAll();
        }

        #endregion
    }
}
