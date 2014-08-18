using System;
using System.Web;
/*
     NickName:Irving
     Email:zhouyontao@outlook.com
     Date:2012年2月15日12:20:51
 */
namespace Homeinns.Common.Base.Web
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public class CacheUtil
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object obj)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, obj);
        }
    }
}
