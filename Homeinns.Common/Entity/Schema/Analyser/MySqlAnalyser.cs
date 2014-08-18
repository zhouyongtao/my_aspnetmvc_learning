using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Homeinns.Common.Entity.Schema.Analyser
{
    internal class MySqlAnalyser : DatabaseAnalyser
    {
        public MySqlAnalyser(string connectionText) : base(connectionText)
        {
        }

        public override string ProviderInvariantName
        {
            get { return "System.Data.OleDb"; }
        }

        protected override DbType GetDBType(string typeName, ref bool isLazy)
        {
            throw new NotImplementedException();
        }

        protected override DataTable GetColumnsData(DbConnection conn)
        {
            throw new System.NotImplementedException();
        }

        protected override DataTable GetPrimaryKeyData(DbConnection conn)
        {
            throw new System.NotImplementedException();
        }

        protected override void FillTableDictTableAndColumn(IDictionary<string, TableSchema> tableDict, DataTable columnData)
        {
            throw new System.NotImplementedException();
        }

        protected override void FillTableDictPrimaryKey(IDictionary<string, TableSchema> tableDict, DataTable indexColumnData)
        {
            throw new System.NotImplementedException();
        }
    }
}