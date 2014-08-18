using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  C# IN DEPT第九章
 * @date 2013年9月10日17:35:56
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Ext
{
    public static class ExpressionUtil
    {
        // Methods
        public static Func<TArg1, TResult> CreateExpression<TArg1, TResult>(Func<Expression, UnaryExpression> body)
        {
            try
            {
                ParameterExpression expression;
                return Expression.Lambda<Func<TArg1, TResult>>(body(expression = Expression.Parameter(typeof(TArg1), "inp")), new ParameterExpression[] { expression }).Compile();
            }
            catch (Exception exception)
            {
                string msg = exception.Message;
                return delegate
                {
                    throw new InvalidOperationException(msg);
                };
            }
        }

        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(Func<Expression, Expression, BinaryExpression> body)
        {
            return CreateExpression<TArg1, TArg2, TResult>(body, false);
        }

        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(Func<Expression, Expression, BinaryExpression> body, bool castArgsToResultOnFailure)
        {
            Func<TArg1, TArg2, TResult> func;
            try
            {
                ParameterExpression expression=null;
                ParameterExpression expression2=null;
                try
                {
                    func = Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(expression = Expression.Parameter(typeof(TArg1), "lhs"), expression2 = Expression.Parameter(typeof(TArg2), "rhs")), new ParameterExpression[] { expression, expression2 }).Compile();
                }
                catch (InvalidOperationException)
                {
                    if (!castArgsToResultOnFailure || ((typeof(TArg1) == typeof(TResult)) && (typeof(TArg2) == typeof(TResult))))
                    {
                        throw;
                    }
                    Expression expression3 = (typeof(TArg1) == typeof(TResult)) ? ((Expression)expression) : ((Expression)Expression.Convert(expression, typeof(TResult)));
                    Expression expression4 = (typeof(TArg2) == typeof(TResult)) ? ((Expression)expression2) : ((Expression)Expression.Convert(expression2, typeof(TResult)));
                    func = Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(expression3, expression4), new ParameterExpression[] { expression, expression2 }).Compile();
                }
            }
            catch (Exception exception)
            {
                string msg = exception.Message;
                func = delegate
                {
                    throw new InvalidOperationException(msg);
                };
            }
            return func;
        }

    }
}
