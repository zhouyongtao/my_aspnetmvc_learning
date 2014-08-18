using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace Homeinns.Common.Entity.Schema.Analyser
{
    internal class SQLiteAnalyser : DatabaseAnalyser
    {
        public SQLiteAnalyser(string connectionText)
            : base(connectionText)
        {
        }

        public override string ProviderInvariantName
        {
            get
            {
                return "System.Data.SQLite";
            }
        }

        protected override DbType GetDBType(string typeName, ref bool isLazy)
        {
            typeName = typeName.ToUpper();
            switch (typeName)
            {
                case "BIT":
                case "YESNO":
                case "LOGICAL":
                case "BOOL":
                    return DbType.Boolean;
                case "TINYINT":
                    return DbType.Byte;
                case "SMALLINT":
                    return DbType.Int16;
                case "INT":
                    return DbType.Int32;
                case "INTEGER":
                case "BIGINT":
                case "COUNTER":
                case "AUTOINCREMENT":
                case "IDENTITY":
                case "LONG":
                    return DbType.Int64;
                case "REAL":
                    return DbType.Single;
                case "DOUBLE":
                case "FLOAT":
                    return DbType.Double;
                case "NUMERIC":
                case "DECIMAL":
                case "MONEY":
                case "CURRENCY":
                    return DbType.Decimal;
                case "TEXT":
                case "NTEXT":
                case "LONGTEXT":
                    isLazy = true;
                    return DbType.String;
                case "CHAR":
                case "NCHAR":
                case "VARCHAR":
                case "NVARCHAR":

                case "LONGCHAR":
                case "LONGVARCHAR":
                case "STRING":
                case "MEMO":
                case "NOTE":
                    return DbType.String;
                case "BLOB":
                case "BINARY":
                case "VARBINARY":
                case "IMAGE":
                case "GENERAL":
                case "OLEOBJECT":
                    isLazy = true;
                    return DbType.Binary;
                case "GUID":
                case "UNIQUEIDENTIFIER":
                    return DbType.Guid;
                case "TIME":
                case "DATE":
                case "DATETIME":
                case "SMALLDATE":
                case "SMALLDATETIME":
                case "TIMESTAMP":
                    return DbType.DateTime;
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
                var tableName = row["TABLE_NAME"].ToString();
                var upperTableName = tableName.ToUpper();
                var columnName = row["COLUMN_NAME"].ToString();
                var upperColumnName = columnName.ToUpper();

                TableSchema sqlTableSchema = null;
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
                var isLazy = false;

                if ((bool)row["PRIMARY_KEY"])
                {
                    sqlTableSchema.PrimaryKeyColumns.Add(tableName);
                    sqlColumnSchema.IsPrimaryKey = true;
                }
                sqlColumnSchema.ColumnType = GetDBType(row["DATA_TYPE"].ToString(), ref isLazy);
                sqlColumnSchema.IsLazy = isLazy;
                if (!string.IsNullOrEmpty(row["CHARACTER_MAXIMUM_LENGTH"].ToString()))
                {
                    sqlColumnSchema.ColumnLength = Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]);
                }
                sqlColumnSchema.IsNullAble = row["IS_NULLABLE"].ToString() == "YES";

                sqlTableSchema = tableDict[upperTableName];
                sqlTableSchema.ColumnDict.Add(upperColumnName, sqlColumnSchema);

                var propertyName = columnName;
                sqlTableSchema.Columns.Add(propertyName);
                if (isLazy)
                {
                    sqlTableSchema.LazyColumns.Add(propertyName);
                }
            }
        }

        protected override void FillTableDictPrimaryKey(IDictionary<string, TableSchema> tableDict, DataTable indexColumnData)
        {
        }
    }
}
