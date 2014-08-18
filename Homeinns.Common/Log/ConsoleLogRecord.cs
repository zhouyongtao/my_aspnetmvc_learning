using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Homeinns.Common.Log
{
    /// <summary>
    /// 控制台记录日志
    /// </summary>
    public class ConsoleLogRecord:IRecordLog
    {
        public ConsoleLogRecord()
        {
            foreach (TraceListener item in Trace.Listeners)
            {
                if (item.Name == "Console")
                    return;
            }

            TraceListener listener = new ConsoleTraceListener(false) { Name = "Console" };
            Trace.Listeners.Add(listener);
        }

        #region IRecordLog 成员

        public void RecordLog(string log,LogTypeEnum logType)
        {
            Trace.Write(log);
            Trace.Flush();
        }

        #endregion
    }
}
