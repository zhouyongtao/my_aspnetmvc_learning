using System.Text.RegularExpressions;

namespace Homeinns.Common.Base.Regular
{
    public class RegexHelper
    {
        /// <summary>
        /// 验证输入字符串为数字
        /// </summary>
        /// <param name="P_str_num">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidateNum(string str_num)
        {
            return Regex.IsMatch(str_num, "^[0-9]*$");
        }

        /// <summary>
        /// 验证输入字符串为电话号码
        /// </summary>
        /// <param name="P_str_phone">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidatePhone(string str_phone)
        {
            return Regex.IsMatch(str_phone, @"\d{3,4}-\d{7,8}");
        }

        /// <summary>
        /// 验证输入字符串为传真号码
        /// </summary>
        /// <param name="P_str_fax">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidateFax(string str_fax)
        {
            return Regex.IsMatch(str_fax, @"86-\d{2,3}-\d{7,8}");
        }

        /// <summary>
        /// 验证输入字符串为邮政编码
        /// </summary>
        /// <param name="P_str_postcode">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidatePostCode(string str_postcode)
        {
            return Regex.IsMatch(str_postcode, @"\d{6}");
        }

        /// <summary>
        /// 验证输入字符串为E-mail地址
        /// </summary>
        /// <param name="P_str_email">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidateEmail(string str_email)
        {
            return Regex.IsMatch(str_email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        /// 验证输入字符串为网络地址
        /// </summary>
        /// <param name="P_str_naddress">输入字符串</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool ValidateNAddress(string str_naddress)
        {
            return Regex.IsMatch(str_naddress, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
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
        /// 是否是莫泰酒店(是否包含字母)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsMotel(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"[a-zA-Z]+");
        }
    }
}
