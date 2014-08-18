using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRainel.Log.ILog
{
    public static class LogerManagerFactory
    {
        // 单例模式
        private static LogerManager _loger;
        public static LogerManager Create()
        {
            if (_loger == null)
            {
                _loger = new LogerManager();
                _loger.RecordOPLog = AppSetting.GetValueByKey("OPLog") == "true";

                if (AppSetting.GetValueByKey("logType").Contains("TextLog"))
                {
                    //配置记录路径(可修改)
                    _loger.LogerList.Add(new TextLoger("", AppDomain.CurrentDomain.BaseDirectory));
                }

                if (AppSetting.GetValueByKey("logType").Contains("EventLog"))
                {
                    _loger.LogerList.Add(new WinEventLoger("ILog", "Irving", System.Environment.UserName));
                }
            }
            return _loger;
        }
    }
}
