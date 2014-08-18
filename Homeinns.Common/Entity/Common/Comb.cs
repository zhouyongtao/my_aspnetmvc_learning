using System;
using System.Collections.Generic;

namespace Homeinns.Common.Entity.Common
{
    public class Comb
    {
        /// <summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static Guid NewComb()
        {
            var guidArray = System.Guid.NewGuid().ToByteArray();
            var baseDate = new DateTime(1900, 1, 1);
            var now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        /// <summary>
        /// 从 SQL SERVER 返回的 GUID 中生成时间信息
        /// </summary>
        public static DateTime GetDateFromComb(System.Guid guid)
        {
            var baseDate = new DateTime(1900, 1, 1);
            var daysArray = new byte[4];
            var msecsArray = new byte[4];
            var guidArray = guid.ToByteArray();

            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            var days = BitConverter.ToInt32(daysArray, 0);
            var msecs = BitConverter.ToInt32(msecsArray, 0);

            var date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }
    }
}
