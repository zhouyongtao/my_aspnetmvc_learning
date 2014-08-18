using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;


namespace Homeinns.Common.Entity.Schema.Analyser
{
    internal abstract class DatabaseAnalyser
    {
        protected DbProviderFactory _DbProviderFactory;
        public DbProviderFactory ProviderFactory
        {
            get
            {
                if(_DbProviderFactory == null)
                {
                    _DbProviderFactory = NRDbProviderFactories.GetFactory(ProviderInvariantName);
                }

                return _DbProviderFactory;
            }
        }

        protected string _connectionText;

        protected DatabaseAnalyser(string connectionText)
        {
            _connectionText = connectionText;
        }

        /*
        private DBMSType _DBSType;

        public DBMSType DBSType
        {
            get
            {
                return _DBSType;
            }
        }
        */

        /// <summary>
        /// 创建数据库解析器
        /// </summary>
        /// <param name="connectionText">数据库连接字符串</param>
        /// <param name="dbmsType">数据库类型枚举</param>
        /// <returns></returns>
        public static DatabaseAnalyser Create(string connectionText,DBMSType dbmsType)
        {
            DatabaseAnalyser dbAnalyser = null;
            switch (dbmsType)
            {
                case DBMSType.Access:
                    dbAnalyser = new AccessAnalyser(connectionText);
                    break;
                case DBMSType.MySql:
                    dbAnalyser = new MySqlAnalyser(connectionText);
                    break;
                case DBMSType.Oracle:
                    dbAnalyser = new OracleAnalyser(connectionText);
                    break;
                case DBMSType.SqlServer:
                    dbAnalyser = new SqlServerAnalyser(connectionText);
                    break;
                case DBMSType.SQLite:
                    dbAnalyser = new SQLiteAnalyser(connectionText);
                    break;
            }

            return dbAnalyser;
        }

        protected internal void FillTableDict(IDictionary<string, TableSchema> tableDict)
        {
            try
            {
                DataTable columnData;
                DataTable indexColumnsData;
                using (DbConnection conn = ProviderFactory.CreateConnection())
                {
                    conn.ConnectionString = _connectionText;
                    
                    conn.Open();
                    columnData = GetColumnsData(conn);
                    indexColumnsData = GetPrimaryKeyData(conn);
                }

                FillTableDictTableAndColumn(tableDict, columnData);
                FillTableDictPrimaryKey(tableDict, indexColumnsData);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                throw ex;
            }
        }

        public abstract string ProviderInvariantName { get; }
        protected abstract DataTable GetColumnsData(DbConnection conn);
        protected abstract DataTable GetPrimaryKeyData(DbConnection conn);
        protected abstract void FillTableDictTableAndColumn(IDictionary<string, TableSchema> tableDict, DataTable columnData);
        protected abstract void FillTableDictPrimaryKey(IDictionary<string, TableSchema> tableDict, DataTable indexColumnData);
        protected abstract DbType GetDBType(string typeName, ref bool isLazy);
   

        
    }
}