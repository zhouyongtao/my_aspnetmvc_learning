using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Homeinns.Common.Util
{
    public class AppSetting
    {
        public static string UrlInterface = Chk.IsNull(ConfigurationManager.AppSettings["UrlInterface"]);
        public static string UrlMMT = Chk.IsNull(ConfigurationManager.AppSettings["UrlMMT"]);
        public static string UrlComment = Chk.IsNull(ConfigurationManager.AppSettings["UrlComment"]);

        public static string AgentCd = Chk.IsNull(ConfigurationManager.AppSettings["AgentCd"]);

        public static string Opr = Chk.IsNull(ConfigurationManager.AppSettings["Opr"]);

        public static string CacheDir = Chk.IsNull(ConfigurationManager.AppSettings["CacheDir"]);

        public static string UseDomainCookie = Chk.IsNull(ConfigurationManager.AppSettings["UseDomainCookie"]);

        public static string CertTypeNo = Chk.IsNull(ConfigurationManager.AppSettings["CertTypeNo"]);
    }
}
