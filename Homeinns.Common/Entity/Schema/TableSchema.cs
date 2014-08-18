using System.Collections.Generic;

namespace Homeinns.Common.Entity.Schema
{
    public class TableSchema
    {
        private string _columnsString;
        private string _hardColumnsString;
        private List<string> _lazyColumns;

        /// <summary>
        /// 列串
        /// </summary>
        public string ColumnString
        {
            get
            {
                if (_columnsString == null)
                {
                    _columnsString = "[" + string.Join("],[", Columns.ToArray()) + "]";
                }
                return _columnsString;
            }
            set { _columnsString = value; }
        }

        /// <summary>
        /// 非惰性列
        /// </summary>
        public string HardColumnsString
        {
            get
            {
                if (_hardColumnsString == null)
                {
                    var tempColumns = new List<string>();
                    foreach (string column in Columns)
                    {
                        if (!_lazyColumns.Contains(column))
                        {
                            tempColumns.Add(column);
                        }
                    }

                    _hardColumnsString = "[" + string.Join("],[", tempColumns.ToArray()) + "]";
                }

                return _hardColumnsString;
            }
            set { _hardColumnsString = value; }
        }

        /// <summary>
        /// 惰性列
        /// </summary>
        public List<string> LazyColumns
        {
            get { return _lazyColumns; }
            set { _lazyColumns = value; }
        }

        /// <summary>
        /// 所有列
        /// </summary>
        public List<string> Columns { get; set; }

        /// <summary>
        /// 主键列
        /// </summary>
        public List<string> PrimaryKeyColumns { get; set; }


        /// <summary>
        /// 列字典
        /// </summary>
        public Dictionary<string, ColumnSchema> ColumnDict { get; set; }
    }
}