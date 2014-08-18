using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NRainel.Log.ILog
{
    /// <summary>
    /// windows事件日志
    /// </summary>
    public class WinEventLoger : LogerBase
    {
        private readonly string _logName;
        private readonly string _souce;
        private readonly string _userID;

        public WinEventLoger(string logName, string source, string userID)
        {
            _logName = logName;
            _souce = source;
            _userID = userID;

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }
        }

        /// <summary>
        /// 向windows事件日志写入应用程序日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        /// <param name="logType">日志类型</param>
        public override void RecordLog(string logText, LogTypeEnum logType)
        {
            string str = String.Format("[UserID]:{0}[DateTime]:{1:yyyy-MM-dd HH:mm:ss,fff}\r\n", _userID, DateTime.Now);
            EventLogEntryType eventLogType = EventLogEntryType.Information;
            switch (logType)
            {
                case LogTypeEnum.OPLog:
                    str += String.Format("[OP]:{0}\r\n", logText);
                    str = "-------------操作日志---------------\r\n" + str;
                    eventLogType = EventLogEntryType.Information;
                    break;
                case LogTypeEnum.ErrorLog:
                    str += String.Format("[Error]:{0}\r\n", logText);
                    str = "-------------错误日志---------------\r\n" + str;
                    eventLogType = EventLogEntryType.Error;
                    break;
            }

            using (EventLog eventLog = new EventLog(_logName) { Source = _souce })
            {
                eventLog.WriteEntry(str, eventLogType);
            }
        }
    }
}
