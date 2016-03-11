using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Utility
{
    /// <summary>
    /// 配置信息类,该类为静态类,直接使用即可.
    /// 配置的key不区分大小写
    /// </summary>
    /// <remarks>
    /// FileName: 	Setting.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/24 21:49:44
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class Setting
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private const string SyncObject = "setting_lock_helper";

        /// <summary>
        /// 信息字典(配置仓储)
        /// </summary>
        public static Dictionary<string, string> Settings { get; private set; }

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static Setting()
        {
            Settings = new Dictionary<string, string>();
        }

        #region 设置信息字典
        /// <summary>
        /// 设置配置信息.
        /// 往配置仓储添加配置,如果仓储里面存在该key,则覆盖该key对应的值
        /// </summary>
        /// <param name="data">要往字典里面添加配置的信息集合</param>
        public static void Set(IDictionary<string, string> data)
        {
            lock (SyncObject)
            {
                foreach (var item in data)
                {
                    var key = item.Key.ToLower();
                    if (Settings.ContainsKey(key))
                        Settings[key] = item.Value;
                    else
                        Settings.Add(key, item.Value);
                }
            }
        }

        /// <summary>
        /// 设置配置信息
        /// 往配置仓储添加配置,如果仓储里面存在改key,则覆盖该key对应的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, string value)
        {
            key = key.ToLower();
            lock (SyncObject)
            {
                if (Settings.ContainsKey(key))
                    Settings[key] = value;
                else
                    Settings.Add(key, value);
            }
        }
        #endregion

        #region 移除配置信息

        /// <summary>
        /// 根据key移除配置仓储里面的配置信息
        /// </summary>
        /// <param name="key">要移除的key</param>
        public static void Remove(string key)
        {
            key = key.ToLower();
            if (!Settings.ContainsKey(key)) return;
            lock (SyncObject)
            {
                if (Settings.ContainsKey(key))
                {
                    Settings.Remove(key);
                }
            }
        }

        /// <summary>
        /// 移除配置仓储里面的多个配置信息
        /// </summary>
        /// <param name="keys">要移除的key的集合</param>
        public static void Remove(params string[] keys)
        {
            lock (SyncObject)
            {
                foreach (var key in keys.Where(key => Settings.ContainsKey(key)))
                {
                    Settings.Remove(key.ToLower());
                }
            }
        }
        #endregion

        #region 获取配置信息
        /// <summary>
        /// 获取字符串类型设置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static string GetString(string key, string def = "")
        {
            key = key.ToLower();
            return Settings != null && Settings.ContainsKey(key) ? Settings[key] : def;
        }

        /// <summary>
        /// 获取bool型类型配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static bool GetBoolean(string key, bool def = default(bool))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            bool.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取char类型配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static char GetChar(string key, char def = default(char))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            char.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取decimal类型的配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static decimal GetDecimal(string key, decimal def = default(decimal))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            decimal.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取double类型的配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static double GetDouble(string key, double def = default(double))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            double.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取float类型的配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static float GetFloat(string key, float def = default(float))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            float.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取byte类型的配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static byte GetByte(string key, byte def = default(byte))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            byte.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取short类型的配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static short GetShort(string key, short def = default(short))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            short.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取int类型的配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static int GetInt(string key, int def = default(int))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            int.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取long类型的配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static long GetLong(string key, long def = default(long))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            long.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取DataTime类型配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static DateTime GetDateTime(string key, DateTime def = default(DateTime))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            DateTime.TryParse(res, out def);
            return def;
        }

        /// <summary>
        /// 获取Guid类型配置信息值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public static Guid GetGuid(string key, Guid def = default(Guid))
        {
            var res = GetString(key);
            if (res.Length == 0)
            {
                return def;
            }

            Guid.TryParse(res, out def);
            return def;
        }
        #endregion

        #region 持久化

        /// <summary>
        /// 将配置信息保存到文件
        /// </summary>
        /// <param name="path">完整的文件名(路径+文件名)</param>
        public static void Save(string path)
        {
            XDocument doc = new XDocument();
            doc.Declaration = new XDeclaration("1.0", "utf-8", null);
            XElement root = new XElement("Settings");
            foreach (var setting in Settings)
            {
                //节点
                XElement ele = new XElement("setting");
                //属性
                XAttribute keyAtr = new XAttribute("key",setting.Key);
                XAttribute valueAtr = new XAttribute("value", setting.Value);
                ele.Add(keyAtr);
                ele.Add(valueAtr);
                root.Add(ele);
            }
            doc.Add(root);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            doc.Save(path);
        }

        /// <summary>
        /// 从文件加载配置信息
        /// </summary>
        /// <param name="path">完整的文件名(路径+文件名)</param>
        public static void Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("文件未找到", path);
            }
            XDocument doc = null;
            using (Stream stream = File.OpenRead(path))
            {
                doc = XDocument.Load(stream);
            }
            var root = doc.Root;
            var elements =  root.Elements();
            foreach (var element in elements)
            {
                lock (SyncObject)
                {
                    if (!Settings.ContainsKey(element.Attribute("key").Value))
                    {
                        Settings.Add(element.Attribute("key").Value,element.Attribute("value").Value);
                    }
                    else
                    {
                        Settings[element.Attribute("key").Value] = element.Attribute("value").Value;
                    }
                }
            }
        }

        #endregion


    }
}
