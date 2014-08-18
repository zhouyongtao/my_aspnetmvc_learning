using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Homeinns.Common.Entity.Schema.Analyser;

namespace Homeinns.Common.Entity.Schema
{
    /// <summary>
    /// 数据库字段信息的包装类
    /// </summary>
    public class DatabaseSchema
    {
        /// <summary>
        /// 数据库结构单例字典
        /// </summary>
        private static readonly Dictionary<string, DatabaseSchema> s_databaseDict =
            new Dictionary<string, DatabaseSchema>();

        private readonly DbProviderFactory _dbProviderFactory;
        private readonly Dictionary<string, TableSchema> _tableDict;
        private readonly string _connectionString;
        private readonly DBMSType _DBMSType;



        private DatabaseSchema(string connectionString, DBMSType dbmsType)
        {
            _connectionString = connectionString;
            _DBMSType = dbmsType;
            DatabaseAnalyser dbAnalyser = DatabaseAnalyser.Create(connectionString, dbmsType);
            _dbProviderFactory = dbAnalyser.ProviderFactory;

            _tableDict = new Dictionary<string, TableSchema>();
            dbAnalyser.FillTableDict(_tableDict);
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// 以"表名"+","+"列名"为Key相应的列的字段类型,长度,是否为空为Value的哈希表
        /// </summary>
        public Dictionary<string, TableSchema> TableDict
        {
            get { return _tableDict; }
        }

        /// <summary>
        /// 数据源操作对象创建工厂
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get { return _dbProviderFactory; }
        }

        public DBMSType DBSType
        {
            get { return _DBMSType; }
        }

        /// <summary>
        /// 创建数据库结构对象(单例)
        /// </summary>
        /// <param name="connstringString"></param>
        /// <param name="dbmsTypeStr">数据库类型字符串</param>
        /// <returns></returns>
        public static DatabaseSchema Create(string connstringString, string dbType)
        {
            dbType = dbType.ToUpper();
            DBMSType dbmsType;
            switch (dbType)
            {
                case "ACCESS":
                    dbmsType = DBMSType.Access;
                    break;
                case "MYSQL":
                    dbmsType = DBMSType.MySql;
                    break;
                case "ORACLE":
                    dbmsType = DBMSType.Oracle;
                    break;
                case "SQLSERVER":
                    dbmsType = DBMSType.SqlServer;
                    break;
                case "SQLITE":
                    dbmsType = DBMSType.SQLite;
                    break;
                default:
                    throw new NullReferenceException("数据库类型设置错误！");
            }

            return Create(connstringString, dbmsType);
        }

        /// <summary>
        /// 创建数据库结构对象(单例)
        /// </summary>
        /// <param name="connstringString">连接字符串</param>
        /// <param name="dbmsType">数据库类型枚举</param>
        /// <returns></returns>
        public static DatabaseSchema Create(string connstringString, DBMSType dbmsType)
        {
            string upperConnStr = connstringString.ToUpper();
            
            if (!s_databaseDict.ContainsKey(upperConnStr))
            {
                lock (typeof(DatabaseSchema))
                {   // 重复判断是为了减少排他锁对多线程读取的访问效率的影响
                    if (!s_databaseDict.ContainsKey(upperConnStr))
                    {
                        DatabaseSchema sdSchema = new DatabaseSchema(connstringString, dbmsType);
                        s_databaseDict.Add(upperConnStr, sdSchema);
                    }
                }
            }
            return s_databaseDict[upperConnStr];
        }

        /// <summary>
        /// 传入表名列表,列名得到列的数据 
        /// </summary>
        /// <param name="tableNames">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns>列数据</returns>
        public ColumnSchema GetColumnSchema(List<string> tableNames, string columnName)
        {
            string upperColumnName = columnName.ToUpper();

            foreach (string tableName in tableNames)
            {
                string upperTableName = tableName.ToUpper();
                if (_tableDict.ContainsKey(upperTableName))
                {
                    TableSchema sqlTableSchema = _tableDict[upperTableName];
                    if (sqlTableSchema.ColumnDict.ContainsKey(upperColumnName))
                    {
                        return sqlTableSchema.ColumnDict[upperColumnName];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 传入表名列名得到列的数据 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns>列数据</returns>
        public ColumnSchema GetColumnSchema(string tableName, string columnName)
        {
            ColumnSchema sqlColumnSchema = _tableDict[tableName.ToUpper()].ColumnDict[columnName.ToUpper()];

            if (sqlColumnSchema != null)
            {
                return sqlColumnSchema;
            }

            return new ColumnSchema();
        }
    }
}