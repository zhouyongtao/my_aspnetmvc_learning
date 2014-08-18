using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NRainel.Log.ILog
{
    /// <summary>
    /// 文本日志记录
    /// </summary>
    public class TextLoger : LogerBase
    {
        private readonly string _userID;
        private readonly string _opLogFilePath;
        private readonly string _errorLogFilePath;

        public TextLoger(string userID, string logFilePath)
        {
            _userID = userID;

            _opLogFilePath = logFilePath + "\\OPLog";
            _errorLogFilePath = logFilePath + "\\ErrorLog";

            CheckPath(_opLogFilePath);
            CheckPath(_errorLogFilePath);
        }

        /// <summary>
        /// 保证 路径存在
        /// </summary>
        /// <param name="path"></param>
        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public override void RecordLog(string logText, LogTypeEnum logType)
        {
            string str = String.Format("[UserID]:{0}[DateTime]:{1:yyyy-MM-dd HH:mm:ss,fff}\r\n", _userID, DateTime.Now);

            string fileName = string.Empty;
            switch (logType)
            {
                case LogTypeEnum.OPLog:
                    str += String.Format("[OP]:{0}\r\n", logText);
                    str = "-------------操作日志---------------\r\n" + str;
                    fileName = String.Format("{0}\\{1:yyMMdd}OP.log", _opLogFilePath, DateTime.Today);
                    break;
                case LogTypeEnum.ErrorLog:
                    str += String.Format("[Error]:{0}\r\n", logText);
                    str = "-------------错误日志---------------\r\n" + str;
                    fileName = String.Format("{0}\\{1:yyMMdd}ER.log", _errorLogFilePath, DateTime.Today);
                    break;
            }

            // 写入日志
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.Write(str);
            }

        }
    }

}
