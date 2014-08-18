using System;
using System.Collections.Generic;

namespace Homeinns.Common.Entity.Text
{
    /// <summary>
    /// Command查询字符串中表名与参数名
    /// </summary>
    public class CommandTextAnalyser
    {
        #region 辅助方法:取出DbCommand查询字符串中表名与参数名

        /// <summary>
        /// 取出DbCommand字符串中的表名
        /// </summary>
        /// <param name="commandText">带@的DbCommand字符串</param>
        /// <returns>表名列表</returns>
        public List<string> GetTableNames(string commandText)
        {
            commandText = commandText.ToUpper();
            string[] tableNamesTemp =
                commandText.Split(new[] { "FROM ", "JOIN ", "DELETE ", "UPDATE ", "INSERT INTO ", "INSERT " },
                                  StringSplitOptions.None);

            var tableNameList = new List<string>();
            

            for (int i = 0; i < tableNamesTemp.Length - 1; ++i)
            {
                string tableName;
                int removeIndex = tableNamesTemp[i + 1].Trim().IndexOfAny(new[] {']', ' ', '(', ')' });

                if (removeIndex != -1)
                    tableName = tableNamesTemp[i + 1].Trim().Remove(removeIndex);
                else
                    tableName = tableNamesTemp[i + 1];
                tableName = tableName.Replace("[", "");
                if (!tableNameList.Contains(tableName))
                    tableNameList.Add(tableName);
            }

            return tableNameList;
        }

 

        /// <summary>
        /// 取出Command字符串中的参数列表
        /// </summary>
        /// <param name="commandText">带@的DbCommand字符串</param>
        /// <returns>参数列表</returns>
        public string[] GetDbParameterNames(string commandText)
        {
            //取得以@开头的字符串
            int index = commandText.IndexOf('@');
            if (index == -1)
                return null;
            string[] sqlParameterNames = commandText.Substring(index).Split(new[] {'@'},
                                                                            StringSplitOptions.RemoveEmptyEntries);


            var sqlParameternameList = new List<string>();
            string tempStr;

            //去除参数以外的字符
            for (int i = 0; i < sqlParameterNames.Length; ++i)
            {
                int removeIndex = sqlParameterNames[i].IndexOfAny(new[] {' ', ',', ')', '=', '>', '<', '.', '+','\r','\n'});
                if (removeIndex != -1)
                    tempStr = sqlParameterNames[i].Remove(removeIndex);
                else
                    tempStr = sqlParameterNames[i];

                if (!sqlParameternameList.Contains(tempStr))
                {
                    sqlParameternameList.Add(tempStr);
                }
            }

            return sqlParameternameList.ToArray();
        }

        /// <summary>
        /// 取出DbCommand字符串中的参数列表
        /// </summary>
        /// <param name="commandText">带@的DbCommand字符串</param>
        /// <returns>参数列表</returns>
        public string[] GetSqlParameterNamesForWhere(string commandText)
        {
            //取得以@开头的字符串
            int index = commandText.IndexOf('@');
            if (index == -1)
                return null;
            string[] sqlParameterNames = commandText.Substring(index).Split(new[] {'@'},
                                                                            StringSplitOptions.RemoveEmptyEntries);
            var sqlParameternameList = new List<string>();
            string tempStr;
            //去除参数以外的字符
            for (int i = 0; i < sqlParameterNames.Length; ++i)
            {
                int removeIndex = sqlParameterNames[i].IndexOfAny(new[] {' ', ',', ')', '=', '>', '<', '.', '+'});
                if (removeIndex != -1)
                    tempStr = sqlParameterNames[i].Remove(removeIndex);
                else
                    tempStr = sqlParameterNames[i];
                sqlParameternameList.Add(tempStr);
            }

            return sqlParameternameList.ToArray();
            //return sqlParameterNames;
        }

        //public string GetColumnName(string commandString)
        //{
        //    int inIndex = commandString.ToUpper().IndexOf(" IN");
        //    commandString = commandString.Remove(inIndex).Trim();
        //    return commandString.Substring(commandString.LastIndexOf(' ') + 1);
        //}

        #endregion
    }
}