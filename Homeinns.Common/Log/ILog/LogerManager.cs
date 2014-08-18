using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRainel.Log.ILog
{
    public class LogerManager
    {
        readonly List<LogerBase> _logerList = new List<LogerBase>();
        public List<LogerBase> LogerList
        {
            get { return _logerList; }
        }

        /// <summary>
        /// 是否记录操作日志
        /// </summary>
        public bool RecordOPLog { get; set; }
        /// <summary>
        /// 向windows事件日志写入应用程序日志
        /// </summary>
        /// <param name="logText">日志内容</param>
        /// <param name="logType">日志类型</param>
        public void RecordLog(string logText, LogTypeEnum logType)
        {
            // 如果标记不需要记录操作日志 并且此次日志是操作日志 则退出方法
            if (RecordOPLog == false && logType == LogTypeEnum.OPLog)
                return;

            for (int i = 0; i < _logerList.Count; i++)
            {
                _logerList[i].RecordLog(logText, logType);
            }
        }
    }
}
