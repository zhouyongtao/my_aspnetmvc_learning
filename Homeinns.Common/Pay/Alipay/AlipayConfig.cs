using System;
using System.Collections.Generic;
using Homeinns.Common.Util;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description 支付宝接口
 * @date 2013年10月18日16:05:31
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Pay
{
    /// <summary>
    /// 支付宝接口基础配置类
    /// </summary>
    public class AlipayConfig
    {
        //合作身份者ID，以2088开头由16位纯数字组成的字符串
        public static string partner = "";
        //交易安全检验码，由数字和字母组成的32位字符串
        public static string key = "";
        //字符编码格式 目前支持 gbk 或 utf-8
        public static string input_charset = "utf-8";
        //签名方式
        public static string sign_type = "MD5";
        //签约支付宝账号或卖家收款支付宝帐户[酒店在线支付]
        public static string seller_email = "";
        //储值卡充值收款账户
        public static string payment_seller_email = "";


        /*############################################### 充值异步推送通知 ########################*/
        //充值同步通知
        public static string account_pay_return_url = DBSetting.getAppText("account_pay_return_url");
        //二维码支付同步通知返回
        public static string account_qr_return_url = DBSetting.getAppText("account_qr_return_url");
        /*################################################################################################*/



        /*############################################### 支付与退款同步与异步推送通知 ######################*/
        //同步推送通知
        public static string alipay_pay_return_url = DBSetting.getAppText("alipay_pay_return_url");
        //二维码支付同步通知返回
        public static string alipay_qr_return_url = DBSetting.getAppText("alipay_qr_return_url");
        //支付异步推送通知
        public static string alipay_pay_notify_url = @"http://xxxxxxxxxxx//Web/Alipay_Notify.aspx";
        //退款异步推送通知
        public static string alipay_refund_notify_url = @"http://xxxxxxxxxxx/Web/Refund/Alipay_otify.aspx";
        /*####################################################################################################*/
    }
}