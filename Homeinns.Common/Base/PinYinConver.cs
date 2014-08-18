using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.ObjectModel;

namespace Homeinns.Common.Base
{
    /// <summary>
    /// 汉字转拼音类
    /// </summary>
    public class PinYinConver
    {
        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string Convert(string hzString)
        {
            if (string.IsNullOrEmpty(hzString))
                return "";

            char[] noWChar = hzString.ToCharArray();
            string txt = "";
            for (int j = 0; j < noWChar.Length; j++)
            {
                if (IsValidChar(noWChar[j]))
                {
                    txt += ConvertToFirstPinYin(noWChar[j].ToString());
                }
            }
            return txt;
        }
        /// <summary> 
        /// 将字符串转换成首个拼音 
        /// </summary> 
        /// <param name="chineseStr">字符串</param> 
        /// <returns></returns> 
        public static string ConvertToFirstPinYin(string chineseStr)
        {
            if (chineseStr == null)
                return "";

            char[] charArray = chineseStr.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in charArray)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    ReadOnlyCollection<string> pyColl = chineseChar.Pinyins;
                    foreach (string py in pyColl)
                    {
                        if (py != null)
                        {
                            sb.Append(py.Substring(0, 1));
                            break;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return sb.ToString();
        }
        /// <summary> 
        /// 将字符串转换成拼音 
        /// </summary> 
        /// <param name="chineseStr">姓名字符串</param> 
        /// <param name="includeTone">是否包含音调</param> 
        /// <returns></returns> 
        public static string ConvertToPinYin(string chineseStr, bool includeTone)
        {
            if (chineseStr == null)
                throw new ArgumentNullException("chineseStr");

            char[] charArray = chineseStr.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in charArray)
            {

                ReadOnlyCollection<string> pyColl = null;
                try
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    pyColl = chineseChar.Pinyins;
                }
                catch
                {
                    continue;
                }
                foreach (string py in pyColl)
                {
                    sb.Append(py);
                }
            }
            if (!includeTone)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (char c in sb.ToString())
                {
                    if (!char.IsNumber(c))
                        sb2.Append(c);
                }
                return sb2.ToString();
            }
            return sb.ToString();
        }
        public static string ConvertToPinYin(string chineseStr)
        {
            return ConvertToPinYin(chineseStr, true);
        }
        public static bool IsValidChar(char ch)
        {
            return ChineseChar.IsValidChar(ch);
        }
        /// <summary> 
        /// 是否为有效的中文字 
        /// </summary> 
        /// <param name="chn"></param> 
        /// <returns></returns> 
        public static bool IsValidChinese(string chn)
        {
            if (chn == null)
                return false;

            foreach (char c in chn)
            {
                if (!IsValidChar(c))
                    return false;
            }
            return true;
        }
    }
}
