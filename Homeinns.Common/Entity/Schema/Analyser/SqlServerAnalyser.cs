using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Homeinns.Common.Entity.Schema.Analyser
{
    internal class SqlServerAnalyser : DatabaseAnalyser
    {
        public SqlServerAnalyser(string connectionText) : base(connectionText)
        {
        }

        /// <summary>
        /// 设置表主键
        /// </summary>
        /// <param name="tableDict"></param>
        /// <param name="indexColumnData"></param>
        protected override void FillTableDictPrimaryKey(IDictionary<string, TableSchema> tableDict,
                                                        DataTable indexColumnData)
        {
            foreach (DataRow row in indexColumnData.Rows)
            {
                if (row["constraint_name"].ToString().ToUpper().IndexOf("PK_") != -1)
                {
                    string columnName = row["column_name"].ToString();
                    string upperColumnName = columnName.ToUpper();
                    TableSchema sqlTableSchema = tableDict[row["table_name"].ToString().ToUpper()];
                    sqlTableSchema.PrimaryKeyColumns.Add(columnName);
                    sqlTableSchema.ColumnDict[upperColumnName].IsPrimaryKey = true;
                }
            }
        }


        protected override DataTable GetColumnsData(DbConnection conn)
        {
            return conn.GetSchema("Columns");
        }

        protected override DataTable GetPrimaryKeyData(DbConnection conn)
        {
            return conn.GetSchema("IndexColumns");
        }

        /// <summary>
        /// 填充表数据字典
        /// </summary>
        /// <param name="tableDict"></param>
        /// <param name="columnData"></param>
        protected override void FillTableDictTableAndColumn(IDictionary<string, TableSchema> tableDict, DataTable columnData)
        {
            foreach (DataRow row in columnData.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                string upperTableName = tableName.ToUpper();
                string columnName = row["COLUMN_NAME"].ToString();
                string upperColumnName = columnName.ToUpper();

                TableSchema sqlTableSchema;
                if (!tableDict.ContainsKey(upperTableName))
                {
                    sqlTableSchema = new TableSchema
                    {
                        ColumnDict = new Dictionary<string, ColumnSchema>(),
                        Columns = new List<string>(),
                        PrimaryKeyColumns = new List<string>(),
                        LazyColumns = new List<string>()
                    };
                    tableDict.Add(upperTableName, sqlTableSchema);
                }

                var sqlColumnSchema = new ColumnSchema { TableName = tableName, ColumnName = columnName };
                bool isLazy = false;
                sqlColumnSchema.ColumnType = GetDBType(row["DATA_TYPE"].ToString(), ref isLazy);
                sqlColumnSchema.IsLazy = isLazy;
                if (!string.IsNullOrEmpty(row["CHARACTER_MAXIMUM_LENGTH"].ToString()))
                    sqlColumnSchema.ColumnLength = Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]);
                sqlColumnSchema.IsNullAble = row["IS_NULLABLE"].ToString() == "YES";

                sqlTableSchema = tableDict[upperTableName];
                sqlTableSchema.ColumnDict.Add(upperColumnName, sqlColumnSchema);

                string propertyName = columnName;
                sqlTableSchema.Columns.Add(propertyName);
                if (isLazy)
                    sqlTableSchema.LazyColumns.Add(propertyName);
            }
        }


        public override string ProviderInvariantName
        {
            get { return "System.Data.SqlClient"; }
        }

        protected override DbType GetDBType(string typeName, ref bool isLazy)
        {
            switch (typeName)
            {
                case "bigint":
                    return DbType.Int64;
                case "int":
                    return DbType.Int32;
                case "smallint":
                    return DbType.Int16;
                case "tinyint":
                    return DbType.Byte;
                case "bit":
                    return DbType.Boolean;
                case "decimal":
                    return DbType.Decimal;
                case "numberic":
                    return DbType.Decimal;
                case "money":
                    return DbType.Decimal;
                case "smallmoney":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "real":
                    return DbType.Double;
                case "datetime":
                    return DbType.DateTime;
                case "smalldatetime":
                    return DbType.DateTime;
                case "timestamp":
                    return DbType.DateTime;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "varchar":
                    return DbType.AnsiString;
                case "text":
                    return DbType.AnsiString;
                case "nchar":
                    return DbType.StringFixedLength;
                case "nvarchar":
                    return DbType.String;
                case "ntext":
                    return DbType.String;
                case "binary":
                    isLazy = true;
                    return DbType.Binary;
                case "varbinary":
                    return DbType.Binary;
                case "image":
                    isLazy = true;
                    return DbType.Binary;
                case "uniqueidentifier":
                    return DbType.Guid;
            }

            return DbType.String;
        }
    }
}