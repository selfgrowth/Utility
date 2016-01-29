using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Utility
{
    /// <summary>
    /// Cache的操作类
    /// </summary>
    /// <remarks>
    /// FileName: 	CacheHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/27 9:44:18
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class CacheHelper
    {
        /// <summary>
        /// key前缀,防止和其他部门或者模块冲突
        /// </summary>
        private static readonly string CacheKeyPrefix = string.Empty;

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
        /// 获取对应的Cache
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static object Get(string key)
        {
            return Context.Cache[key];
        }

        /// <summary>
        /// 获取对应的Cache,并返回指定的类型
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static T Get<T>(string key)
            where T : class
        {
            object value =Get(key);
            return value as T;
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置具有依赖项和到期策略的Cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="dependencies">依赖项</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="slidingExpiration">滑动的时间差</param>
        public static void Set(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Context.Cache.Insert(key,value,dependencies,absoluteExpiration,slidingExpiration);
        }
        /// <summary>
        /// 设置有绝对过期时间的Cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            Set(key, value, null, absoluteExpiration, TimeSpan.Zero);
        }
        /// <summary>
        /// 设置有滑动过期时间的Cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="slidingExpiration">滑动过期时间</param>
        public static void Set(string key, object value, TimeSpan slidingExpiration)
        {
            Set(key, value, null, DateTime.MaxValue, slidingExpiration);
        }

        /// <summary>
        /// 设置依赖于数据库的Cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="databaseEntryName">在应用程序的 Web.config 文件的数据库元素中定义的数据库的名称。</param>
        /// <param name="tableName">关联的数据库表的名称</param>
        public static void SetDependentSqlServer(string key, object value, string databaseEntryName, string tableName)
        {
            Set(key, value, new SqlCacheDependency(databaseEntryName, tableName), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        #endregion

        #region Remove

        /// <summary>
        /// 移除指定的Cache
        /// </summary>
        /// <param name="key">要移除的Cache的key</param>
        public static void Remove(string key)
        {
            Context.Cache.Remove(key);
        }

        /// <summary>
        /// 移除所有的Cache
        /// </summary>
        public static void Clear()
        {
            var cacheEnum = Context.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                Context.Cache.Remove(cacheEnum.Key.ToString());
            }
        }

        #endregion

    }
}
