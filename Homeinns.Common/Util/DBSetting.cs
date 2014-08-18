using System;
using System.Collections.Generic;
using System.Linq;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description 数据库配置
 * @date ‎2013‎年‎8‎月‎17‎日 ‎17:47:32
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Util
{
    /// <summary>
    /// DB配置
    /// </summary>
    public class DBSetting
    {

        /// <summary>
        /// Hominns 默认数据库配置(38)
        /// </summary>
        public static string Hominns
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Hominns"].ConnectionString; }
        }

        /// <summary>
        /// 地图信息(38)
        /// </summary>
        public static string Mapbar
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Mapbar"].ConnectionString; }
        }

        /// <summary>
        /// bbs(136)
        /// </summary>
        public static string BBS
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["BBS"].ConnectionString; }
        }
        

        /// <summary>
        /// CMS数据库
        /// </summary>
        public static string CMS
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["CMS"].ConnectionString; }
        }

        /// <summary>
        /// Motel 默认数据库配置
        /// </summary>
        public static string Motel
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Motel"].ConnectionString; }
        }

        /// <summary>
        /// Trans 默认数据库配置(直连酒店数据 订单冲账)
        /// </summary>
        public static string HomeinnsTrans
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["HomeinnsTrans"].ConnectionString; }
        }

        /// <summary>
        /// Trans 默认数据库配置(直连酒店数据 订单冲账)
        /// </summary>
        public static string MotelTrans
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["MotelTrans"].ConnectionString; }
        }

        /// <summary>
        /// Log 默认数据库配置（接口日志）
        /// </summary>
        public static string Log
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Log"].ConnectionString; }
        }

        /// <summary>
        /// Task 默认数据库配置(暂时没有用)
        /// </summary>
        public static string Task
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Task"].ConnectionString; }
        }

        /// <summary>
        /// Member 默认数据库配置（联系人）
        /// </summary>
        public static string Member
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Member"].ConnectionString; }
        }


        /// <summary>
        /// Member 默认数据库配置（房态与价格）
        /// </summary>
        public static string PriceAndRmFlow
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["PriceAndRmFlow"].ConnectionString; }
        }

        /// <summary>
        /// 深度点评
        /// </summary>
        public static string Comment
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Comment"].ConnectionString; }
        }

        /// <summary>
        /// 客史
        /// </summary>
        public static string TransHis
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["TransHis"].ConnectionString; }
        }


        public static string HCS
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["HCS"].ConnectionString; }
        }


        /// <summary>
        /// 获取164配置字符串
        /// </summary>
        public static string Mysql
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString; }
        }

        /// <summary>
        /// 获取DB配置字符串
        /// </summary>
        public static string getConText(string conText)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[conText].ConnectionString;
        }

        /// <summary>
        /// 获取AppSettings配置字符串
        /// </summary>
        public static string getAppText(string appName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[appName];
        }

        #region 其他配置
        /// <summary>
        /// 获取DB配置字符串
        /// </summary>
        public static string ConManagerText
        {
            get { return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConText"].ConnectionString; }
        }

        /// <summary>
        /// 获取DB配置字符串
        /// </summary>
        public static string ConDBText(string conText, params object[] args)
        {
            if (args == null || args.Length == 0)
                return System.Configuration.ConfigurationManager.AppSettings[conText];
            else
                return System.Configuration.ConfigurationManager.ConnectionStrings[conText].ConnectionString;
        }
        #endregion

        #region 本地Debug
#if DEBUG
        /// <summary>
        /// Debug
        /// </summary>
        public static string Debug
        {
            get { return "Data Source=.;Initial Catalog=CRS;User ID=sa;Password=123"; }
        }
#endif
        #endregion
    }
}