using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description Ajax方法属性
 * @date 2013‎年‎9‎月‎14‎日 ‏‎15:15:43
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ajax
{
    /// <summary>
    /// 内部 Ajax 方法 容器
    /// </summary>
    internal class AjaxAction
    {
        /// <summary>
        /// 方法信息(系统定义)
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// 发现参数属性并提供对参数元数据的访问。方法参数集合(系统定义)
        /// </summary>
        public ParameterInfo[] Parameters { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="method">方法</param>
        public AjaxAction(MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters(); //获取该方法参数
        }
    }
}
