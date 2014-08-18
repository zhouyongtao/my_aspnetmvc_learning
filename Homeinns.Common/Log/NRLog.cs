using System;
using System.Collections.Generic;
using System.IO;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  Nlog日志类
 * @date 2013年9月10日14:21:53
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Log
{
    /*
     日志配置
     <add key="logType" value="TextLog"/>
     <add key="OPLog" value="true"/>
    */
    public class NRLog
    {
        private static LogerManager log = null;
        private readonly static object lockObject = new object();
        private NRLog()
        { }
        /// <summary>
        /// 创建工厂
        /// </summary>
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
        /// <summary>
        /// 记录简单异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void ExceptionLog(string message, Exception exception)
        {
            try
            {
                if (message.IsEmpty() || exception == null)
                    return;
                string logsDir = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }
                using (StreamWriter fs = new StreamWriter(String.Format("{0}\\ExceptionLog{1:yyyy-MM-dd}.log", logsDir, DateTime.Today), true))
                {
                    fs.WriteLine(string.Format("Time:{0} =============================================> \r\n Error:{1}", DateTime.Now, String.Format("{0}: {1}", message, exception)));
                    fs.Flush();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}