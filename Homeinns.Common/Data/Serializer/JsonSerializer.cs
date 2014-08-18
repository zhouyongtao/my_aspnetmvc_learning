using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using Homeinns.Common.Data.Serialize;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  JSON辅助类
 * @date 2012年9月10日10:56:28
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Data.Serializer
{
    /// <summary>
    /// Json辅助类
    /// </summary>
    public class JsonSerializer
    {
        private readonly static JavaScriptSerializer jss = new JavaScriptSerializer();

        #region 构造函数
        static JsonSerializer()
        {
            //注册时间转换器
            jss.RegisterConverters(new[] { new DateTimeConverter() });
        }
        #endregion

        /// <summary>
        /// JSON转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonText)
        {
            return jss.Deserialize<T>(jsonText);
            //return JsonConvert.DeserializeObject<T>(jsonText, new IsoDateTimeConverter());

        }

        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            /*
              var settings = new JsonSerializerSettings();
              settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy年MM月dd日 HHmmss" });
             */

            return JsonConvert.SerializeObject(obj, new JavaScriptDateTimeConverter());
            //return jss.Serialize(obj);
        }

        /// <summary>
        /// DataTable转JSON
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SerializeDataTable(DataTable source)
        {
            return JsonConvert.SerializeObject(source, new DataTableConverter());
        }


        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>Json格式的字符串</returns>
        public static string ObjectToJson(object obj)
        {
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {

                throw new Exception("JsonHelper.ObjectToJson(): " + ex.Message);
            }
        }

        /// <summary>
        /// 数据表转键值对集合
        /// 把DataTable转成 List集合, 存每一行
        /// 集合中放的是键值对字典,存每一列
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>哈希表数组</returns>
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 数据集转键值对数组字典
        /// </summary>
        /// <param name="dataSet">数据集</param>
        /// <returns>键值对数组字典</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));

            return result;
        }

        /// <summary>
        /// 数据表转Json
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>Json字符串</returns>
        public static string DataTableToJson(DataTable dt)
        {
            return ObjectToJson(DataTableToList(dt));
        }

        /// <summary>
        /// Json文本转对象,泛型方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="JsonText">Json文本</param>
        /// <returns>指定类型的对象</returns>
        public static T JsonToObject<T>(string JsonText)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                return js.Deserialize<T>(JsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JsonHelper.JsonToObject(): " + ex.Message);
            }
        }

        /// <summary>
        /// 将Json文本转换为数据表数据
        /// </summary>
        /// <param name="JsonText">Json文本</param>
        /// <returns>数据表字典</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJson(string JsonText)
        {
            return JsonToObject<Dictionary<string, List<Dictionary<string, object>>>>(JsonText);
        }

        /// <summary>
        /// 将Json文本转换成数据行
        /// </summary>
        /// <param name="JsonText">Json文本</param>
        /// <returns>数据行的字典</returns>
        public static Dictionary<string, object> DataRowFromJson(string JsonText)
        {
            return JsonToObject<Dictionary<string, object>>(JsonText);
        }
    }
}
