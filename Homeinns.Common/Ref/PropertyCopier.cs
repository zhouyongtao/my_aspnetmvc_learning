using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace Homeinns.Common.Ref
{
    public static class PropertyCopier<TSource, TTarget>
    {
        // Fields
        private static readonly Func<TSource, TTarget> creator;
        private static readonly Exception initializationException;
        private static readonly List<PropertyInfo> sourceProperties;
        private static readonly List<PropertyInfo> targetProperties;

        // Methods
        static PropertyCopier()
        {
            PropertyCopier<TSource, TTarget>.sourceProperties = new List<PropertyInfo>();
            PropertyCopier<TSource, TTarget>.targetProperties = new List<PropertyInfo>();
            try
            {
                PropertyCopier<TSource, TTarget>.creator = PropertyCopier<TSource, TTarget>.BuildCreator();
                PropertyCopier<TSource, TTarget>.initializationException = null;
            }
            catch (Exception exception)
            {
                PropertyCopier<TSource, TTarget>.creator = null;
                PropertyCopier<TSource, TTarget>.initializationException = exception;
            }
        }

        private static Func<TSource, TTarget> BuildCreator()
        {
            ParameterExpression expression=null;
            List<MemberBinding> bindings = new List<MemberBinding>();
            foreach (PropertyInfo info in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (info.CanRead)
                {
                    PropertyInfo property = typeof(TTarget).GetProperty(info.Name);
                    if (property == null)
                    {
                        throw new ArgumentException(String.Format("Property {0} is not present and accessible in {1}", info.Name, typeof(TTarget).FullName));
                    }
                    if (!property.CanWrite)
                    {
                        throw new ArgumentException(String.Format("Property {0} is not writable in {1}", info.Name, typeof(TTarget).FullName));
                    }
                    if ((property.GetSetMethod().Attributes & MethodAttributes.Static) != MethodAttributes.ReuseSlot)
                    {
                        throw new ArgumentException(String.Format("Property {0} is static in {1}", info.Name, typeof(TTarget).FullName));
                    }
                    if (!property.PropertyType.IsAssignableFrom(info.PropertyType))
                    {
                        throw new ArgumentException(String.Format("Property {0} has an incompatible type in {1}", info.Name, typeof(TTarget).FullName));
                    }
                    bindings.Add(Expression.Bind(property, Expression.Property(expression = Expression.Parameter(typeof(TSource), "source"), info)));
                    PropertyCopier<TSource, TTarget>.sourceProperties.Add(info);
                    PropertyCopier<TSource, TTarget>.targetProperties.Add(property);
                }
            }
            return Expression.Lambda<Func<TSource, TTarget>>(Expression.MemberInit(Expression.New(typeof(TTarget)), bindings), new ParameterExpression[] { expression }).Compile();
        }

        internal static TTarget Copy(TSource source)
        {
            if (PropertyCopier<TSource, TTarget>.initializationException != null)
            {
                throw PropertyCopier<TSource, TTarget>.initializationException;
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return PropertyCopier<TSource, TTarget>.creator(source);
        }

        internal static void Copy(TSource source, TTarget target)
        {
            if (PropertyCopier<TSource, TTarget>.initializationException != null)
            {
                throw PropertyCopier<TSource, TTarget>.initializationException;
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            for (int i = 0; i < PropertyCopier<TSource, TTarget>.sourceProperties.Count; i++)
            {
                PropertyCopier<TSource, TTarget>.targetProperties[i].SetValue(target, PropertyCopier<TSource, TTarget>.sourceProperties[i].GetValue(source, null), null);
            }
        }

    }
}
