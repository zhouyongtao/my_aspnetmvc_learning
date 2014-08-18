using System;
using System.Collections.Generic;
using System.Linq;

namespace Homeinns.Common.Net.Http
{
    /// <summary>
    /// 重定向
    /// </summary>
    public class HttpRedirection
    {
        /// <summary>
        /// 源地址
        /// </summary>
        public Uri Orginal { get; private set; }

        /// <summary>
        /// 当前响应地址
        /// </summary>
        public Uri Current { get; private set; }


        /// <summary>
        /// 创建 <see cref="HttpRedirection"/>  的新实例(HttpRedirection)
        /// </summary>
        public HttpRedirection(Uri orginal, Uri current)
        {
            Orginal = orginal;
            Current = current;
        }
    }
}
