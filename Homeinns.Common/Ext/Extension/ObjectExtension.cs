using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description 扩展object
 * @date 2010年9月10日18:30:25
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ext.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(this object obj)
        {
            //注册其他转换器
            // return fastJSON.JSON.Instance.ToJSON(obj);
            return Homeinns.Common.Data.Serializer.JsonSerializer.Serialize(obj);
            //return Homeinns.Common.Data.Serializer.JsonSerializer.ObjectToJson(obj);
            // return new JavaScriptSerializer().Serialize(obj);
        }

        public static bool IsNull(this object x)
        {
            return x == null;
        }

        public static void ThrowIfNull<T>(this T data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowIfNull<T>(this T data, string name) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
