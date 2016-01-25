using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Object的拓展类
    /// </summary>
    /// <remarks>
    /// FileName: 	ObjectExtension.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/25 16:10:22
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class ObjectExtension
    {
        #region Object拓展方法

        /// <summary>
        /// 判断一个对象是否为NULL
        /// </summary>
        /// <param name="input">判断对象</param>
        /// <returns>是否为NULL</returns>
        public static bool IsNull(this object input)
        {
            return input == null;
        }

        #endregion
    }
}
