using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;

namespace Homeinns.Common.Log
{
    /// <summary>
    /// Windows事件日志
    /// </summary>
    public class EventLogRecord : IRecordLog
    {
        /// <summary>
        /// 操作人
        /// </summary>
        private readonly string _userID;
        /// <summary>
        /// 日志名称
        /// </summary>
        private readonly string _logName;
        /// <summary>
        /// 日志源
        /// </summary>
        private readonly string _logSource;

        public EventLogRecord(string logName, string logSource, string userID)
        {
            _logName = logName;
            _logSource = logSource;
            _userID = userID;
            if (!EventLog.SourceExists(logSource))
            {
                EventLog.CreateEventSource(logSource, logName);
            }
        }

        #region IRecordLog 成员
        /// <summary>
        /// 向windows事件日志写入应用程序日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        /// <param name="logType">日志类型</param>
        public void RecordLog(string logText, LogTypeEnum logType)
        {
            string logInfo = String.Format("[UserID]:{0}[DateTime]:{1:yyyy-MM-dd HH:mm:ss,fff}\r\n", _userID, DateTime.Now);
            EventLogEntryType eventLogType = EventLogEntryType.Information;
            switch (logType)
            {
                case LogTypeEnum.Info:
                    logInfo += String.Format("[INFO]:{0}\r\n", logText);
                    logInfo = "-------------操作日志---------------\r\n" + logInfo;
                    eventLogType = EventLogEntryType.Information;
                    break;
                case LogTypeEnum.Error:
                    logInfo += String.Format("[Error]:{0}\r\n", logText);
                    logInfo = "-------------错误日志---------------\r\n" + logInfo;
                    eventLogType = EventLogEntryType.Error;
                    break;
            }
            using (EventLog eventLog = new EventLog(_logName) { Source =  _logSource + "Source" })
            {
                if (eventLog != null)
                eventLog.WriteEntry(logInfo, eventLogType);
            }            
        }
        #endregion
    }
}
