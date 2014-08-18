/******************************支付宝接口调用方式*******************************/
//单笔交易查询接口
AlipayService service = new AlipayService();
SortedDictionary<String, String> param = new SortedDictionary<string, string>();
param.Add("trade_no", "2013103063963820");
string url = service.create_single_trade_query(param);
string result = HttpUtil.HttpProxy(url);

//支付宝账务明细分页查询接口
 SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("logon_id", AlipayConfig.seller_email);
            sParaTemp.Add("page_no", "1");
            sParaTemp.Add("trade_no", "2013103063963820");
            //sParaTemp.Add("gmt_start_time", gmt_start_time);
            //sParaTemp.Add("gmt_end_time", gmt_end_time);

//构造账务明细分页查询接口，无需修改
            AlipayService service = new AlipayService();
            XmlDocument xmlDoc = service.create_account_page_query(sParaTemp);
            StringBuilder sbxml = new StringBuilder();
            string nodeIs_success = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
            if (nodeIs_success != "T")//请求不成功的错误信息
            {
                sbxml.Append("错误：" + xmlDoc.SelectSingleNode("/alipay/error").InnerText);
            }
            else//请求成功的支付返回宝处理结果信息
            {
                sbxml.Append(xmlDoc.SelectSingleNode("/alipay/response").InnerText);
            }
            Response.Write(sbxml.ToString());