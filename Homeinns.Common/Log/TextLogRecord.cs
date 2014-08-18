using System;
using System.Collections.Generic;
using System.IO;
using Homeinns.Common.Base;

namespace Homeinns.Common.Log
{
    /// <summary>
    /// 文本日志
    /// </summary>
    public class TextLogRecord:IRecordLog
    {
        /// <summary>
        /// 操作人
        /// </summary>
        private readonly string _userLogID;
        /// <summary>
        /// 操作日志路径
        /// </summary>
        private readonly string _infoLogFilePath;
        /// <summary>
        /// 错误日志路径
        /// </summary>
        private readonly string _errorLogFilePath;

        public TextLogRecord(string userLogID, string logFilePath)
        {
            _userLogID = userLogID;
            _infoLogFilePath = logFilePath + "InfoLog\\";
            _errorLogFilePath = logFilePath + "ErrorLog\\";
            FileUtil.CheckPath(_infoLogFilePath);
            FileUtil.CheckPath(_errorLogFilePath);
        }

        #region IRecordLog 成员
        public void RecordLog(string logText, LogTypeEnum logType)
        {
            string logInfo = String.Format("[UserLogID]:{0}[DateTime]:{1:yyyy-MM-dd HH:mm:ss,fff}\r\n", _userLogID, DateTime.Now);
            string fileName = string.Empty;
            switch (logType)
            {
                case LogTypeEnum.Info:
                    logInfo += String.Format("[OP]:{0}\r\n", logText);
                    logInfo = "-------------操作日志---------------\r\n" + logInfo;
                    fileName = String.Format("{0}\\{1:yyyyMMdd}INFO.log", _infoLogFilePath, DateTime.Today);
                    break;
                case LogTypeEnum.Error:
                    logInfo += String.Format("[Error]:{0}\r\n", logText);
                    logInfo = "-------------错误日志---------------\r\n" + logInfo;
                    fileName = String.Format("{0}\\{1:yyyyMMdd}ERROR.log", _errorLogFilePath, DateTime.Today);
                    break;
            }
              // 写入日志
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.Write(logInfo);
            }
        }
        #endregion
    }
}
