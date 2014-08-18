using System;
using System.Collections.Generic;
using System.Linq;

namespace Homeinns.Common.Log
{
    /// <summary>
    /// 日志记录委托
    /// </summary>
    /// <param name="logText">日志内容</param>
    /// <param name="logType">日志类型</param>
    public delegate void DelegateLogHandler(string logText, LogTypeEnum logType);
}
