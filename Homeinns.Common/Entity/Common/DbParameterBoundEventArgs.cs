using System;
using System.Data.Common;
using Homeinns.Common.Entity.Schema;

namespace Homeinns.Common.Entity.Common
{
    public delegate void DbParameterBoundDelegate(object sender, DbParameterBoundEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class DbParameterBoundEventArgs : EventArgs
    {
        public DbParameterBoundEventArgs(ColumnSchema columnSchema, DbParameter parameter)
        {
            ColumnSchema = columnSchema;
            Parameter = parameter;
        }

        #region 属性

        /// <summary>
        /// 列结构
        /// </summary>
        public ColumnSchema ColumnSchema { get; set; }

        /// <summary>
        /// Parameter对象
        /// </summary>
        public DbParameter Parameter { get; set; }


        #endregion
    }
}