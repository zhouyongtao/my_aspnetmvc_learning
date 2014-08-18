using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.Linq;
using System.Collections.Specialized;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description Ajax辅助方法
 * @date 2013‎年‎9‎月‎14‎日 ‏‎15:15:43
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ajax
{
    public class AjaxRequestHelper
    {
        #region 初始化加载
        private readonly static List<AjaxTypeContainer> ajaxTypeContainerList = new List<AjaxTypeContainer>();

        //默认的构造函数,初始化 ajaxTypeContainerList 集合
        static AjaxRequestHelper()
        {
            try
            {
                Assembly assembly = Assembly.Load(new AssemblyName("TaskManagerService"));  //在此处指定访问的程序集名称

                //GetExportedTypes():获取此程序集中定义的公共类型，这些公共类型在程序集外可见。
                foreach (Type t in assembly.GetExportedTypes())      //获取公共的类成员
                {
                    ajaxTypeContainerList.Add(new AjaxTypeContainer(t));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AjaxRequestHelper.cs中获取程序集时发生异常,信息: " + ex.Message);
            }
        }
        #endregion

        #region 获取方法的参数值
        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static object[] GetMethodParms(AjaxAction action, HttpContext context)
        {
            return GetMethodParms(action.Parameters, context);
        }

        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="method"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object[] GetMethodParms(MethodInfo method, HttpContext context)
        {
            var parms = method.GetParameters();
            return GetMethodParms(parms, context);
        }
        /// <summary>
        /// 获取方法的参数值
        /// </summary>
        /// <param name="parms">获取到的给定方法的参数集合</param>
        /// <param name="context">B端发来的请求</param>
        /// <returns></returns>
        public static object[] GetMethodParms(ParameterInfo[] parms, HttpContext context)
        {
            var objs = new object[parms.Length]; //保存参数类型
            for (var i = 0; i < parms.Length; i++)
            {
                ParameterInfo parm = parms[i];

                if (parm.ParameterType == typeof(NameValueCollection))  //string:stirng 的键值对
                {
                    if (string.Compare(parm.Name, "Form", true) == 0)
                        objs[i] = context.Request.Form;
                    else if (string.Compare(parm.Name, "QueryString", true) == 0)
                        objs[i] = context.Request.QueryString;
                }
                else
                {
                    Type paramterType = parm.ParameterType;
                    if (parm.ParameterType.IsGenericType)  //是否为泛型
                        paramterType = Nullable.GetUnderlyingType(parm.ParameterType) ?? parm.ParameterType;
                    if (TypeHelper.IsSimpleType(paramterType))
                    {
                        objs[i] = GetMethodParmValue(context.Request, parm);
                    }
                    else
                    {
                        //如果这个参数 是自定义的类，那么实例化一个对象，然后使用 Request键值对 赋值
                        var obj = Activator.CreateInstance(paramterType);

                        //----------------参数类型分析完毕---------------------

                        MemberInfo[] members = parm.ParameterType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
                        foreach (MemberInfo mi in members)
                        {
                            if (mi.MemberType == MemberTypes.Method || !TypeHelper.IsFieldOrProperty(mi) || TypeHelper.IsReadOnly(mi))
                                continue;
                            var val = GetMethodParmValue(context.Request, mi.Name, TypeHelper.GetMemberType(mi));
                            if (val != null)
                            {
                                TypeHelper.SetValue(mi, obj, val);
                            }
                        }
                        objs[i] = obj;
                    }
                }
            }
            return objs;
        }

        /// <summary>
        /// 获取方法单个参数的值
        /// </summary>
        /// <param name="request">前台请求</param>
        /// <param name="parmName">参数名称</param>
        /// <param name="parmType">参数类型</param>
        /// <returns></returns>
        public static object GetMethodParmValue(HttpRequest request, string parmName, Type parmType)
        {
            if (string.IsNullOrEmpty(request[parmName])) return null;
            if (!TypeHelper.IsSimpleType(parmType)) return null;
            return DataHelper.ConvertValue(parmType, request[parmName]);
        }

        /// <summary>
        /// 获取方法单个参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static object GetMethodParmValue(HttpRequest request, ParameterInfo parm)
        {
            return GetMethodParmValue(request, parm.Name, parm.ParameterType);
        }

        #endregion

        #region 获取类、方法定义
        public static Type GetType(string typeName)
        {
            foreach (var c in ajaxTypeContainerList)
            {
                var type = c.AjaxClass;
                if (type == null || type.Name != typeName) continue;
                return type;
            }
            return null;
        }

        /// <summary>
        /// 根据类名及方法名,获取方法信息
        /// </summary>
        /// <param name="typeName">类名</param>
        /// <param name="methodName">方法名</param>
        /// <returns>获取的方法信息</returns>
        public static MethodInfo GetMethod(string typeName, string methodName)
        {
            foreach (var c in ajaxTypeContainerList)
            {
                var type = c.AjaxClass;
                if (type == null || type.Name != typeName) continue;
                return c.GetMethod(methodName);
            }
            return null;
        }
        #endregion

    }
}