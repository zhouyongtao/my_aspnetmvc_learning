using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description Ajax类容器对象
 * @date 2013‎年‎9‎月‎14‎日 ‏‎15:15:43
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ajax
{
    internal class AjaxTypeContainer
    {
        private static readonly BindingFlags ActionBindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        /// <summary>
        /// Ajax类型
        /// </summary>
        public Type AjaxClass { get; private set; }

        /// <summary>
        /// 方法集合
        /// </summary>
        public List<AjaxAction> Actions { get; private set; }

        /// <summary>
        /// 初始化Ajax类容器对象
        /// </summary>
        /// <param name="t">类型</param>
        public AjaxTypeContainer(Type t)
        {
            AjaxClass = t;
            Actions = new List<AjaxAction>();
            var methods = t.GetMethods(ActionBindingFlags);
            foreach (MethodInfo method in methods)
            {
                if (new string[] { "ToString", "GetHashCode", "Equals", "MemberwiseClone", "GetType" }.Contains(method.Name))
                {
                    continue;
                }
                Actions.Add(new AjaxAction(method));
            }
        }

        /// <summary>
        /// 根据给定的方法名称,获取方法实体对象
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <returns>返回方法</returns>
        public AjaxAction GetAction(string methodName)
        {
            return Actions.Where(c => c.Method.Name == methodName).FirstOrDefault();
        }

        /// <summary>
        /// 根据给定的方面名称获取方法对象
        /// </summary>
        /// <param name="methodName">给定的方法名称</param>
        /// <returns>获取的方法(系统定义的对象)</returns>
        public MethodInfo GetMethod(string methodName)
        {
            var action = Actions.Where(c => c.Method.Name == methodName).FirstOrDefault();
            if (action == null)
            {
                return null;
            }
            return action.Method;
        }
    }
}
