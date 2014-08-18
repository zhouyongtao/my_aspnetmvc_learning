using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homeinns.Common.Base.Web.JavaScript
{
    /// <summary>
    /// Ajax包装类  匿名类与扩展方法
    /// </summary>
    public class AjaxResponse
    {
        // 是否成功
        public bool IsSuccess { get; set; }

        // 消息
        public string Msg { get; set; }

        // 数据
        public object Data { get; set; }
    }
}
