/*
* 安全工具类
* 田野 2015-11-4
*/
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Longjin.Util
{
    /// <summary>
    /// 安全工具类
    /// </summary>
    public sealed class SecureUtil
    {

        private const string KEY = "0x1fpd_map!~";

        #region MD5加密
        /// <summary>
        /// 字符串按MD5进行加密
        /// </summary>
        public static string EncryptStringByMD5(string strPassword)
        {
            //获取要加密的字段，并转化为Byte[]数组 
            byte[] data = System.Text.Encoding.Unicode.GetBytes(strPassword.ToCharArray());
            //建立加密服务 
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //加密Byte[]数组 
            byte[] result = md5.ComputeHash(data);
            //将加密后的数组转化为字段 
            string sResult = System.Text.Encoding.Unicode.GetString(result);
            //作为密码方式加密 
            string EnPswdStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword.ToString(), "MD5");

            return EnPswdStr;
        }
        #endregion

        #region DES加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">需要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptStringByDES(string pToEncrypt)
        {
            return EncryptStringByDES(pToEncrypt, KEY);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">需要加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string EncryptStringByDES(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //建立加密对象的密钥和偏移量
            //使得输入密码必须输入英文文本
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
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
        #endregion

        #region DES解密
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <returns></returns>
        public static string DecryptStringByDES(string pToDecrypt)
        {
            return DecryptStringByDES(pToDecrypt, KEY);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DecryptStringByDES(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());

        }
        #endregion


        #region SHA-256加密
        /// <summary>
        /// sha256加密
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string EncryptStringBySHA256(string strData)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(strData);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] retVal = sha256.ComputeHash(bytValue);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion


        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Des3EncodeCBC(string key, byte[] data)
        {
            byte[] keyByte = Convert.FromBase64String(key);
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            MemoryStream mStream = new MemoryStream();
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.ECB;
            tdsp.Padding = PaddingMode.PKCS7;
            CryptoStream cStream = new CryptoStream(mStream, tdsp.CreateEncryptor(keyByte, iv), CryptoStreamMode.Write);
            cStream.Write(data, 0, data.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ret;
        }


        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Des3DecodeCBC(string key, byte[] data)
        {
            byte[] keyByte = Convert.FromBase64String(key);
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            MemoryStream msDecrypt = new MemoryStream(data);
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.Mode = CipherMode.CBC;
            tdsp.Padding = PaddingMode.PKCS7;
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, tdsp.CreateDecryptor(keyByte, iv), CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[data.Length];
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
            return fromEncrypt;
        }

        /// <summary>
        /// 16进制字符串转成直接数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(String str)
        {
            if (str == null)
            {
                return null;
            }
            else if (str.Length < 2)
            {
                return null;
            }
            else
            {
                int len = str.Length / 2;
                byte[] buffer = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    var temp = str.Substring(i * 2, 2);
                    buffer[i] = (byte)Convert.ToInt32(temp, 16);
                }
                return buffer;
            }
        }
        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetDes3EncryptedText(byte[] key, byte[] data)
        {
            byte[] k = new byte[24];
            int len = data.Length;
            if (data.Length % 8 != 0)
            {
                len = data.Length - data.Length % 8 + 8;
            }
            byte[] needData = null;
            if (len != 0)
                needData = new byte[len];
            for (int i = 0; i < len; i++)
            {
                needData[i] = 0x00;
            }
            Buffer.BlockCopy(data, 0, needData, 0, data.Length);
            if (key.Length == 16)
            {
                Buffer.BlockCopy(key, 0, k, 0, key.Length);
                Buffer.BlockCopy(key, 0, k, 16, 8);
            }
            else
            {
                Buffer.BlockCopy(key, 0, k, 0, 24);
            }
            var des3 = new TripleDESCryptoServiceProvider();
            des3.Key = k;
            des3.Mode = CipherMode.ECB;
            des3.Padding = PaddingMode.None;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des3.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 将加密结果转成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String GetByteToHex(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                String temp = string.Format("{0:X}", ((int)data[i]) & 0xFF);
                for (int t = temp.Length; t < 2; t++)
                {
                    sb.Append("0");
                }
                sb.Append(temp);
            }
            return sb.ToString();
        }

        /// <summary>
        /// DES加密 
        /// </summary>
        /// <param name="pToEncrypt">需要加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string EncryptByDes(string pToEncrypt, string sKey)
        {

            //建立一個Mode=ECB, Padding=None, Key為12345678的DESCryptoServiceProvider
            DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
            // 16进制字符串转成8字节key
            objDESCryptoServiceProvider.Key = HexToBytes(sKey);
            objDESCryptoServiceProvider.Mode = CipherMode.ECB;
            objDESCryptoServiceProvider.Padding = PaddingMode.None;

            //用UTF-8編碼, 將字串轉為byte[]
            byte[] bysData = Encoding.UTF8.GetBytes(pToEncrypt);
            //因為PaddingMode.None的關係, byte[]的長度要是8的倍數
            byte[] bysFixSizeData = new byte[(int)Math.Ceiling(bysData.Length / 8.0) * 8];
            //將資料複製到長度為8的倍數的byte[]
            Array.Copy(bysData, bysFixSizeData, bysData.Length);

            //進行加密
            byte[] bysEncrypted = objDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bysFixSizeData, 0, bysFixSizeData.Length);
            //將byte[]轉為Base64的字串
           return GetByteToHex(bysEncrypted);
        }


    }
}
