using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Homeinns.Common.Pay
{
    /// <summary>
    /// 类名：Submit
    /// 功能：支付宝各接口请求提交类
    /// 详细：构造支付宝各接口表单HTML文本，获取远程HTTP数据
    /// 版本：3.2
    /// 修改日期：2011-03-17
    /// </summary>
    public class AlipaySubmit
    {
        private static string _key = string.Empty;
        private static string _input_charset = string.Empty;
        private static string _sign_type = string.Empty;

        static AlipaySubmit()
        {
            _key = AlipayConfig.key.Trim().ToLower();
            _input_charset = AlipayConfig.input_charset.Trim().ToLower();
            _sign_type = AlipayConfig.sign_type.Trim().ToUpper();
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            var sPara = new Dictionary<string, string>();
            var mysign = string.Empty;
            sPara = AlipayCore.FilterPara(sParaTemp);
            mysign = AlipayCore.BuildMysign(sPara, _key, _sign_type, _input_charset);
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);
            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            var sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp);
            var strRequestData = AlipayCore.CreateLinkStringUrlencode(sPara, code);
            return strRequestData;
        }

        /// <summary>
        /// 构造提交表单HTML数据
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildFormHtml(SortedDictionary<string, string> sParaTemp, string gateway, string strMethod, string strButtonValue)
        {
            var dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);
            var sbHtml = new StringBuilder();
            sbHtml.Append(String.Format("<form id='alipaysubmit' name='alipaysubmit' action='{0}_input_charset={1}' method='{2}'>", gateway, _input_charset, strMethod.ToLower().Trim()));
            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append(String.Format("<input type='hidden' name='{0}' value='{1}'/>", temp.Key, temp.Value));
            }
            sbHtml.Append(String.Format("<input type='submit' value='{0}' style='display:none;'></form>", strButtonValue));
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return sbHtml.ToString();
        }

        /// <summary>
        /// 构造模拟远程HTTP的POST请求，获取支付宝的返回XML处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <returns>支付宝返回XML处理结果</returns>
        public static XmlDocument SendPostInfo(SortedDictionary<string, string> sParaTemp, string gateway)
        {
            var code = Encoding.GetEncoding(_input_charset);
            var strRequestData = BuildRequestParaToString(sParaTemp, code);
            var bytesRequestData = code.GetBytes(strRequestData);
            var strUrl = String.Format("{0}_input_charset={1}", gateway, _input_charset);
            var xmlDoc = new XmlDocument();
            try
            {
                var myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.ContentLength = bytesRequestData.Length;
                var requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                var HttpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = HttpWResp.GetResponseStream();
                var Reader = new XmlTextReader(myStream);
                xmlDoc.Load(Reader);
            }
            catch (Exception exp)
            {
                var strXmlError = String.Format("<error>{0}</error>", exp.Message);
                xmlDoc.LoadXml(strXmlError);
            }
            return xmlDoc;
        }

        /// <summary>
        /// 构造模拟远程HTTP的GET请求，获取支付宝的返回XML处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <returns>支付宝返回XML处理结果</returns>
        public static XmlDocument SendGetInfo(SortedDictionary<string, string> sParaTemp, string gateway)
        {
            var code = Encoding.GetEncoding(_input_charset);
            var strRequestData = BuildRequestParaToString(sParaTemp, code);
            var strUrl = gateway + strRequestData;
            var xmlDoc = new XmlDocument();
            try
            {
                var myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "get";

                var HttpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = HttpWResp.GetResponseStream();

                var Reader = new XmlTextReader(myStream);
                xmlDoc.Load(Reader);
            }
            catch (Exception exp)
            {
                var strXmlError = String.Format("<error>{0}</error>", exp.Message);
                xmlDoc.LoadXml(strXmlError);
            }
            return xmlDoc;
        }
    }
}
