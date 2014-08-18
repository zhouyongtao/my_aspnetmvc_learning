using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRainel.Log.ILog
{
    public abstract class LogerBase
    {
        /// <summary>
        /// 向windows事件日志写入应用程序日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        /// <param name="logType">日志类型</param>
        public abstract void RecordLog(string logText, LogTypeEnum logType);

    }
}
