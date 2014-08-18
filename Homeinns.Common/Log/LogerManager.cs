using System;
using System.Collections.Generic;
using System.Text;

namespace Homeinns.Common.Log
{
    public class LogerManager
    {
        #region 字段
        readonly List<IRecordLog> _logerList = new List<IRecordLog>();
        readonly string _location;
        #endregion

        #region 构造

        public LogerManager()
        { }

        public LogerManager(Type type)
        {
            _location = type.FullName;
        }

        public LogerManager(string location)
        {
            _location = location;
        }

        #endregion

        /// <summary>
        /// 日志记录器集合
        /// </summary>
        public List<IRecordLog> LogerList
        {
            get { return _logerList; }
        }

        /// <summary>
        /// 是否记录操作日志
        /// </summary>
        public bool IsRecord { get; set; }

        /// <summary>
        /// 根据日志类型进行记录
        /// </summary>
        /// <param name="info">日志内容</param>
        /// <param name="logType"></param>
        public void RecordLog(string logText, LogTypeEnum logType)
        {
            //  logText = string.Format("/*------------------------------------START------------------------------------\r\n[DateTime]:{0}\r\n[Location:]{1}\r\n{2}\r\n*/\r\n "
            // ,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff")
            //,_location
            //, logText);
            LogerList.ForEach(log =>
            {
                // 如果标记不需要记录操作日志 并且此次日志是操作日志 则退出方法
                if(IsRecord == false && logType == LogTypeEnum.Info) return; 
                log.RecordLog(logText, logType);
            });
        }
    }
}
