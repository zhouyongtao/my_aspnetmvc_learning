using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description Ajax请求的统一返回结果类
 * @date 2013‎年‎9‎月‎14‎日 ‏‎15:15:43
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer 
 */
namespace Homeinns.Common.Ajax
{
    [Serializable]
    public class AjaxResult
    {
        private bool isError = false;          //是否发生错误
        private int _total;                    //数据库中的记录总数

        /// <summary>
        /// 数据库中的记录总数
        /// </summary>
        [ScriptIgnore]
        public int Total
        {
            get { return _total; }
        }

        //默认的构造函数
        private AjaxResult()
        {
        }

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return isError; } }

        /// <summary>
        /// 错误信息，或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        #region Error
        /// <summary>
        /// 设置标识错误,并给定错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>操作结果</returns>
        public static AjaxResult Error(string message)
        {
            return new AjaxResult()
            {
                isError = true,
                Message = message
            };
        }
        #endregion

        #region Success
        /// <summary>
        /// 调用该方法,设置操作成功!
        /// </summary>
        /// <returns>操作结果</returns>
        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
                isError = false
            };
        }

        /// <summary>
        /// 设置操作成功,并给定成功信息
        /// </summary>
        /// <param name="message">成功信息</param>
        /// <returns>操作结果</returns>
        public static AjaxResult Success(string message)
        {
            return new AjaxResult()
            {
                isError = false,
                Message = message
            };
        }

        /// <summary>
        /// 设置操作成功,设置操作返回结果
        /// </summary>
        /// <param name="data">操作成功返回的数据</param>
        /// <returns>操作结果</returns>
        public static AjaxResult Success(object data)
        {
            return new AjaxResult()
            {
                isError = false,
                Data = data
            };
        }

        /// <summary>
        /// 设置操作成功,设置操作返回结果
        /// </summary>
        /// <param name="data">从数据库中读取的DataTable数据</param>
        /// <param name="total">记录总数</param>
        /// <returns>操作结果</returns>
        public static AjaxResult Success(DataTable data, int total)
        {
            DataTable dt = data as DataTable;
            string dataTableJson;      //JSON字符串
            if (dt == null)
                dataTableJson = "[]";
            else
                dataTableJson = "";
            AjaxResult result = new AjaxResult()
            {
                _total = total,            //结记录总数
                isError = false,
            };
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        builder.Append(String.Format("\"{0}\":{1}", dt.Columns[j].ColumnName, new JavaScriptSerializer().Serialize(dt.Rows[i][j])));
                        if (j < (dt.Columns.Count - 1))
                        {
                            builder.Append(",");                  //将DataTable数据拼接成JSON数据
                        }
                    }
                    builder.Append("}");
                    if (i < (dt.Rows.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("]");
            dataTableJson = builder.ToString();
            result.Data = String.Format(@"{{Rows:{0},Total:{1}}}", dataTableJson, result.Total);
            return result;
        }

        /// <summary>
        /// 操作成功,设置成功信息及返回结果
        /// </summary>
        /// <param name="data">操作返回的结果</param>
        /// <param name="message">成功信息</param>
        /// <returns>操作结果</returns>
        public static AjaxResult Success(object data, string message)
        {
            return new AjaxResult()
            {
                isError = false,
                Data = data,
                Message = message
            };
        }
        #endregion

        /// <summary>
        /// 设置操作成功,设置操作返回结果
        /// </summary>
        /// <param name="data">操作成功返回的数据</param>
        /// <returns>操作结果</returns>
        public static AjaxResult SuccessNewton(object data)
        {
            return new AjaxResult()
            {
                isError = false,
                Data = fastJSON.JSON.Instance.ToJSON(data)
            };
        }

        /// <summary>
        /// 将结果序列化为JSON数据
        /// </summary>
        /// <returns>JSON数据</returns>
        public override string ToString()
        {
            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            //注册自定义的转换器
            //jsSerializer.RegisterConverters(new[] { new  DateTimeConverter() });
            //return JsonConvert.SerializeObject(this, new JavaScriptDateTimeConverter());
            return new JavaScriptSerializer().Serialize(this);
            //return JSONHelper.Serialize(this);
        }
    }
}