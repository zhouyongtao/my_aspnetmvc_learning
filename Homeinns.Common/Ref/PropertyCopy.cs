using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homeinns.Common.Ref
{
    public static class PropertyCopy
    {
        public static void Copy<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            PropertyCopier<TSource, TTarget>.Copy(source, target);
        }
    }
    public static class PropertyCopy<TTarget> where TTarget : class, new()
    {
        public static TTarget CopyFrom<TSource>(TSource source) where TSource : class
        {
            return PropertyCopier<TSource, TTarget>.Copy(source);
        }
    }
}
