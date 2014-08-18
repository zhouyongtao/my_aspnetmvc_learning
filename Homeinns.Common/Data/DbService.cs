using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data;
using Homeinns.Common.Util;
using Homeinns.Common.Log;


/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description 数据库连接相关
 * @date 2010年6月08日20:15:05
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Data
{

    /// <summary>
    /// 数据库辅助类
    /// </summary>
    public class DbService
    {
        /// <summary>
        /// 获得数据库连接(默认DBSetting.ConText配置)
        /// </summary>
        /// <returns></returns>
        public static IDbConnection OpenConnection()
        {
            IDbConnection conn = null;
            try
            {
                conn = new SqlConnection(DBSetting.Hominns);
                conn.Open();
            }
            catch (Exception ex)
            {
                NRLog.ExceptionLog("DBService OpenConnection: ", ex);
            }
            return conn;
        }

        /// <summary>
        /// 获得数据库连接
        /// </summary>
        /// <param name="connText">数据库连接参数(必填)</param>
        /// <returns></returns>
        public static IDbConnection OpenConnection(string connText)
        {
            if (connText == null || connText.Length == 0)
                throw new ArgumentNullException("connText");
            IDbConnection conn = null;
            try
            {
                conn = new SqlConnection(connText);
                conn.Open();
            }
            catch (Exception ex)
            {
                NRLog.ExceptionLog("DBService OpenConnection: ", ex);
            }
            return conn;
        }

        /*
         /// <summary>
         /// 获取MySql的连接数据库对象
         /// </summary>
         /// <param name="connText"></param>
         /// <returns></returns>
         public MySqlConnection OpenConnection(string connText)
         {
             MySqlConnection connection = new MySqlConnection(connText);
             connection.Open();
             return connection;
         }
         */


        /*
        public  T Persistent<TProperty>(T entity, Expression<Func<TProperty>> propertyExpression)
        {
            var type = typeof(T);
            string tableName = type.Name;
            string propertyName = propertyExpression.GetMemberInfo().Name;
            var value = propertyExpression.GetValue();
            var id = type.GetProperty("Id").GetValue(entity, null);

            //string.Format("update {0} set {1}=:value where Id=:id", tableName, propertyName))
            return entity;
        }
         */
    }
}
