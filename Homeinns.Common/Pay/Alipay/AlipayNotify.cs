﻿using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Homeinns.Common.Pay
{
    /// <summary>
    /// 类名：Notify
    /// 功能：支付宝通知处理类
    /// 详细：处理支付宝各接口通知返回
    /// 版本：3.2
    /// 修改日期：2011-03-17
    /// //////////////////////注意/////////////////////////////
    /// 调试通知返回时，可查看或改写log日志的写入TXT里的数据，来检查通知返回是否正常 
    /// </summary>
    public class AlipayNotify
    {
        #region 字段
        private string _partner = "";               //合作身份者ID
        private string _key = "";                   //交易安全校验码
        private string _input_charset = "";         //编码格式
        private string _sign_type = "";             //签名方式

        //支付宝通知验证路径
        private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        #endregion

        /// <summary>
        /// 构造函数
        /// 从配置文件中初始化变量
        /// </summary>
        public AlipayNotify()
        {
            //初始化基础配置信息
            _partner = AlipayConfig.partner.Trim();
            _key = AlipayConfig.key.Trim().ToLower();
            _input_charset = AlipayConfig.input_charset.Trim().ToLower();
            _sign_type = AlipayConfig.sign_type.Trim().ToUpper();
        }

        /// <summary>
        ///  验证消息是否是支付宝发出的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notify_id">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
        {
            //获取返回回来的待签名数组签名后结果
            string mysign = GetResponseMysign(inputPara);
            //获取是否是支付宝服务器发来的请求的验证结果
            string responseTxt = GetResponseTxt(notify_id);

            //写日志记录（若要调试，请取消下面两行注释）
            string sWord = "responseTxt=" + responseTxt + "\n sign=" + sign + "&mysign=" + mysign + "\n 返回回来的参数：" + GetPreSignStr(inputPara) + "\n ";
            AlipayCore.LogResult(sWord);

            //判断responsetTxt是否为true，生成的签名结果mysign与获得的签名结果sign是否一致
            //responsetTxt的结果不是true，与服务器设置问题、合作身份者ID、notify_id一分钟失效有关
            //mysign与sign不等，与安全校验码、请求时的参数格式（如：带自定义参数等）、编码格式有关
            if (responseTxt == "true" && sign == mysign)//验证成功
            {
                return true;
            }
            else//验证失败
            {
                return false;
            }
        }

        /// <summary>
        /// 获取待签名字符串（调试用）
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <returns>待签名字符串</returns>
        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //过滤空值、sign与sign_type参数
            sPara = AlipayCore.FilterPara(inputPara);
            //获取待签名字符串
            string preSignStr = AlipayCore.CreateLinkString(sPara);
            return preSignStr;
        }

        /// <summary>
        /// 获取返回回来的待签名数组签名后结果
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <returns>签名结果字符串</returns>
        private string GetResponseMysign(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //过滤空值、sign与sign_type参数
            sPara = AlipayCore.FilterPara(inputPara);
            //获得签名结果
            string mysign = AlipayCore.BuildMysign(sPara, _key, _sign_type, _input_charset);
            return mysign;
        }

        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notify_id">通知验证ID</param>
        /// <returns>验证结果</returns>
        private string GetResponseTxt(string notify_id)
        {
            string veryfy_url = String.Format("{0}partner={1}&notify_id={2}", Https_veryfy_url, _partner, notify_id);
            //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
            string responseTxt = Get_Http(veryfy_url, 120000);
            return responseTxt;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }
    }
}