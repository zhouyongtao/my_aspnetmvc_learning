using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.ServiceModel.Channels;
using Homeinns.Common.Net;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;
using System.Configuration;

namespace Homeinns.Common.Util
{
    /// <summary>
    /// 数据库辅助函数
    /// </summary>
    public class DBFunc
    {

        /// <summary>
        /// 在一个String值两端加上单引号，主要用于构造SQL命令
        /// </summary>
        /// <param name="str">要被加单引号的参数值</param>
        /// <returns>加过单引号的String</returns>
        public static string addSingleQuotes(string str)
        {
            return String.Format("'{0}'", str);
        }

        /// <summary>
        /// 检查要插入sql查询语句的UI输入值串，是否存在注入式的可能。
        /// </summary>
        /// <param name="str">被检查的串</param>
        /// <returns>检查结果，true表示str为合法的输入，false表示str为非法的输入</returns>
        public static bool checkSqlInputValue(string str)
        {
            const string unsafeMask = @"^\?(.*)(select |delete |count\(|drop table|updata |truncate |asc\(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|:|'|net user| or )(.*)$";
            if (System.Text.RegularExpressions.Regex.Match(str.ToLower(), unsafeMask).Success)
                return false;
            else return true;
        }
        /// <summary>
        /// 计算字符串的字节长度
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>字符串字节长度</returns>
        public static int getByteLen(string str)
        {
            int numLen = 0;
            for (int i = 0; i < str.Length; i++)
            {
                numLen += System.Text.Encoding.Default.GetByteCount(str.Substring(i, 1));
            }
            return numLen;
        }
    }

    /// 检查数据是否合法
    /// </summary>
    public class Chk
    {
        private Chk()
        {
            //
            // 这个类提供静态功能函数，禁止构造实例
            //
        }

        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool IsDbNull(object objValue)
        {
            try
            {
                return Convert.IsDBNull(objValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DBNull2Str(object objValue)
        {
            if (IsDbNull(objValue))
            {
                return "";
            }
            else
            {
                return objValue.ToString();
            }
        }

        public static bool DBNull2Bit(object objValue)
        {
            bool Default = false;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (bool)objValue;
            }
        }

        public static bool DBNull2Bit(object objValue, bool fDefault)
        {
            bool Default = fDefault;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (bool)objValue;
            }
        }

        public static int DBNull2Int(object objValue)
        {
            int Default = 0;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (int)objValue;
            }
        }

        public static int DBNull2TinyInt(object objValue)
        {
            int Default = 0;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToInt32(objValue);
            }
        }


