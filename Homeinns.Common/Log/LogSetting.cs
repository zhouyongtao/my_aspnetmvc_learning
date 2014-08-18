using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homeinns.Common.Log
{
    public class LogSetting
    {
        /// <summary>
        /// 根据key得到Value值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
