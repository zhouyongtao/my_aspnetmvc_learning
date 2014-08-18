using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Homeinns.Common.Base.Web
{
    /// <summary>
    /// Cookie操作
    /// </summary>
    public class CookieUtil
    {
        /// <summary>
        ///设置Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresDay"></param>
        public static void SetCookieValue(string key, string value, double expiresDay)
        {
            HttpContext.Current.Response.Cookies.Set(new HttpCookie(key, value) { Expires = DateTime.Now.AddDays(expiresDay) });
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiresDay">天数</param>
        public static void SetCookieValue(string name, string key, string value, double expiresDay)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Values.Add(key, value);
            cookie.Expires = DateTime.Now.AddDays(expiresDay);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie GetCookieValue(string name)
        {
            return HttpContext.Current.Request.Cookies.Get(name) ?? null;
        }
        /// <summary>
        ///移除Cookie值
        /// </summary>
        /// <param name="cookie"></param>
        public static void RemoveCookieValue(string name)
        {
            HttpCookie myCookie = GetCookieValue(name);
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
    }
}
