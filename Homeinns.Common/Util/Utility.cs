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
    /// ���ݿ⸨������
    /// </summary>
    public class DBFunc
    {

        /// <summary>
        /// ��һ��Stringֵ���˼��ϵ����ţ���Ҫ���ڹ���SQL����
        /// </summary>
        /// <param name="str">Ҫ���ӵ����ŵĲ���ֵ</param>
        /// <returns>�ӹ������ŵ�String</returns>
        public static string addSingleQuotes(string str)
        {
            return String.Format("'{0}'", str);
        }

        /// <summary>
        /// ���Ҫ����sql��ѯ����UI����ֵ�����Ƿ����ע��ʽ�Ŀ��ܡ�
        /// </summary>
        /// <param name="str">�����Ĵ�</param>
        /// <returns>�������true��ʾstrΪ�Ϸ������룬false��ʾstrΪ�Ƿ�������</returns>
        public static bool checkSqlInputValue(string str)
        {
            const string unsafeMask = @"^\?(.*)(select |delete |count\(|drop table|updata |truncate |asc\(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|:|'|net user| or )(.*)$";
            if (System.Text.RegularExpressions.Regex.Match(str.ToLower(), unsafeMask).Success)
                return false;
            else return true;
        }
        /// <summary>
        /// �����ַ������ֽڳ���
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        /// <returns>�ַ����ֽڳ���</returns>
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

    /// ��������Ƿ�Ϸ�
    /// </summary>
    public class Chk
    {
        private Chk()
        {
            //
            // ������ṩ��̬���ܺ�������ֹ����ʵ��
            //
        }

        /// <summary>
        /// ����Ƿ�Ϊ��
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
        /// ����Ƿ�����Ч������
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
        /// ����Ƿ�������
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
        /// ���Զ�������Ƿ���Pingͨ
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
                    sPingrst = "����";
                    IsLink = true;
                }
                else if (strRst.IndexOf("Destination host unreachable") != -1)
                {
                    sPingrst = "�޷�����Ŀ������";
                    IsLink = false;
                }
                else if (strRst.IndexOf("Request timed out") != -1)
                {
                    sPingrst = "��ʱ";
                    IsLink = false;
                }
                else if (strRst.IndexOf("Unknown host") != -1)
                {
                    sPingrst = "�޷���������";
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
    /// ����
    /// </summary>
    public class Const
    {
        public const string HomeinnsTitle = "��ҿ�ݾƵ�-";
        public const string ChinaPayCode = "01";
        public const string billCode = "02";

        public const string CashCode = "00";
        /// <summary>
        /// ֧����
        /// </summary>
        public static string AlipayCode = "03";
        /// <summary>
        /// ������������֧��
        /// </summary>
        public const string ICBCCode = "07";
        /// <summary>
        /// ������������֧��
        /// </summary>
        public const string CCBCode = "08";
        /// <summary>
        /// ������������֧��
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
        /// MM��dd��
        /// </summary>
        public const string DateFormatString3 = @"MM��dd��";

        /// <summary>
        /// YYYY��MM��dd��
        /// </summary>
        public const string DateFormatString4 = @"yyyy��MM��dd��";

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

        public const string RoomNull = "����";

        public const string RoomFull = "����";

        public const string WebResvDesp = "cnet";

        public const string WebOpr = "Web";

        public const string AgentCd = "800333";

        /// <summary>
        ///CRS�����ſ���������������
        /// </summary>
        public const string Hour = "18";

        /// <summary>
        /// Eq�Ƶ���ʩ
        /// </summary>
        public const string DespEq = "Eq";

        /// <summary>
        /// Tr��ͨ
        /// </summary>
        public const string DespTr = "Tr";

        /// <summary>
        /// Rm�ͷ�����
        /// </summary>
        public const string DespRm = "Rm";

        /// <summary>
        /// Dr�г�
        /// </summary>
        public const string DespDr = "Dr";

        /// <summary>
        /// Cr���ÿ�
        /// </summary>
        public const string DespCr = "Cr";


        /// <summary>
        /// ��˾���ۿ�
        /// </summary>
        public const string HelpcyType = "cyType";
        /// <summary>
        /// �Ż�����-��Դ��
        /// </summary>
        public const string HelpGustTp = "ActGustTp";
        /// <summary>
        /// ����/֧��
        /// </summary>
        public const string HelpPayTp = "Activity";
        /// <summary>
        /// �����б�
        /// </summary>
        public const string HelpBankTp = "BANK";
        /// <summary>
        /// ��Ա����
        /// </summary>
        public const string HelpCardTp = "CardTp_H";
        /// <summary>
        /// ֤��
        /// </summary>
        public const string HelpPassTp = "CtfTp";
        /// <summary>
        /// ��Դ
        /// </summary>
        public const string HelpGdTp = "GustKind";
        /// <summary>
        /// �Ƶ��Ǽ�
        /// </summary>
        public const string HelpHotelTp = "hotelclass";
        /// <summary>
        /// ����/ֱӪ
        /// </summary>
        public const string HelpMagTp = "hoteltype";
        /// <summary>
        /// Memoѡ�����
        /// </summary>
        public const string HelpMemoTp = "note";
        /// <summary>
        /// Ԥ����Դ
        /// </summary>
        public const string HelpResvTp = "resvtype";
        /// <summary>
        /// ȷ���б�
        /// </summary>
        public const string HelpReturnTp = "returntp";
        /// <summary>
        /// ��̬�������
        /// </summary>
        public const string HelpLockTp = "SF";
        /// <summary>
        /// �Ƶ��Ƽ�
        /// </summary>
        public const string HelpSaleTp = "RL";

        /// <summary>
        /// Descript�ֶ���
        /// </summary>
        public const string HelpText = "Descript";

        /// <summary>
        /// Code�ֶ���
        /// </summary>
        public const string HelpValue = "CD";

        //--------------���Ŷ��Žӿ����� START-----------
        //�����ļ�����
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

        //--------------���Ŷ��Žӿ����� END-----------



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
    //    /// ����Cookiesֵ
    //    /// </summary> 
    //    /// <param name="DomainName">����</param> 
    //    /// <param name="CookieName">Cookie������</param>
    //    /// <param name="CookieValue">Cookie��ֵ</param>
    //    /// <param name="ExpiresDayValue">����ʱ��,��λ,��</param>
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
    //    /// ȡCookiesֵ
    //    /// ����ȡ��������
    //    /// </summary> 
    //    /// <param name="DomainName">����</param> 
    //    /// <param name="CookieName">Cookie������</param>
    //    public static string GetCookies(string DomainName, string CookieName)
    //    {
    //        CookieName = System.Web.HttpContext.Current.Server.UrlEncode(CookieName);
    //        //CookieName=Security.Encrypt(CookieName,Security.myKey);//���ܼ���

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
    /// ��ȫ
    /// </summary>
    public class Safe
    {
        public static string SafeSqlLikeClauseLiteral(string inputSQL)
        {
            // ���������滻��
            // '  ���  ''
            // [  ���  [[]
            // %  ���  [%]
            // _  ���  [_]

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

        #region Sql��ע��
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
    ///Utility ��ժҪ˵��
    /// </summary>
    public class Utility
    {
        public Utility()
        {
            //
            //TODO: �ڴ˴���ӹ��캯���߼�
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
        /// ���IP��ַ��ʽ
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// ��������
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
        /// ������ϵ�л�Ϊ�������ֽ���
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
        /// ��������xml��ϵ�л�Ϊ����
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
        /// �����л�
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
        /// ��Xml��ʽ���ַ���ϵ�л�Ϊobject���͵Ķ���
        /// </summary>
        /// <typeparam name="T">����ռλ��</typeparam>
        /// <param name="xml">����:Xml��ʽ�ַ���</param>
        /// <returns>����һ��T���͵Ķ���</returns>
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
        /// ��ָ���Ķ���ϵ�л�ΪXml��ʽ���ַ���
        /// </summary>
        /// <typeparam name="T">����չλ��</typeparam>
        /// <param name="obj">����:Ҫ����ϵ�л��Ķ���</param>
        /// <returns>����Xml��ʽ���ַ���</returns>
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
        /// �����󼯺�����ת��Ϊ�ֽ�����
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
            //���תxml�Ļ��������ע��ȥ��
            //Encoding utf = Encoding.UTF8 ;
            //string xml = utf.GetString(buffer).Trim();
        }
        /// <summary>
        /// ����ʵ��ת��xml
        /// </summary>
        /// <param name="item">����ʵ��</param>
        /// <returns></returns>
        public static string EntityToXml<T>(T item)
        {
            IList<T> items = new List<T>();
            items.Add(item);
            return EntityToXml(items);
        }
        /// <summary>
        /// ����ʵ����ת��xml
        /// </summary>
        /// <param name="items">����ʵ����</param>
        /// <returns></returns>
        public static string EntityToXml<T>(IList<T> items)
        {
            //����XmlDocument�ĵ�
            XmlDocument doc = new XmlDocument();
            //������Ԫ��
            XmlElement root = doc.CreateElement(typeof(T).Name + "s");
            //��Ӹ�Ԫ�ص���Ԫ�ؼ�
            foreach (T item in items)
            {
                EntityToXml(doc, root, item);
            }
            //��XmlDocument�ĵ���Ӹ�Ԫ��
            doc.AppendChild(root);

            return doc.InnerXml;
        }
        /// <summary>
        /// ������ת��Ϊxml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        /// <param name="item"></param>
        private static void EntityToXml<T>(XmlDocument doc, XmlElement root, T item)
        {
            //����Ԫ��
            XmlElement xmlItem = doc.CreateElement(typeof(T).Name);
            //��������Լ�
            System.Reflection.PropertyInfo[] propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo pinfo in propertyInfo)
            {
                if (pinfo != null)
                {
                    //������������
                    string name = pinfo.Name;
                    //��������ֵ
                    string value = String.Empty;

                    if (pinfo.GetValue(item, null) != null)
                        value = pinfo.GetValue(item, null).ToString();//��ȡ��������ֵ
                    //����Ԫ�ص�����ֵ
                    xmlItem.SetAttribute(name, value);
                }
            }
            //��������Ԫ��
            root.AppendChild(xmlItem);
        }
        /// <summary>
        /// Xmlת�ɶ���ʵ����
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
        /// Xmlת�ɶ���ʵ��
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
        /// �����󼯺�����ת��Ϊxml��ʽ�ַ���
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
        /// ʹ��System.Runtime.Serialization.DataContractSerializer�ཫList&lt;T&gt;��������ϵ�л�Ϊxml��ʽ�ַ���
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
        /// ʹ��System.Runtime.Serialization.DataContractSerializer�ཫ����object�������л�Ϊxml�ַ���
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
        /// ʹ��System.Runtime.Serialization.DataContractSerializer�ཫxml��ʽ�ַ���ת��ΪList&lt;T&gt;��������
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
        /// ʹ��System.Runtime.Serialization.DataContractSerializer�ཫxml��ʽ�ַ���ת��Ϊ����object����
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
        /// ��Xml��ʽ���ַ���ת��Ϊ�ֽ���
        /// </summary>
        /// <param name="xml">����:Xml��ʽ�ַ���</param>
        /// <returns>����ֵ:�����ֽ���</returns>
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
        /// ȡ������������
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
    /// ��־����
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
    /// ����(����ʱ�ɸ���)
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
    /// �Զ��帨������������
    /// </summary>
    public class CustomFun
    {
        /// <summary>
        /// �����ۿۺ���Żݼ�
        /// </summary>
        /// <param name="Rate">����</param>
        /// <param name="Discount">�ۿ�</param>
        /// <returns>�Żݷ���(decimal)</returns>
        public static decimal CalculateSaleRate(decimal Rate, decimal Discount)
        {
            return Convert.ToDecimal((Rate * Discount).ToString("F0"));
        }

    }


    public class SyncAgent
    {
        /// <summary>
        /// �μ�ͬ���������б�
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
        /// ���������б�
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
        /// CRS���ûʱ���õĻ�Ա����
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



