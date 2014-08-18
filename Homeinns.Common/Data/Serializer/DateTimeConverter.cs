using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Homeinns.Common.Data.Serialize
{
    /// <summary>
    /// 弃用
    /// </summary>
    public class DateTimeConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary,Type type,JavaScriptSerializer serializer)
        {
            if (dictionary.ContainsKey("DateTime"))
                //return new DateTime(long.Parse(dictionary["DateTime"].ToString()),DateTimeKind.Unspecified);
                return Convert.ToDateTime(dictionary["DateTime"].ToString());
            return null;
        }

        public override IDictionary<string, object> Serialize(object obj,JavaScriptSerializer serializer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null) return result;
            //result["DateTime"] = ((DateTime)obj).Ticks;
            result["DateTime"] = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
            //result["DateTime"] = "new Date(" + ((DateTime)obj).Ticks + ")";
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>() { typeof(DateTime?), typeof(DateTime) }; }
        }
    }
}
