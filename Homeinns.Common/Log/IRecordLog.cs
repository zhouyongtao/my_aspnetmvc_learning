using System;
using System.Collections.Generic;
using System.Text;

namespace Homeinns.Common.Log
{
    /// <summary>
    /// 日志记录接口
    /// </summary>
    public interface IRecordLog
    {
        /// <summary>
        /// 向windows事件日志写入应用程序日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        /// <param name="logType">日志类型</param>
        void RecordLog(string logText, LogTypeEnum logType);
    }
}
