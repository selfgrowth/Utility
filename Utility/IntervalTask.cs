using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Utility
{
    /// <summary>
    /// 计划任务辅助类 
    /// </summary>
    /// <remarks>
    /// FileName: 	IntervalTask.cs
    /// CLRVersion: 4.0.30319.18444
    /// Author: 	Devin
    /// DateTime: 	2016/1/22 15:20:50
    /// GitHub:		https://github.com/v5bep7/Utility
    /// </remarks>
    public static class IntervalTask
    {
        private static readonly Dictionary<string, Timer> TimerValues = new Dictionary<string, Timer>();

        /// <summary>
        /// 开始一个任务,返回一个标识该任务的markId
        /// </summary>
        /// <param name="elapsed">需要执行的任务</param>
        /// <param name="interval">执行的间隔时间.单位: 毫秒</param>
        /// <param name="autoReset">是否是周期性任务,还是一次性任务.默认是周期性任务</param>
        /// <returns></returns>
        public static string Start(Action elapsed, int interval, bool autoReset = true)
        {
            if (elapsed == null)
            {
                throw new ArgumentNullException("elapsed");
            }
            interval = interval > 0 ? interval : 1000;
            var timer = new Timer();
            //timer.Elapsed += new ElapsedEventHandler(elapsed);
            timer.Elapsed += new ElapsedEventHandler((sender, e) =>
            {
                elapsed();
            });
            timer.Interval = interval;
            timer.AutoReset = autoReset;
            timer.Start();
            string markId = Guid.NewGuid().ToString();
            TimerValues.Add(markId, timer);
            return markId;
        }

        /// <summary>
        /// 通过MarkId停止任务
        /// </summary>
        /// <param name="markId"></param>
        public static void Stop(string markId)
        {
            if (TimerValues.Keys.Contains(markId))
            {
                TimerValues[markId].Stop();
                TimerValues[markId].Close();
                TimerValues[markId].Dispose();
                TimerValues.Remove(markId);
            }
        }

        /// <summary>
        /// 通过markId获取Timer对象
        /// </summary>
        /// <param name="markId"></param>
        /// <returns></returns>
        public static Timer GetTimer(string markId)
        {
            if (TimerValues.Keys.Contains(markId))
            {
                return TimerValues[markId];
            }
            return null;
        }


    }
}
