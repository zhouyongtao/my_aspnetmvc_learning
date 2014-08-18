using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

/*
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  C# IN DEPT第九章
 * @date 2013年9月11日11:45:38
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer
 */
namespace Homeinns.Common.Ext.Extension
{
    public static class TypeExtension
    {
        public static bool IsNullableType(this Type type)
        {
            return (((type != null) && type.IsGenericType) &&
                (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        public static bool IsEnumerableType(this Type enumerableType)
        {
            return (FindGenericType(typeof(IEnumerable<>), enumerableType) != null);
        }

        public static Type GetElementType(this Type enumerableType)
        {
            Type type = FindGenericType(typeof(IEnumerable<>), enumerableType);
            if (type != null)
            {
                return type.GetGenericArguments()[0];
            }
            return enumerableType;
        }

        public static bool IsKindOfGeneric(this Type type, Type definition)
        {
            return (FindGenericType(definition, type) != null);
        }

        public static Type FindGenericType(this Type definition, Type type)
        {
            while ((type != null) && (type != typeof(object)))
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == definition))
                {
                    return type;
                }
                if (definition.IsInterface)
                {
                    foreach (Type type2 in type.GetInterfaces())
                    {
                        Type type3 = FindGenericType(definition, type2);
                        if (type3 != null)
                        {
                            return type3;
                        }
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        public static Func<TResult> Ctor<TResult>(this Type type)
        {
            return Expression.Lambda<Func<TResult>>(Expression.New(GetConstructor(type, Type.EmptyTypes)), new ParameterExpression[0]).Compile();
        }

        public static Func<TArg1, TResult> Ctor<TArg1, TResult>(this Type type)
        {
            ParameterExpression expression;
            ConstructorInfo constructor = GetConstructor(type, new Type[] { typeof(TArg1) });
            return Expression.Lambda<Func<TArg1, TResult>>(Expression.New(constructor, new Expression[] { expression = Expression.Parameter(typeof(TArg1), "arg1") }), new ParameterExpression[] { expression }).Compile();
        }

        public static Func<TArg1, TArg2, TResult> Ctor<TArg1, TArg2, TResult>(this Type type)
        {
            ParameterExpression expression;
            ParameterExpression expression2;
            ConstructorInfo constructor = GetConstructor(type, new Type[] { typeof(TArg1), typeof(TArg2) });
            return Expression.Lambda<Func<TArg1, TArg2, TResult>>(Expression.New(constructor, new Expression[] { expression = Expression.Parameter(typeof(TArg1), "arg1"), expression2 = Expression.Parameter(typeof(TArg2), "arg2") }), new ParameterExpression[] { expression, expression2 }).Compile();
        }

        public static Func<TArg1, TArg2, TArg3, TResult> Ctor<TArg1, TArg2, TArg3, TResult>(this Type type)
        {
            ParameterExpression expression;
            ParameterExpression expression2;
            ParameterExpression expression3;
            ConstructorInfo constructor = GetConstructor(type, new Type[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) });
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TResult>>(Expression.New(constructor, new Expression[] { expression = Expression.Parameter(typeof(TArg1), "arg1"), expression2 = Expression.Parameter(typeof(TArg2), "arg2"), expression3 = Expression.Parameter(typeof(TArg3), "arg3") }), new ParameterExpression[] { expression, expression2, expression3 }).Compile();
        }

        public static Func<TArg1, TArg2, TArg3, TArg4, TResult> Ctor<TArg1, TArg2, TArg3, TArg4, TResult>(this Type type)
        {
            ParameterExpression expression;
            ParameterExpression expression2;
            ParameterExpression expression3;
            ParameterExpression expression4;
            ConstructorInfo constructor = GetConstructor(type, new Type[] { typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4) });
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(Expression.New(constructor, new Expression[] { expression = Expression.Parameter(typeof(TArg1), "arg1"), expression2 = Expression.Parameter(typeof(TArg2), "arg2"), expression3 = Expression.Parameter(typeof(TArg3), "arg3"), expression4 = Expression.Parameter(typeof(TArg4), "arg4") }), new ParameterExpression[] { expression, expression2, expression3, expression4 }).Compile();
        }

        private static ConstructorInfo GetConstructor(Type type, params Type[] argumentTypes)
        {
            type.ThrowIfNull<Type>("type");
            argumentTypes.ThrowIfNull<Type[]>("argumentTypes");
            ConstructorInfo constructor = type.GetConstructor(argumentTypes);
            if (constructor != null)
            {
                return constructor;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(type.Name).Append(" has no ctor(");
            for (int i = 0; i < argumentTypes.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }
                builder.Append(argumentTypes[i].Name);
            }
            builder.Append(')');
            throw new InvalidOperationException(builder.ToString());
        }
    }
}
