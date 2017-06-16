using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace System
{
    /// <summary>
    /// 编解码扩展
    /// </summary>
    public static class EncodingExtend
    {
        /// <summary>
        /// 特殊字符 转义 编码               入库
        /// </summary>
        /// <param name="_this">原字符串</param>
        /// <returns>返回字符串</returns>
        public static string EscapeChars(this string _this)
        {
            if (string.IsNullOrEmpty(_this))
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.HtmlEncode(_this);
            }
        }

        /// <summary>
        /// 特殊字符 反转义     解码                   出库
        /// </summary>
        /// <param name="_this">原字符串</param>
        /// <returns>返回字符串</returns>
        public static string UnescapeChars(this string _this)
        {
            if (string.IsNullOrEmpty(_this))
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.HtmlDecode(_this);
            }
        }


        private const string KEY = "0x1fpd_map!~";

        /// <summary>
        /// 字符串按MD5进行加密
        /// </summary>
        public static string MD5(this string str)
        {
            //获取要加密的字段，并转化为Byte[]数组 
            byte[] data = System.Text.Encoding.Unicode.GetBytes(str.ToCharArray());
            //建立加密服务 
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //加密Byte[]数组 
            byte[] result = md5.ComputeHash(data);
            //将加密后的数组转化为字段 
            string sResult = System.Text.Encoding.Unicode.GetString(result);
            //作为密码方式加密 
            string EnPswdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str.ToString(), "MD5");

            return EnPswdStr;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptDES(this string str)
        {
            return EncryptDES(str, KEY);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string EncryptDES(this string str, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中
            byte[] inputByteArray = System.Text.Encoding.Default.GetBytes(str);

            //建立加密对象的密钥和偏移量
            //使得输入密码必须输入英文文本
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns></returns>
        public static string DecryptDES(this string str)
        {
            return DecryptDES(str, KEY);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DecryptDES(this string str, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[str.Length / 2];
            for (int x = 0; x < str.Length / 2; x++)
            {
                int i = Convert.ToInt32(str.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());

        }
    }
}