        public static int DBNull2Int(object objValue, int iDefault)
        {
            int Default = iDefault;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (int)objValue;
            }
        }

        public static Decimal DBNull2Decimal(object objValue)
        {
            Decimal Default = 0;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (Decimal)objValue;
            }
        }

        public static Decimal DBNull2Decimal(object objValue, Decimal iDefault)
        {
            Decimal Default = iDefault;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (Decimal)objValue;
            }
        }

        public static Double DBNull2Double(object objValue)
        {
            Double Default = 0;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToDouble(objValue);
            }
        }

        public static Double DBNull2Double(object objValue, Double iDefault)
        {
            Double Default = iDefault;

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToDouble(objValue);
            }
        }


        public static DateTime DBNull2Date(object objValue)
        {
            DateTime Default = Convert.ToDateTime(Const.DefaultNullDate);

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (DateTime)objValue;
            }
        }

        public static DateTime DBNull2Date(object objValue, string dDefault)
        {
            DateTime Default = Convert.ToDateTime(dDefault);

            if (IsDbNull(objValue))
            {
                return Default;
            }
            else
            {
                return (DateTime)objValue;
            }
        }



        /// <summary>
        /// 检查是否是有效的日期
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool IsDate(object objValue)
        {
            try
            {
                Convert.ToDateTime(objValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否是数字
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool IsNumeric(object objValue)
        {
            try
            {
                Convert.ToDecimal(objValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int Null2Int(object objValue)
        {
            return Null2Int(objValue, 0);
        }

        public static int Null2Int(object objValue, int iDefault)
        {
            int Default = iDefault;

            if (objValue == null || objValue.ToString() == string.Empty)
            {
                return Default;
            }
            else
            {
                return Convert.ToInt32(objValue);
            }
        }

        public static Decimal Null2Decimal(object objValue)
        {
            return Null2Decimal(objValue, 0);
        }

        public static Decimal Null2Decimal(object objValue, Decimal iDefault)
        {
            Decimal Default = iDefault;

            if (objValue == null || objValue.ToString() == string.Empty)
            {
                return Default;
            }
            else
            {
                return Convert.ToDecimal(objValue);
            }
        }


        public static Double Null2Double(object objValue)
        {
            return Null2Double(objValue, 0);
        }

        public static Double Null2Double(object objValue, Double iDefault)
        {
            Double Default = iDefault;

            if (objValue == null || objValue.ToString() == string.Empty)
            {
                return Default;
            }
            else
            {
                return Convert.ToDouble(objValue);
            }
        }

        public static string IsNull(Object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            return ob.ToString();
        }

        public static int IsNullInt(Object ob)
        {
            if (ob == null || ob.ToString() == string.Empty)
            {
                return 0;
            }
            return int.Parse(ob.ToString());
        }

        public static byte IsNullByte(Object ob)
        {
            if (ob == null || ob.ToString() == string.Empty)
            {
                return 0;
            }
            return Convert.ToByte(ob.ToString());
        }

        public static DateTime IsNullDate(Object ob)
        {
            DateTime Default = Convert.ToDateTime(Const.DefaultNullDate);
            try
            {
                if (ob == null || ob.ToString() == string.Empty)
                    return Default;

                return Convert.ToDateTime(ob);
            }
            catch
            {
                return Default;
            }
        }

        /// <summary>
        /// 检查远程主机是否能Ping通
        /// </summary>
        /// <param name="sHost"></param>
        /// <returns></returns>
        public static bool CmdPing(string sHost)
        {
            try
            {
                bool IsLink;
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                string sPingrst = string.Empty;
                process.StartInfo.Arguments = "ping " + sHost + " -n 1";
                process.Start();
                process.StandardInput.AutoFlush = true;
                string temp = "ping " + sHost + " -n 1";
                process.StandardInput.WriteLine(temp);
                process.StandardInput.WriteLine("exit");
                string strRst = process.StandardOutput.ReadToEnd();
                if (strRst.IndexOf("(0% loss)") != -1)
                {
                    sPingrst = "连接";
                    IsLink = true;
                }
                else if (strRst.IndexOf("Destination host unreachable") != -1)
                {
                    sPingrst = "无法到达目的主机";
                    IsLink = false;
                }
                else if (strRst.IndexOf("Request timed out") != -1)
                {
                    sPingrst = "超时";
                    IsLink = false;
                }
                else if (strRst.IndexOf("Unknown host") != -1)
                {
                    sPingrst = "无法解析主机";
                    IsLink = false;
                }
                else
                {
                    sPingrst = strRst;
                    IsLink = false;
                }
                process.Close();
                //return sPingrst;
                return IsLink;
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResvType
    {
        public const string CallCenter = "ctel";
        public const string WebOrder = "cnet";
        public const string Guarantee = "cngt";
        public const string WebSrv = "csvr";
        public const string QuickCash = "cbil";
        public const string OwnerCompany = "ccrv";
        public const string WapOrder = "cwap";

    }

    /// <summary>
    /// 常量
    /// </summary>
    public class Const
    {
        public const string HomeinnsTitle = "如家快捷酒店-";
        public const string ChinaPayCode = "01";
        public const string billCode = "02";

        public const string CashCode = "00";
        /// <summary>
        /// 支付宝
        /// </summary>
        public static string AlipayCode = "03";
        /// <summary>
        /// 工商银行网上支付
        /// </summary>
        public const string ICBCCode = "07";
        /// <summary>
        /// 建设银行网上支付
        /// </summary>
        public const string CCBCode = "08";
        /// <summary>
        /// 招商银行网上支付
        /// </summary>
        public const string CMBCode = "09";

        public static string TransCd_ChinaPay = "002008";
        public static string TransCd_bill = "002010";
        public static string TransCd_Alipay = "002009";


        public const string PayRangeFist = "1";
        public const string PayRangeFull = "2";

        public const string ActReturnCode = "RETN";

        public const string DateFormatFullString = @"yyyyMMddHHmmss";

        public const string TM = @"HHmmss";
        /// <summary>
        /// yyyy-M-d
        /// </summary>
        public const string DateFormatString = @"yyyy-M-d";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string DateFormatString2 = @"yyyy-MM-dd";

        /// <summary>
        /// MM月dd日
        /// </summary>
        public const string DateFormatString3 = @"MM月dd日";

        /// <summary>
        /// YYYY年MM月dd日
        /// </summary>
        public const string DateFormatString4 = @"yyyy年MM月dd日";

        /// <summary>
        /// YYYYMMDD
        /// </summary>
        public const string DateFormatString5 = @"yyyyMMdd";

        public const string DefaultNullDate = @"1900-1-1";
        public const string TimeFormatString = @"T,DateTimeFormatInfo.InvariantInfo";

        /// <summary>
        /// @"#,###,###,##0"
        /// </summary>
        public const string IntFormatString = @"#,###,###,##0";
        /// <summary>
        /// @"#,###,###,##0.00"
        /// </summary>
        public const string DecimalFormatString = @"#,###,###,##0.00";
        /// <summary>
        /// @"#########.00"
        /// </summary>
        public const string DecimalString = @"#########.00";

        /// <summary>
        /// Format(F0)
        /// </summary>
        public const string IntFormatStringNoDot = "F0";
        /// <summary>
        /// @"#########0.00"
        /// </summary>
        public const string DecimalFormatStringNoDot = @"#########0.00";

        /// <summary>
        /// @"###,###,##0.0"
        /// </summary>
        public const string PercentFormatString = @"###,###,##0.0";

        public const string CashTp = "&yen; ";

        public const string RoomNull = "满房";

        public const string RoomFull = "良好";

        public const string WebResvDesp = "cnet";

        public const string WebOpr = "Web";

        public const string AgentCd = "800333";

        /// <summary>
        ///CRS几点后放开其它渠道保留房
        /// </summary>
        public const string Hour = "18";

        /// <summary>
        /// Eq酒店设施
        /// </summary>
        public const string DespEq = "Eq";

        /// <summary>
        /// Tr交通
        /// </summary>
        public const string DespTr = "Tr";

        /// <summary>
        /// Rm客房服务
        /// </summary>
        public const string DespRm = "Rm";

        /// <summary>
        /// Dr行车
        /// </summary>
        public const string DespDr = "Dr";

        /// <summary>
        /// Cr信用卡
        /// </summary>
        public const string DespCr = "Cr";


        /// <summary>
        /// 公司卡折扣
        /// </summary>
        public const string HelpcyType = "cyType";
        /// <summary>
        /// 优惠类型-客源分
        /// </summary>
        public const string HelpGustTp = "ActGustTp";
        /// <summary>
        /// 担保/支付
        /// </summary>
        public const string HelpPayTp = "Activity";
        /// <summary>
        /// 银行列表
        /// </summary>
        public const string HelpBankTp = "BANK";
        /// <summary>
        /// 会员卡类
        /// </summary>
        public const string HelpCardTp = "CardTp_H";
        /// <summary>
        /// 证件
        /// </summary>
        public const string HelpPassTp = "CtfTp";
        /// <summary>
        /// 客源
        /// </summary>
        public const string HelpGdTp = "GustKind";
        /// <summary>
        /// 酒店星级
        /// </summary>
        public const string HelpHotelTp = "hotelclass";
        /// <summary>
        /// 特许/直营
        /// </summary>
        public const string HelpMagTp = "hoteltype";
        /// <summary>
        /// Memo选择类别
        /// </summary>
        public const string HelpMemoTp = "note";
        /// <summary>
        /// 预订来源
        /// </summary>
        public const string HelpResvTp = "resvtype";
        /// <summary>
        /// 确认列表
        /// </summary>
        public const string HelpReturnTp = "returntp";
        /// <summary>
        /// 房态控制类别
        /// </summary>
        public const string HelpLockTp = "SF";
        /// <summary>
        /// 酒店推荐
        /// </summary>
        public const string HelpSaleTp = "RL";

        /// <summary>
        /// Descript字段名
        /// </summary>
        public const string HelpText = "Descript";

        /// <summary>
        /// Code字段名
        /// </summary>
        public const string HelpValue = "CD";

        //--------------电信短信接口配置 START-----------
        //配置文件常量
        public const int MessageLen = 70;
        public const string ServerIP = "ServerIP";
        public const string ServerPort = "ServerPort";
        public const string IcpId = "IcpId";
        public const string IcpShareKey = "IcpShareKey";

        public const string nMsgType = "nMsgType";
        public const string sFeeType = "sFeeType";
        public const string sFeeCode = "sFeeCode";
        public const string sFixedFee = "sFixedFee";
        public const string sChargeTermID = "sChargeTermID";
        public const string sSrcTermID = "sSrcTermID";
        public const string sDestTermID = "sDestTermID";
        public const string nNeedReply = "nNeedReply";
        public const string nMsgLevel = "nMsgLevel";
        public const string sServiceID = "sServiceID";
        public const string nMsgFormat = "nMsgFormat";
        public const string sValidTime = "sValidTime";
        public const string sAtTime = "sAtTime";

        //--------------电信短信接口配置 END-----------



        public static string FormatFullDate(string sYear)
        {
            string sReturn;
            sReturn = sYear + "-1-1";

            return sReturn;
        }

        public static string FormatFullDate(string sYear, string sMonth)
        {
            string sReturn;
            sReturn = sYear + "-" + sMonth + "-1";

            return sReturn;
        }

        public const string PayTp = "0";
        public const string GustTp = "Comp";
        public const string GustTp2 = "Member";
        public const string GustTp3 = "Guest";
        //public const string GustTpWap = "Cwap"; 

        public const string GustCd = "";

    }


    //public class Cookie
    //{
    //    /// <summary> 
    //    /// 设置Cookies值
    //    /// </summary> 
    //    /// <param name="DomainName">域名</param> 
    //    /// <param name="CookieName">Cookie键名称</param>
    //    /// <param name="CookieValue">Cookie键值</param>
    //    /// <param name="ExpiresDayValue">过期时间,单位,天</param>
    //    public static void SetCookies(string DomainName, string CookieName, string CookieValue, int ExpiresDayValue)
    //    {
    //        CookieName = HttpContext.Current.Server.UrlEncode(CookieName);
    //        CookieValue = HttpContext.Current.Server.UrlEncode(CookieValue);

    //        DomainName = HttpContext.Current.Server.UrlEncode(DomainName);

    //        HttpCookie MyCookie = new HttpCookie(CookieName);

    //        MyCookie.Value = CookieValue;
    //        if (ExpiresDayValue != 0)
    //        {
    //            MyCookie.Expires = DateTime.Now.AddDays(ExpiresDayValue);
    //        }
    //        HttpContext.Current.Response.AppendCookie(MyCookie);

    //    }


    //    /// <summary> 
    //    /// 取Cookies值
    //    /// 返回取到的内容
    //    /// </summary> 
    //    /// <param name="DomainName">域名</param> 
    //    /// <param name="CookieName">Cookie键名称</param>
    //    public static string GetCookies(string DomainName, string CookieName)
    //    {
    //        CookieName = System.Web.HttpContext.Current.Server.UrlEncode(CookieName);
    //        //CookieName=Security.Encrypt(CookieName,Security.myKey);//加密键名

    //        try
    //        {
    //            if (System.Web.HttpContext.Current.Request.Cookies[CookieName].Value != null)
    //            {
    //                return System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[CookieName].Value);
    //            }
    //            else
    //            {
    //                return "";
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            SetCookies(DomainName, CookieName, "", 7);
    //            return "";
    //        }
    //    }

    //}

    /// <summary>
    /// 安全
    /// </summary>
    public class Safe
    {
        public static string SafeSqlLikeClauseLiteral(string inputSQL)
        {
            // 进行以下替换：
            // '  变成  ''
            // [  变成  [[]
            // %  变成  [%]
            // _  变成  [_]

            string s = inputSQL;
            s = inputSQL.Replace("'", "''");
            s = s.Replace("[", "[[]");
            s = s.Replace("%", "[%]");
            s = s.Replace("_", "[_]");
            s = s.Replace("-", "[-]");
            s = s.Replace("<", "[<]");
            s = s.Replace(">", "[>]");
            s = s.Replace("=", "[=]");
            s = s.Replace(";", "[;]");
            return s;
        }

        #region Sql防注入
        public static bool ChkInject(string sParam)
        {
            try
            {
                string[] anySqlStr = ConfigurationManager.AppSettings["SqlInject"].Split('|');
                foreach (string jk in anySqlStr)
                {
                    if (sParam.Contains(jk))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }


    /// <summary>
    ///Utility 的摘要说明
    /// </summary>
    public class Utility
    {
        public Utility()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        private static Hashtable _Hashtable = new Hashtable();
        public static Hashtable HashFlags
        {
            get { return _Hashtable; }
        }

        public static string getUnionNo(string sTerminalNo, string sInterfaceNo, string sServerNo, string sOccurTime, string sSequence)
        {
            return sTerminalNo + sInterfaceNo + sServerNo + sOccurTime + sSequence;
        }

        public static RemoteEndpointMessageProperty getIpAndPort()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties propertites = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endPoint = propertites[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;


            HttpRequestMessageProperty a = propertites["httpRequest"] as HttpRequestMessageProperty;
            string b = a.Headers["Accept"];

            return endPoint;
        }


        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double getRtByRoundOff(double d)
        {
            try
            {
                double dRet = 0;
                dRet = Math.Round(d, MidpointRounding.AwayFromZero);
                return dRet;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象系列化为二进制字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeObject<T>(T obj)
        {
            byte[] buffer = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    serializer.Serialize(ms, obj);
                    //XmlSerializer xml = new XmlSerializer(typeof(T)); 
                    //xml.Deserialize(ms);
                    buffer = ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
            return buffer;
        }
        public static byte[] GetByteFromStream(Stream stream)
        {
            byte[] buffer = null;
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                string stream_value = reader.ReadToEnd();
                buffer = System.Text.Encoding.UTF8.GetBytes(stream_value);
            }
            return buffer;
        }

        public static string GetStringFromStream(Stream stream)
        {
            string value = string.Empty;
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }

        public static byte[] GetByteFromStreamFor64bit(Stream stream)
        {
            byte[] buffer = null;
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                string stream_value = reader.ReadToEnd();
                buffer = Convert.FromBase64String(stream_value);
            }
            return buffer;
        }
        /// <summary>
        /// 将二进制xml流系列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(Stream stream)
        {
            T obj;
            string str_reader = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    str_reader = sr.ReadToEnd();
                }
                //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer =
                //                                                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                XmlSerializer xml_serializer = new XmlSerializer(typeof(T));
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str_reader);//Convert.FromBase64String(str_reader);
                using (Stream ms = new MemoryStream(buffer))
                {
                    //t = (T)deserializer.Deserialize(ms);
                    StreamReader s_reader = new StreamReader(ms, System.Text.Encoding.UTF8);
                    obj = (T)xml_serializer.Deserialize(s_reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        public static string SerializeObject2NonFormatedXmlString<T>(T t)
        {
            string xml = "";
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    System.Xml.Serialization.XmlSerializer xmlSerizlizer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    xmlSerizlizer.Serialize(ms, t);
                    xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
            return xml;
        }
        public static T getObject<T>(Stream stream)
        {
            T obj;
            try
            {
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                XmlSerializer _xs = new XmlSerializer(typeof(T));
                string value = sr.ReadToEnd();
                Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
                Stream sm = new MemoryStream(bytes);
                StreamReader reader = new StreamReader(sm, System.Text.Encoding.UTF8);
                obj = (T)_xs.Deserialize(reader);
            }
            catch (Exception er)
            {
                throw er;
            }
            return obj;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeSerializeByBytes<T>(byte[] bytes)
        {
            T obj;
            try
            {
                //BinaryFormatter Formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(bytes);
                obj = getObject<T>(ms);
                //Formatter.Binder = new UBinder();
                //ms.Position = 0;
                //object Obj = Formatter.Deserialize(ms); 
                //ms.Close();
                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 将Xml格式的字符串系列化为object类型的对象
        /// </summary>
        /// <typeparam name="T">类型占位符</typeparam>
        /// <param name="xml">参数:Xml格式字符串</param>
        /// <returns>返回一个T类型的对象</returns>
        public static T DeserializeXml2Object<T>(string xml)
        {

            T obj = default(T);
            if (xml == "" || xml == null)
                return obj;
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xml);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(ms);
                    XmlSerializer xml_serializer = new XmlSerializer(typeof(T));
                    obj = (T)xml_serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        /// <summary>
        /// 将指定的对象系列化为Xml格式的字符串
        /// </summary>
        /// <typeparam name="T">类型展位符</typeparam>
        /// <param name="obj">参数:要进行系列化的对象</param>
        /// <returns>返回Xml格式的字符串</returns>
        public static string SerializeObject2XmlString<T>(T obj)
        {
            string xmlString = "";
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(ms, System.Text.Encoding.UTF8);
                    writer.Indentation = 4;
                    writer.Formatting = System.Xml.Formatting.Indented;
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(writer, obj);
                    writer.Close();
                    xmlString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xmlString;
        }
        /// <summary>
        /// 将对象集合类型转换为字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static byte[] GetByteFromList<T>(List<T> list)
        {
            XmlSerializer xmlLizer = new XmlSerializer(typeof(List<T>));
            byte[] buffer = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
                    xmlLizer.Serialize(writer, list);
                    int count = (int)ms.Length;
                    buffer = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(buffer, 0, count);
                    writer.Close();
                }
            }
            catch
            {
                return null;
            }
            return buffer;
            //如果转xml的话将下面的注释去掉
            //Encoding utf = Encoding.UTF8 ;
            //string xml = utf.GetString(buffer).Trim();
        }
        /// <summary>
        /// 对象实例转成xml
        /// </summary>
        /// <param name="item">对象实例</param>
        /// <returns></returns>
        public static string EntityToXml<T>(T item)
        {
            IList<T> items = new List<T>();
            items.Add(item);
            return EntityToXml(items);
        }
        /// <summary>
        /// 对象实例集转成xml
        /// </summary>
        /// <param name="items">对象实例集</param>
        /// <returns></returns>
        public static string EntityToXml<T>(IList<T> items)
        {
            //创建XmlDocument文档
            XmlDocument doc = new XmlDocument();
            //创建根元素
            XmlElement root = doc.CreateElement(typeof(T).Name + "s");
            //添加根元素的子元素集
            foreach (T item in items)
            {
                EntityToXml(doc, root, item);
            }
            //向XmlDocument文档添加根元素
            doc.AppendChild(root);

            return doc.InnerXml;
        }
        /// <summary>
        /// 将对象转换为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        /// <param name="item"></param>
        private static void EntityToXml<T>(XmlDocument doc, XmlElement root, T item)
        {
            //创建元素
            XmlElement xmlItem = doc.CreateElement(typeof(T).Name);
            //对象的属性集
            System.Reflection.PropertyInfo[] propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo pinfo in propertyInfo)
            {
                if (pinfo != null)
                {
                    //对象属性名称
                    string name = pinfo.Name;
                    //对象属性值
                    string value = String.Empty;

                    if (pinfo.GetValue(item, null) != null)
                        value = pinfo.GetValue(item, null).ToString();//获取对象属性值
                    //设置元素的属性值
                    xmlItem.SetAttribute(name, value);
                }
            }
            //向根添加子元素
            root.AppendChild(xmlItem);
        }
        /// <summary>
        /// Xml转成对象实例集
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static IList<T> XmlToEntityList<T>(string xml)
        {
            XmlDocument doc = new XmlDocument();
            if (xml == string.Empty)
                return null;
            try
            {
                doc.LoadXml(xml);
            }
            catch
            {
                return null;
            }
            //if (doc.ChildNodes.Count != 1)
            //    return null;
            //if (doc.ChildNodes[0].Name.ToLower() != typeof(T).Name.ToLower() + "s")
            //    return null;

            XmlNode node = doc.ChildNodes[0];

            IList<T> items = new List<T>();

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name.ToLower() == typeof(T).Name.ToLower())
                    items.Add(XmlNodeToEntity<T>(child));
                ;
            }

            return items;
        }
        /// <summary>
        /// Xml转成对象实例
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static T XmlToEntity<T>(string xml)
        {
            IList<T> items = XmlToEntityList<T>(xml);
            if (items != null && items.Count > 0)
                return items[0];
            else return default(T);
        }
        private static T XmlNodeToEntity<T>(XmlNode node) //where T : new()
        {
            T item = default(T);

            if (node.NodeType == XmlNodeType.Element)
            {
                XmlElement element = (XmlElement)node;

                System.Reflection.PropertyInfo[] propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                foreach (XmlAttribute attr in element.Attributes)
                {
                    string attrName = attr.Name.ToLower();
                    string attrValue = attr.Value.ToString();
                    foreach (System.Reflection.PropertyInfo pinfo in propertyInfo)
                    {
                        if (pinfo != null)
                        {
                            string name = pinfo.Name.ToLower();
                            Type dbType = pinfo.PropertyType;
                            if (name == attrName)
                            {
                                if (String.IsNullOrEmpty(attrValue))
                                    continue;
                                switch (dbType.ToString())
                                {
                                    case "System.Int32":
                                        pinfo.SetValue(item, Convert.ToInt32(attrValue), null);
                                        break;
                                    case "System.Boolean":
                                        pinfo.SetValue(item, Convert.ToBoolean(attrValue), null);
                                        break;
                                    case "System.DateTime":
                                        pinfo.SetValue(item, Convert.ToDateTime(attrValue), null);
                                        break;
                                    case "System.Decimal":
                                        pinfo.SetValue(item, Convert.ToDecimal(attrValue), null);
                                        break;
                                    case "System.Double":
                                        pinfo.SetValue(item, Convert.ToDouble(attrValue), null);
                                        break;
                                    default:
                                        pinfo.SetValue(item, attrValue, null);
                                        break;
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            return item;
        }
        /// <summary>
        /// 将对象集合类型转换为xml格式字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetXmlFromList<T>(List<T> list)
        {
            string xml = "";
            byte[] buffer = GetByteFromList<T>(list);
            if (buffer != null)
            {
                xml = Encoding.UTF8.GetString(buffer);
            }
            return xml;
        }
        /// <summary>
        /// 使用System.Runtime.Serialization.DataContractSerializer类将List&lt;T&gt;集合类型系列化为xml格式字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SerializeList<T>(List<T> list)
        {
            string xml = string.Empty;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<T>));
                    XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
                    serializer.WriteObject(ms, list);
                    xml = Encoding.UTF8.GetString(ms.ToArray());
                    writer.Close();
                }
            }
            catch
            {
                return null;
            }
            return xml;
        }
        /// <summary>
        /// 使用System.Runtime.Serialization.DataContractSerializer类将对象object类型序列化为xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeByDataContract<T>(T obj)
        {
            string xml = string.Empty;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    XmlTextWriter xmlWriter = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
                    serializer.WriteObject(ms, obj);//serializer.WriteObject(xmlWriter, obj);
                    xml = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                    xmlWriter.Close();
                }
            }
            catch
            {
                return null;
            }
            return xml;
        }
        /// <summary>
        /// 使用System.Runtime.Serialization.DataContractSerializer类将xml格式字符串转换为List&lt;T&gt;集合类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static List<T> DeserializeListFromXml<T>(string xml)
        {
            List<T> objList = default(List<T>);
            try
            {
                TextReader txtReader = new StringReader(xml.Trim());
                XmlTextReader xmlReader = new XmlTextReader(txtReader);
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<T>));
                objList = serializer.ReadObject(xmlReader) as List<T>;
                txtReader.Close();
                xmlReader.Close();
            }
            catch
            {
                return objList;
            }
            return objList;
        }
        /// <summary>
        /// 使用System.Runtime.Serialization.DataContractSerializer类将xml格式字符串转换为对象object类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeByDataContract<T>(string xml)
        {
            T obj;
            try
            {
                TextReader txtReader = new StringReader(xml.Trim());
                XmlTextReader xmlReader = new XmlTextReader(txtReader);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                obj = (T)serializer.ReadObject(xmlReader);
                txtReader.Close();
                xmlReader.Close();
            }
            catch
            {
                return default(T);
            }
            return obj;
        }
        /// <summary>
        /// 将Xml格式的字符串转换为字节流
        /// </summary>
        /// <param name="xml">参数:Xml格式字符串</param>
        /// <returns>返回值:返回字节流</returns>
        public static byte[] SerializeXml2Byte(string xml)
        {
            return System.Text.Encoding.UTF8.GetBytes(xml);
        }
        public static string IsNull(object value)
        {
            return value == null ? "" : value.ToString();
        }
        public static string GetUnicodeStr(string s)
        {
            try
            {
                //string r = JSON.JsonEncode(s);
                //return s;
                string r = string.Empty;
                byte[] bts = UnicodeEncoding.Unicode.GetBytes(s);
                for (int i = 0; i < bts.Length; i += 2)
                    r += "\\u" + bts[i + 1].ToString("X").PadLeft(2, '0') + bts[i].ToString("X").PadLeft(2, '0');
                return r;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static Guid getGuid()
        {
            return Guid.NewGuid();//.ToString("N");
        }

        public static string getGuidForD()
        {
            return Guid.NewGuid().ToString("D");
        }

        /// <summary>
        /// 取得四舍五入金额
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal getRtBYRoundOff(decimal d)
        {
            try
            {
                decimal dRet = 0;

                dRet = Math.Round(d, 0, MidpointRounding.AwayFromZero);

                return dRet;
            }
            catch
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        REQUEST = 1,
        RESPONSE = 2,
        EXCEPTION = 3
    }

    public enum RequestType
    {
        GET = 1,
        POST = 2
    }
    /// <summary>
    /// 设置(运行时可更改)
    /// </summary>
    //public class Setting
    //{
    //    public class Colors
    //    {
    //        public static System.Drawing.Color ERROR_COLOR = Color.Tomato;
    //        public static System.Drawing.Color Choice=Color.FromArgb(255, 99, 00);
    //        public static System.Drawing.Color NuChoice=Color.FromArgb(99, 99, 99);
    //    }
    //}

    /// <summary>
    /// 自定义辅助公共函数库
    /// </summary>
    public class CustomFun
    {
        /// <summary>
        /// 计算折扣后的优惠价
        /// </summary>
        /// <param name="Rate">房价</param>
        /// <param name="Discount">折扣</param>
        /// <returns>优惠房价(decimal)</returns>
        public static decimal CalculateSaleRate(decimal Rate, decimal Discount)
        {
            return Convert.ToDecimal((Rate * Discount).ToString("F0"));
        }

    }


    public class SyncAgent
    {
        /// <summary>
        /// 参加同步的渠道列表
        /// </summary>
        public static ArrayList ModifyRealRateAgentList
        {
            get
            {
                string sSyncAgent = System.Configuration.ConfigurationManager.AppSettings["SYNCAGENT"];
                string[] sAgent = sSyncAgent.Split(',');
                ArrayList AgentList = new ArrayList(sAgent.Length);
                for (int i = 0; i < sAgent.Length; i++)
                {
                    AgentList.Add(sAgent[i].ToString());
                }
                return AgentList;
            }
        }
        /// <summary>
        /// 所有渠道列表
        /// </summary>
        public static ArrayList AllAgentList
        {
            get
            {
                string sAllAgent = System.Configuration.ConfigurationManager.AppSettings["ALLAGENT"];
                string[] sAgent = sAllAgent.Split(',');
                ArrayList AgentList = new ArrayList(sAgent.Length);
                for (int i = 0; i < sAgent.Length; i++)
                {
                    AgentList.Add(sAgent[i].ToString());
                }
                return AgentList;
            }
        }

        /// <summary>
        /// CRS设置活动时常用的会员类型
        /// </summary>
        public static ArrayList MemberList
        {
            get
            {
                string sMember = System.Configuration.ConfigurationManager.AppSettings["MEMBERCD"];
                string[] sMemberCD = sMember.Split(',');
                ArrayList MemberList = new ArrayList(sMember.Length);
                for (int i = 0; i < sMember.Length; i++)
                {
                    MemberList.Add(sMember[i].ToString());
                }
                return MemberList;
            }
        }
    }
}



