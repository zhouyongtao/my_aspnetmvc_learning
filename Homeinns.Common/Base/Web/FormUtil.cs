using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
namespace Homeinns.Common.Base.Web
{
    /// <summary>
    /// Form票据验证
    /// </summary>
    public class FormUtil
    {
        /// <summary>
        /// 判断当前用户是否为验证用户
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated) //HttpContext.Current.Request.IsAuthenticated
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获得用户标识(用户ID)
        /// </summary>
        /// <returns></returns>
        public static string GetIdentityName()
        {
            if (IsAuthenticated())
            {
                return HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 保存票据
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="day">票据有效时间</param>
        public static void SetAuthCookie(string userID, int expiresDay)
        {
            if (!IsAuthenticated())
            {
                FormsAuthentication.SetAuthCookie(userID, true);
                HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(expiresDay);
            }
        }
        /// <summary>
        /// 保存票据
        /// </summary>
        /// <param name="userID"></param>
        public static void SetAuthCookie(string userID)
        {
            if (!IsAuthenticated())
            {
                FormsAuthentication.SetAuthCookie(userID, false);
            }
        }
    }
}
