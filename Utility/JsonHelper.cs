using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utility
{
    /// <summary>
    /// 操作Json的辅助类
    /// </summary>
    /// <remarks>
    /// FileName: 	JsonHelper.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/21 00:08:26
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class JsonHelper
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        /// <summary>
        /// 将一个对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>序列化后的Json字符串</returns>
        public static string Serialize(object obj)
        {
            string result;
            using (StringWriter sw = new StringWriter())
            {
                JsonSerializer.Serialize(new JsonTextWriter(sw), obj);
                result = sw.GetStringBuilder().ToString();
            }
            return result;
        }

        /// <summary>
        /// 将一个Json字符串反序列化成Object对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <returns>反序列化后的Object对象</returns>
        public static object Deserialize(string json)
        {
            using (var reader = new StringReader(json))
            {
                return JsonSerializer.Deserialize(new JsonTextReader(reader));
            }
        }

        /// <summary>
        /// 将一个Json字符串反序列化成指定类型的对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(string json)where T:class
        {
            return JsonSerializer.Deserialize(new JsonTextReader(new StringReader(json))) as T;
        }

        /// <summary>
        /// 将一个json字符串转换成JArray对象
        /// </summary>
        /// <param name="json">要转换的Json字符串</param>
        /// <returns>转换完的JArray对象</returns>
        public static JArray GetJArray(string json)
        {
            return JArray.Parse(json);
        }

        /// <summary>
        /// 将一个json字符串转换成JObject对象
        /// </summary>
        /// <param name="json">要转换的Json字符串</param>
        /// <returns>转换完的JObject对象</returns>
        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        /// <summary>
        /// 将json字符串转换成JToken对象,然后执行对应的操作
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <param name="expression">需要对JToken执行的操作</param>
        /// <returns></returns>
        public static string Select(string json,Func<JToken,string> expression)
        {
            JToken token = JToken.Parse(json);
            return expression(token);
        }
    }
}
