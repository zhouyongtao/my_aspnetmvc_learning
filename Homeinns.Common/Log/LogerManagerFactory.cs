using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homeinns.Common.Log
{
    public static class LogerManagerFactory
    {
        private static LogerManager _loger;
        public static LogerManager Create()
        {
            if (_loger == null)
            {
                _loger = new LogerManager() { IsRecord = LogSetting.GetValueByKey("LogOpt") == "true" };
                if (LogSetting.GetValueByKey("logType").Contains("TextLog"))
                {
                    //配置日志记录路径(修改路径在此修改)
                    _loger.LogerList.Add(new TextLogRecord(System.Environment.UserName, AppDomain.CurrentDomain.BaseDirectory));
                }
                else if (LogSetting.GetValueByKey("logType").Contains("EventLog"))
                {
                    _loger.LogerList.Add(new EventLogRecord("ILog", "Irving", System.Environment.UserName));
                }
            }
            return _loger;
        }
    }
}
