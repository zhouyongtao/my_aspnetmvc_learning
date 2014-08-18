﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Homeinns.Common.Net.Http
{
    /// <summary>
    /// 表示字符串响应
    /// </summary>
    public class ResponseStringContent : ResponseBinaryContent
    {
        /// <summary>
        /// 创建 <see cref="ResponseStringContent"/>  的新实例(ResponseStringContent)
        /// </summary>
        public ResponseStringContent(HttpContext context, HttpClient client)
            : base(context, client)
        {
        }

        /// <summary>
        /// 获得响应内容
        /// </summary>
        public new string Result
        {
            get
            {
                return StringResult;
            }
        }

        /// <summary>
        /// 处理响应
        /// </summary>
        /// <param name="stream"></param>
        protected override void ProcessResponse(Stream stream)
        {
            base.ProcessResponse(stream);
        }
    }
}
