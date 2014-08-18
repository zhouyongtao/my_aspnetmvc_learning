using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Homeinns.Common.Entity.Schema.Analyser
{
    internal class AccessAnalyser : DatabaseAnalyser
    {
        public AccessAnalyser(string connectionText) : base(connectionText)
        {
        }

        public override string ProviderInvariantName
        {
            get { return "System.Data.OleDb"; }
        }

        protected override DbType GetDBType(string typeName, ref bool isLazy)
        {
            switch (typeName)
            {
                case "2":
                    return DbType.Int16;
                case "3":
                    return DbType.Int32;
                case "4":
                    return DbType.Double;
                case "5":
                    return DbType.Double;
                case "6":
                    return DbType.Decimal;
                case "7":
                    return DbType.Date; // 无法完整保存 时分秒
                case "11":
                    return DbType.Boolean;
                case "12":
                    return DbType.String; //?
                case "17":
                    return DbType.Byte;
                case "20":
                    return DbType.Int64;
                case "72":
                    return DbType.Guid;
                case "128":
                    return DbType.Binary;
                case "129":
                    return DbType.AnsiStringFixedLength;
                case "130":
                    return DbType.StringFixedLength;
                case "131":
                    return DbType.Decimal;
                case "135":
                    return DbType.DateTime;
                case "200":
                    return DbType.AnsiString;
                case "201":
                    return DbType.AnsiString;
                case "202":
                    return DbType.String;
                case "203":
                    return DbType.String;
                case "204":
                    isLazy = true;
                    return DbType.Binary;
                case "205":
                    isLazy = true;
                    return DbType.Binary;
            }

            return DbType.String;
        }

        protected override DataTable GetColumnsData(DbConnection conn)
        {
            return conn.GetSchema("Columns");
        }

        protected override DataTable GetPrimaryKeyData(DbConnection conn)
        {
            return conn.GetSchema("Indexes");
        }

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

        protected override void FillTableDictPrimaryKey(IDictionary<string, TableSchema> tableDict, DataTable indexColumnData)
        {
            foreach (DataRow row in indexColumnData.Rows)
            {
                if ((bool)row["PRIMARY_KEY"])
                {
                    string columnName = row["COLUMN_NAME"].ToString();
                    string upperColumnName = columnName.ToUpper();
                    TableSchema sqlTableSchema = tableDict[row["TABLE_NAME"].ToString().ToUpper()];
                    sqlTableSchema.PrimaryKeyColumns.Add(columnName);
                    sqlTableSchema.ColumnDict[upperColumnName].IsPrimaryKey = true;
                }
            }
        }
    }
}