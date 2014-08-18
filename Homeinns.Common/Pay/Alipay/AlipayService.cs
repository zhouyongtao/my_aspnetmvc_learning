using System.Text;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;

namespace Homeinns.Common.Pay
{
    /// <summary>
    /// 类名：Service
    /// 功能：支付宝各接口构造类
    /// 详细：构造支付宝各接口请求参数
    /// 版本：3.2
    /// 修改日期：2011-03-17
    /// 说明：
    /// 要传递的参数要么不允许为空，要么就不要出现在数组与隐藏控件或URL链接里。
    /// </summary>
    public class AlipayService
    {
        #region 字段
        //合作者身份ID
        private readonly string _partner = string.Empty;
        //字符编码格式
        private readonly string _input_charset = string.Empty;

        //支付宝网关地址（新）
        private const string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        //支付宝通知验证路径
        //public static string GATEWAY_NEW = String.Format("{0}_input_charset={1}", "https://mapi.alipay.com/gateway.do?", AlipayConfig.input_charset);

        #endregion

        /// <summary>
        /// 构造函数
        /// 从配置文件及入口文件中初始化变量
        /// </summary>
        public AlipayService()
        {
            _partner = AlipayConfig.partner.Trim();
            _input_charset = AlipayConfig.input_charset.Trim().ToLower();
        }


        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public string Query_timestamp()
        {
            string url = String.Format("{0}service=query_timestamp&partner={1}", GATEWAY_NEW, _partner);
            string encrypt_key = "";
            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);
            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;
            return encrypt_key;
        }

        /*##################################################支付宝相关接口##############################################*/

        /// <summary>
        /// 即时到账交易接口(构建表单)
        /// </summary>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public string create_direct_pay_by_user(SortedDictionary<String, String> sParaTemp)
        {
            //增加基本配置
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset);
            return AlipaySubmit.BuildFormHtml(sParaTemp, GATEWAY_NEW, "POST", "确认");
        }


        /// <summary>
        /// 构造即时到账批量退款无密接口
        /// </summary>
        /// <param name="sParaTemp">请求参数集合</param>
        /// <returns>返回XML处理结果</returns>
        public XmlDocument Refund_fastpay_by_platform_nopwd(SortedDictionary<string, string> sParaTemp)
        {
            //增加基本配置
            sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset);
            //获取支付宝的返回XML处理结果
            XmlDocument strHtml = new XmlDocument();
            strHtml = AlipaySubmit.SendPostInfo(sParaTemp, GATEWAY_NEW);
            return strHtml;
        }


        /// <summary>
        /// 账务明细分页查询接口(HTTP POST方式)
        /// </summary>
        /// <param name="sParaTemp">请求参数集合</param>
        /// <returns>支付宝的返回XML处理结果</returns>
        public XmlDocument create_account_page_query(SortedDictionary<string, string> sParaTemp)
        {
            //增加基本配置
            sParaTemp.Add("service", "account.page.query");
            sParaTemp.Add("partner", _partner);
            sParaTemp.Add("_input_charset", _input_charset);
            //获取支付宝的返回XML处理结果
            XmlDocument strHtml = new XmlDocument();
            strHtml = AlipaySubmit.SendPostInfo(sParaTemp, GATEWAY_NEW);
            return strHtml;
        }


        /// <summary>
        /// 单笔交易查询接口(HTTP GET方式)
        /// </summary>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public string create_single_trade_query(SortedDictionary<String, String> sParaTemp)
        {
            //增加基本配置
            sParaTemp.Add("service", "single_trade_query");
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset);
            return GATEWAY_NEW + AlipaySubmit.BuildRequestParaToString(sParaTemp, Encoding.GetEncoding(_input_charset));
        }


        /*##################################################支付宝相关接口###########################################*/


        #region 若要增加其他支付宝接口，可以按照下面的格式定义
        /// <summary>
        /// 构造(支付宝接口名称)接口
        /// </summary>
        /// <param name="sParaTemp">请求参数集合</param>
        /// <returns>表单提交HTML文本或者支付宝返回XML处理结果</returns>
        public string AlipayInterface(SortedDictionary<string, string> sParaTemp)
        {
            //增加基本配置

            //表单提交HTML数据变量
            string strHtml = "";


            //构造请求参数数组


            //构造给支付宝处理的请求
            //请求方式有以下三种：
            //1.构造表单提交HTML数据:Submit.BuildFormHtml()
            //2.构造模拟远程HTTP的POST请求，获取支付宝的返回XML处理结果:Submit.SendPostInfo()
            //3.构造模拟远程HTTP的GET请求，获取支付宝的返回XML处理结果:Submit.SendGetInfo()
            //请根据不同的接口特性三选一


            return strHtml;
        }
        #endregion

        #region 支付宝辅助函数

        /// <summary>
        /// 根据银行编码获得银行名称
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public static string GetBankName(string bankType)
        {
            string bankName = string.Empty;
            if ("ICBCB2C".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国工商银行";
            }
            else if ("CCB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国建设银行";
            }
            else if ("ABC".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国农业银行";
            }
            else if ("COMM".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "交通银行";
            }
            else if ("CMB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "招商银行";
            }
            else if ("BOCB2C".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国银行";
            }
            else if ("CITIC".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中信银行";
            }
            else if ("SDB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "深圳发展银行";
            }
            else if ("SPDB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "浦发银行";
            }
            else if ("CIB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "兴业银行";
            }
            else if ("SPABANK".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "平安银行";
            }
            else if ("GDB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "广发银行";
            }
            else if ("CEBBANK".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国光大银行";
            }
            else if ("CMBC".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国民生银行";
            }
            else if ("SHBANK".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "上海银行";
            }
            else if ("POSTGC".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "中国邮政储蓄银行";
            }
            else if ("BJRCB".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "北京农村商业银行";
            }
            else if ("ZFBZF".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "支付宝支付";
            }
            else if ("ZFBZFWX".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "支付宝扫码支付";
            }
            else if ("NBBANK".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "宁波银行";
            }
            else if ("HZCBB2C".Equals(bankType, StringComparison.OrdinalIgnoreCase))
            {
                bankName = "杭州银行";
            }
            return bankName;
        }
        #endregion
    }
}