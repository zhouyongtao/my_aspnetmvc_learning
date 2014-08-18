using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRainel.Log.ILog
{
    /// <summary>
    /// Log日志记录
    /// </summary>
    public class Log
    {
        private static LogerManager log = null;
        private readonly static object lockObject = new object();
        public static LogerManager Loger
        {
            get
            {
                if (log == null)
                {
                    lock (lockObject)
                    {
                        if (log == null)

                            log = LogerManagerFactory.Create();
                    }
                }
                return log;
            }
        }
    }
}
