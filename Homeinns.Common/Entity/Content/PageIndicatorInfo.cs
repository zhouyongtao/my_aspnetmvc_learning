using System;
using System.Collections.Generic;
using System.Text;

namespace Homeinns.Common.Entity.Content
{
    public class PageIndicatorInfo
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 缓存分页数据
        /// </summary>
        public bool CacheData { get; set; }

    }
}