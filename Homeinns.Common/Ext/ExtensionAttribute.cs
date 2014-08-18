using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Runtime.CompilerServices
{
#if !NET4
    /// <summary>支持使用扩展方法的特性</summary>
    /// <remarks>
    /// 为了能在vs2010+.Net 2.0中使用扩展方法，添加该特性。
    /// 在vs2010+.Net4.0中引用当前程序集，会爆一个预定义类型多次定义的警告，不影响使用
    /// </remarks>
    public sealed class ExtensionAttribute : Attribute { }
#endif
}
