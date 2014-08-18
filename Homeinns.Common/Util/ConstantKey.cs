using System;
using System.Collections.Generic;
using System.Linq;

namespace Homeinns.Common.Util
{
    /// <summary>
    /// 全局配置类
    /// </summary>
    public class ConstantKey
    {

        /// <summary>
        /// JSON头
        /// </summary>
        public static string JSON = @"application/json";

        /// <summary>
        /// XML头
        /// </summary>
        public static string XML = @"application/xml";

        /// <summary>
        /// 时间格式
        /// </summary>
        public static string DateFormatDay = "yyyyMMdd";

        /// <summary>
        /// 时间格式
        /// </summary>
        public static string DateFormatTime = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        ///时间格式
        /// </summary>
        public static string DateFormatSecond = @"yyyyMMddHHmmss";

        /// <summary>
        ///时间格式
        /// </summary>
        public static string DateFormatMillisecond = @"yyyyMMddHHmmssfff";


        /// <summary>
        /// 自动登录Cookie的名字
        /// </summary>
        public static string autoLoginKey = @"AutoDotNetAuth";

        /// <summary>
        /// 台湾联盟Cookie的名字
        /// </summary>
        public static string autoLoginKeyYL = @"AutoDotNetAuthYL";

        /// <summary>
        /// 用户Session
        /// </summary>
        public static string MemberKey = @"Member";
        /// <summary>
        /// 验证码SESSION
        /// </summary>
        public static string VerifyCode = @"VerifyCode";

