using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NRainel.Log.ILog
{
    /*
     NickName:Irving
     Email:zhouyontao@outlook.com
     Date:2012年5月5日11:45:26
     
            配置
            <add key="logType" value="TextLog"/>
            <add key="OPLog" value="true"/>
     */
    /// <summary>
    /// AppSetting操作类
    /// </summary>
    public static class AppSetting
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
