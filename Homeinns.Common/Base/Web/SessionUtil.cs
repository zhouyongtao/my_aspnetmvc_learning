using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Homeinns.Common.Base.Web
{
    /// <summary>
    /// Session 操作类
    /// 1、GetSession(string name)根据session名获取session对象
    /// 2、SetSession(string name, object val)设置session
    /// </summary>
    public class SessionUtil
    {
        /// <summary>
        /// 移除指定Session
        /// </summary>
        /// <param name="name"></param>
        public static void ClearSession(string name)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session.Remove(name);
            }
        }
        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                return HttpContext.Current.Session[name];
            }
            else
            {
                return "";//从数据库中重新查询
            }
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(string name, object val)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session.Remove(name);
            }
            HttpContext.Current.Session.Add(name, val);
        }
    }
}

