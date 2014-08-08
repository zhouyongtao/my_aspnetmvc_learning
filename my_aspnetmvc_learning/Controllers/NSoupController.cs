using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Z;
using NLog;
using HanTing;
using System.Threading.Tasks;
using NSoup;
using NSoup.Nodes;
using System.Net.Http;

namespace my_aspnetmvc_learning.Controllers
{
    public class NSoupController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region 设置默认Http参数
        /// <summary>
        /// 设置默认UserAgent
        /// </summary>
        private static readonly string defaultUserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0";
        /// <summary>
        /// 设置会话Cookie
        /// </summary>
        private readonly static CookieContainer cookieContainer = new CookieContainer();
        #endregion

        #region 定义请求的委托签名
        private static Func<string, string, string, string, string, string, int, string> request = (url, method, data, refer, contentType, soapAction, timeout) =>
        {
            try
            {
                if (url.IsEmpty())
                    throw new ArgumentNullException("url");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = method.ToUpper();
                req.AllowAutoRedirect = true;
                req.CookieContainer = cookieContainer;
                req.ContentType = contentType;
                req.UserAgent = defaultUserAgent;
                req.Timeout = timeout;
                //调用WebService
                if (!soapAction.IsEmpty())
                {
                    req.Headers.Add("SOAPAction", soapAction);
                }
                if (refer.IsNotEmpty())
                {
                    req.Referer = refer;
                }
                if (method.ToUpper() == "POST" && !data.IsEmpty())
                {
                    byte[] postBytes = UTF8Encoding.UTF8.GetBytes(data);
                    req.ContentLength = postBytes.Length;
                    using (Stream stream = req.GetRequestStream())
                    {
                        stream.Write(postBytes, 0, postBytes.Length);
                    }
                }
                //忽略HTTPS证书
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslError) =>
                {
                    return true;
                };
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                using (StreamReader read = new StreamReader(rep.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    return read.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("HanTing Request Exception url: {0} \r\n data: {1}", url, data), ex);
                return string.Empty;
            }
        };

        private static Func<string, string, string, int, int, HanTingData> remoteData = (cityID, checkInDate, checkOutDate, pageIndex, pageSize) =>
        {
            var param = new SortedDictionary<string, string>();
            param.Add("CityID", cityID);
            param.Add("CheckInDate", checkInDate);
            param.Add("CheckOutDate", checkOutDate);
            param.Add("Distence", "20000");
            param.Add("IsGetVirtualRoom", "true");
            param.Add("IsPointExchangRoom", "0");
            param.Add("KeyWord", "");
            param.Add("Latlng", "");
            param.Add("ActivityID", "");
            param.Add("PageIndex", pageIndex.ToString());
            param.Add("PageSize", pageSize.ToString());
            param.Add("QueryRoomType", "PNA");
            param.Add("SortBy", "0");
            string postData = BuildRequestParam(param, System.Text.Encoding.UTF8);
            string refer = String.Format(@"http://i.huazhu.com/hotel/list?checkindate={0}&checkoutdate={1}&cityID={2}&keyword=&Latlng", checkInDate, checkOutDate, cityID);
            string hotelData = request(@"http://i.huazhu.com/api/hotel/list", "POST", postData, refer, "application/x-www-form-urlencoded; charset=UTF-8", null, 20000);
            try
            {
                var hanTing = JsonConvert.DeserializeObject<HanTingData>(hotelData);
                if (hanTing == null || hanTing.Data == null || hanTing.Data.HotelList == null || !hanTing.Data.HotelList.Any())
                {
                    logger.Error(String.Format(@"获取数据异常 ： api: http://i.huazhu.com/api/hotel/list postData: {0}", postData));
                }
                return hanTing;
            }
            catch (Exception ex)
            {
                logger.Error("HanTingServiceJob   任务   GetRemoteHotelData  :" + ex.Message, ex);
                return null;
            }
        };
        #endregion

        // GET: /NSoup/
        public ActionResult Index()
        {
            Redslide.HttpLib.Request.Get("http://hotels.ctrip.com/hotel/shanghai2",
            result =>
            {
                Document doc = NSoupClient.Parse(result, "gb2312");
                string hotelData = doc.GetElementById("hotel_list").Html();
            });
            return Content("");
        }

        public async Task<string> Get()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("http://www.baidu.com");
        }


        /// <summary>
        /// 构造参数
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string BuildRequestParam(SortedDictionary<string, string> param, Encoding encoding)
        {
            var prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in param)
            {
                prestr.Append(String.Format("{0}={1}&", temp.Key, HttpUtility.UrlEncode(temp.Value, encoding)));
            }
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }
    }
}