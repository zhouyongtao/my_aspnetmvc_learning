using System;
using System.Collections.Generic;
using System.Linq;

namespace Homeinns.Common.Net.Http
{
    /// <summary>
    /// 表示这是一个附加的表单文件
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AttachedFileAttribute : Attribute
    {
    }
}