        /// <summary>
        /// 防重复预定
        /// </summary>
        public static string REPT_ACCESS_TOKEN = "REPT_ACCESS_TOKEN";
        /// <summary>
        /// 防重复点评
        /// </summary>
        public static string REPT_COMT_TOKEN = "REPT_COMT_TOKEN";
        /// <summary>
        /// 防深度重复点评
        /// </summary>
        public static string REPT_DEPT_TOKEN = "REPT_DEPT_TOKEN";
        #region 接口全局配置
        /// <summary>
        /// 主机
        /// </summary>
        public static string HOST_API = @"http://192.168.210.207";
        public static string LISENCE = "800333";
        public static string OPR_NO = "Web";
        public static string TERM_NO = "800820";
        //积分
        public static string TERM_NO_SCORE = "800821";
        public static string SERIAL_NO
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        public static string TERM_SEQ
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            }
        }


        #endregion

        #region 接口
        /// <summary>
        /// 普通酒店预订接口
        /// </summary>
        public static string BookingForWeb = "BookingForWeb/JSON";
        /// <summary>
        /// 带电子券酒店预订接口
        /// </summary>
        public static string BookingWithTicketForWeb = "BookingWithTicketForWeb/JSON";

        /// <summary>
        /// 积分酒店预订接口
        /// </summary>
        public static string BookingWithScoreForWeb = "BookingWithScoreForWeb/JSON";

        /// <summary>
        /// 积分酒店预订接口
        /// </summary>
        public static string BookingWithTicketForSilver = "BookingWithTicketForSilver/JSON";

        /// <summary>
        /// 酒店预定并排房预订接口
        /// </summary>
        public static string BookingWithSelfRoomForWeb = "BookingWithSelfRoomForWeb/JSON";
        /// <summary>
        /// 记录银发卡购买日志
        /// </summary>
        public static string DealSilverCardLogForWeb = "DealSilverCardLogForWeb/JSON";

        /// <summary>
        /// 记录银旅卡OTA激活的日志
        /// </summary>
        public static string DealSilverOTALogForWeb = "DealSilverOTALogForWeb/JSON";

        /// <summary>
        /// 修改密码
        /// </summary>
        public static string ModifyPassword = "/MymyitSrv/RestMymyti.svc/Rest/ModifyPassword/Json";

        /// <summary>
        /// 忘记密码
        /// </summary>
        public static string InitPassword = "/MymyitSrv/RestMymyti.svc/Rest/InitPassword/Json";

        /// <summary>
        /// 积分明细
        /// </summary>
        public static string QueryScoreTrans = @"/MymyitSrv/RestMymyti.svc/Rest/QueryTrans/Json";

        /// <summary>
        /// 设置默认卡
        /// </summary>
        public static string setDefaultCard = @"/MymyitSrv/RestMymyti.svc/Rest/setDefaultCard/Json";

        /// <summary>
        /// 获得单个订单
        /// </summary>
        public static string SingleOrderDetail = @"GetSingleOrderDetail/${SLISENCE}/${SSERIALNO}/${SORDERNO}/${CARDNOS}/JSON";

        /// <summary>
        /// 我的订单
        /// </summary>
        public static string OrderMemberForWeb = @"GetOrderMemberForWeb/JSON";

        /// <summary>
        /// 历史订单信息
        /// </summary>
        public static string OrderHistoryForWeb = @"GetOrderHistoryForWeb/JSON";

        /// <summary>
        /// 酒店订单取消接口（包括普通订单与储值卡，支付宝网银订单
        /// </summary>
        public static string CancelResvForWeb = @"CancelResvForWeb/${SLISENCE}/${SACCOUNTNO}/${SSERIALNO}/${SORDERNO}/${SCARDNOS}/JSON";

        /// <summary>
        /// 电子卷类型
        /// </summary>
        public static string QueryTicketType = @"/WCFServiceNew2/CouponService.svc/Rest/QueryTicketType/Json";

        /// <summary>
        /// 查询电子卷
        /// </summary>
        public static string QueryTicket = @"/WCFServiceNew2/CouponService.svc/Rest/QueryTicket/Json";


        /// <summary>
        /// 我住过的酒店
        /// </summary>
        public static string HisSourceByCtf = @"/WCFServiceNew2/ServiceDemo.svc/Rest/GetHisSourceByCtf/${sLisence}/${ctfTp}/${ctfNo}/Json";

        /// <summary>
        /// 可以点评的酒店
        /// </summary>
        public static string AvailableTrans = @"/GetAvailableTrans/${SLISENCE}/${SSERIALNO}/${CTFNO}/${CTFTP}/JSON";

        /// <summary>
        /// 预先连接酒店的WS
        /// </summary>
        public static string ConnectHotelInAdvanceForWeb = @"ConnectHotelInAdvanceForWeb/${SLISENCE}/${SACCOUNTNO}/${SSERIALNO}/${HOTELCD}/JSON";


        /// <summary>
        /// 我收藏的酒店
        /// </summary>
        public static string FavoriteHotel = @"GetFavoriteHotel/${SLISENCE}/${SSERIALNO}/${SCTFTP}/${SCTFNO}/JSON";

        /// <summary>
        /// 我的联系人
        /// </summary>
        public static string GetLinkMan = @"GetLinkMan/${SLISENCE}/${SSERIALNO}/${SCTFTP}/${SCTFNO}/JSON";

        /// <summary>
        /// 取得酒店通知信息
        /// </summary>
        public static string GetHotelNote = @"GetHotelNoteForWeb/${SLISENCE}/${SSERIALNO}/${HOTELCD}/JSON";

        /// <summary>
        /// 添加联系人
        /// </summary>
        public static string SetLinkMan = @"SetLinkMan/JSON";

        /// <summary>
        /// 酒店详情页点评
        /// </summary>
        public static string GetHotelCommentsTrans = @"GetHotelCommentsTrans/${SLISENCE}/${SSERIALNO}/${HOTELCD}/${BEGINROW}/${ENDROW}/JSON";

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        public static string GetRecordScoreTrans = @"GetRecordScoreTrans/${SLISENCE}/${SSERIALNO}/${SACCOUNT}/JSON";
        /// <summary>
        /// 自助选房排房
        /// </summary>
        public static string DealSelfRoomForHotel = @"DealSelfRoomForHotel/JSON";

        /// <summary>
        /// 差评点评详情
        /// </summary>
        public static string DealHotelCommentDetailForWeb = @"DealHotelCommentDetailForWeb/JSON";


        /// <summary>
        /// 记录积分兑换酒店后的信息
        /// </summary>
        public static string DealScoreResvForWeb = @"DealScoreResvForWeb/JSON";
        /// <summary>
        /// 更新自助选房的状态
        /// </summary>
        public static string DealSelfRmStatusForWeb = @"DealSelfRmStatusForWeb/JSON";
        //获得积分预定的门市价(换算积分数)
        public static string GetResvRoomRateForWeb = @"GetResvRoomRateForWeb/${SLISENCE}/${SSERIALNO}/${HOTELCD}/${RMTP}/${STDATE}/${ENDDATE}/JSON";
        //解锁帐户电子券
        public static string UnLockTicketForWeb = @"UnLockTicketForWeb/${SLISENCE}/${SACCOUNTNO}/${SSERIALNO}/JSON";

        //绑定如家银旅激活码
        public static string ActiveSilverTicketForWeb = @"ActiveSilverTicketForWeb/${SLISENCE}/${SACCOUNTNO}/${SSERIALNO}/${VERIFYCODE}/JSON";
        //验证如家银旅激活码
        public static string VerifySilverTicketForWeb = @"VerifySilverTicketForWeb/${SLISENCE}/${SACCOUNTNO}/${SSERIALNO}/${VERIFYCODE}/JSON";
        //记录二维码开门问卷调查的日志
        public static string DealAskQAForOpenDoor = @"DealAskQAForOpenDoor/JSON";

    

        /// <summary>
        /// 如家快讯
        /// </summary>
        public static string GetNewsForWeb = @"GetNewsForWeb/JSON";
        /// <summary>
        /// 获得促销活动信息
        /// </summary>
        public static string GetPromotionsForWeb = @"GetPromotionsForWeb/JSON";
        /// <summary>
        /// 获得单个促销活动信息
        /// </summary>
        public static string GetSinglePromotionForWeb = @"GetSinglePromotionForWeb/${ID}/JSON";

        /// <summary>
        /// 获得焦点图信息
        /// </summary>
        public static string GetScrollNewsForWeb = @"GetScrollNewsForWeb/JSON";

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        public static string UpdateNoteForWeb = @"UpdateNoteForWeb/${SLISENCE}/${ORDERNO}/${NOTE}/${HOTELCD}/JSON";


        #region 储值卡相关接口
        /// <summary>
        /// MMT支付成功后更新订单信息
        /// </summary>
        public static string DealResvForMmtPay = @"DealResvForMmtPay/JSON";
        #endregion

        #endregion

        #region  WINDOWS缓存相关配置

        /// <summary>
        /// 缓存服务器
        /// </summary>
        //public static string Remote_CacheServer = @"\\172.23.100.95\Cache";
        /// <summary>
        ///城市缓存
        /// </summary>
        public static string CityCache = @"Homeinns_CityCache.dat";
        /// <summary>
        ///
        /// </summary>
        public static string RegionCache = @"Homeinns_RegionCache.dat";
        /// <summary>
        /// 酒店缓存
        /// </summary>
        public static string HotelCache = @"Homeinns_HotelCache.dat";

        /// <summary>
        /// 台湾酒店缓存
        /// </summary>
        public static string TaiWanHotelCache = @"Homeinns_TaiWanHotelCache.dat";

        /// <summary>
        /// 商圈信息
        /// </summary>
        public static string TradAreaCache = @"Homeinns_TradAreaCache.dat";

        /// <summary>
        /// 行政区缓存
        /// </summary>
        public static string DistrictCache = @"Homeinns_DistrictCache.dat";

        /// <summary>
        ///房型缓存
        /// </summary>
        public static string RmTypeCache = @"Homeinns_RmTypeCache.dat";

        /// <summary>
        /// 地区信息
        /// </summary>
        public static string ZoneForArea = @"Homeinns_ZoneForAreaCache.dat";

        /// <summary>
        /// 交通设施
        /// </summary>
        public static string TransportCache = @"Homeinns_TransportCache.dat";

        /// <summary>
        /// CrsHelp配置
        /// </summary>
        public static string CrsHelpCache = @"Homeinns_CrsHelpCache.dat";

        /// <summary>
        /// 电子卷信息
        /// </summary>
        public static string TicketCacheJob = @"Homeinns_TicketCache.dat";

        /// <summary>
        /// 商圈
        /// </summary>
        public static string LandMarkCache = @"Homeinns_LandMarkCache.dat";
        /// <summary>
        /// 促销活动
        /// </summary>
        public static string ActivityCache = @"Homeinns_ActivityCache.dat";
        /// <summary>
        /// 促销活动价格
        /// </summary>
        public static string ActivityPriceCache = @"Homeinns_ActivityPriceCache.dat";
        /// <summary>
        /// 自助选房
        /// </summary>
        public static string SelfRoomCache = @"Homeinns_SelfRoomCache.dat";
        /// <summary>
        /// 酒店远程连接信息
        /// </summary>
        public static string RemoteHotelCache = @"Homeinns_RemoteHotelCache.dat";
        /// <summary>
        /// 短消息任务
        /// </summary>
        public static string LetterMsgCache = @"Homeinns_LetterMsgCache.dat";
        /// <summary>
        /// 酒店房间属性信息
        /// </summary>
        public static string RmSelfTpCache = @"Homeinns_RmSelfTpCache.dat";
        /// <summary>
        /// 酒店提醒数据
        /// </summary>
        public static string HotelNoteCache = @"Homeinns_HotelNoteCache.dat";

        /// <summary>
        /// 积分商城商品信息
        /// </summary>
        public static string ProductCache = @"Homeinns_ProductCache.dat";

        /// <summary>
        /// 酒店周边
        /// </summary>
        public static string HotelAroundCache = @"Homeinns_HotelAroundCache.dat";


        #endregion
    }
}
