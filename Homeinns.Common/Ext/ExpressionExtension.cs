using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/*
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description Expression扩展
 * @date 2013‎年‎9‎月‎20‎日 ‏‎19:30:49
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ext
{
    public static class ExpressionExtension
    {
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body, params  ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }
    }
}