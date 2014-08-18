using System.Data;

namespace Homeinns.Common.Entity.Schema
{
    /// <summary>
    /// 列数据实体类
    /// </summary>
    public class ColumnSchema
    {
        #region 属性过程

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 列长度
        /// </summary>
        public int? ColumnLength { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullAble { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否惰性
        /// </summary>
        public bool IsLazy { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        #endregion

        /// <summary>
        /// 列类型
        /// </summary>
        public DbType ColumnType { get; set; }
    }
}