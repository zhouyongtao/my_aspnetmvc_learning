using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Homeinns.Common.Base.Encrypt
{
    public class EncryptUtil
    {
        #region Member Variable

        /// <summary>
        /// Aes Key (AES加密所需32位密匙).
        /// </summary>
        private readonly string _strAesKey = null;

        #endregion

        #region Construct Function

        public EncryptUtil()
        { }

        public EncryptUtil(string aesKey)
        {
            this._strAesKey = aesKey;
        }

        #endregion

        #region AES 加密/解密方式
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public string AES_Encrypt(string str, System.Security.Cryptography.PaddingMode paddingMode = System.Security.Cryptography.PaddingMode.PKCS7)
        {
            if (str != null)
            {
                Byte[] keyArray = Encoding.UTF8.GetBytes(this._strAesKey);
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
                rDel.Padding = paddingMode;
                System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            return null;
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public string AES_Decrypt(string str, System.Security.Cryptography.PaddingMode paddingMode = System.Security.Cryptography.PaddingMode.PKCS7)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var keyArray = Encoding.UTF8.GetBytes(this._strAesKey);
                var toEncryptArray = Convert.FromBase64String(str);

                var rijndaelManaged = new System.Security.Cryptography.RijndaelManaged { Key = keyArray, Mode = System.Security.Cryptography.CipherMode.ECB, Padding = paddingMode };
                System.Security.Cryptography.ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            return str;
        }
        #endregion

        #region MD5加密
        public string MD5_Encrypt(string str)
        {
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            Encoding encoding = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();
            foreach (var element in md5Hasher.ComputeHash(encoding.GetBytes(str)))
            {
                sb.Append(element.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion

        #region 携程
        public static string AESEncrypt(string plainText, string strKey)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组	
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
            cs.Close();
            ms.Close();
            string cipherText = Convert.ToBase64String(cipherBytes);
            return cipherText;
        }
        public static string AESDecrypt(string cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptBytes = new byte[cipherBytes.Length];
            MemoryStream ms = new MemoryStream(cipherBytes);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            string decryptText = Encoding.UTF8.GetString(decryptBytes);
            return decryptText == null ? "" : decryptText.Replace("\0", "");
        }
        #endregion
    }
}
