/*
* 字符串操作类
* 田野 2015-11-4
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace Longjin.Util
{
    /// <summary>
    /// 字符串操作类
    /// </summary>
    public sealed class StringUtil
    {
        private StringUtil() { }

        #region ConvertToHTML
        /// <summary>
        /// 将Text字符串转换成HTML格式字符串
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static string ConvertToHTML(string str)
        {
            if (str == null || str == string.Empty) return string.Empty;
            return str.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace(" ", "&nbsp;");
        }
        #endregion

        #region Ellipsis
        /// <summary>
        /// 按指定的长度截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="len">长度</param>
        /// <param name="suffix">后缀</param>
        /// <returns></returns>
        public static string Ellipsis(string str, int len, string suffix)
        {
            if (str == null || str.Length == 0 || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;

            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + suffix;
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region RemoveHtmlTag
        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="html">要移除HTML标签的字符串</param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string html)
        {
            //string temp=string.Empty;
            if (html != null && html != "")
            {
                html = Regex.Replace(html, "<[^>]*>", "");
                html = html.Replace("&nbsp;", " ");
            }
            return html;
        }
        #endregion

        #region 汉字转拼音首字母
        /// <summary>
        /// 汉字转拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留           
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母     
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "";
        }
        #endregion

        #region ConvertChsToFirstLetterPinyin
        /// <summary>
        /// 将汉字转换成拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertChsToFirstLetterPinyin(string str)
        {
            string result = "";

            if (string.IsNullOrEmpty(str)) return "";

            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    result += chineseChar.Pinyins[0].Substring(0, 1).ToUpper();
                }
                else
                {
                    result += c.ToString();
                }

            }

            return result;
        }
        #endregion

        #region ConvertChsToPinyin
        /// <summary>
        /// 将汉字转换成拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertChsToPinyin(string str)
        {
            string result = "";

            if (string.IsNullOrEmpty(str)) return "";

            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    result += chineseChar.Pinyins[0].Substring(0, 1).ToUpper() + chineseChar.Pinyins[0].Substring(1, chineseChar.Pinyins[0].Length - 2).ToLower();
                }
                else
                {
                    result += c.ToString();
                }
            }

            return result;
        }
        #endregion

        #region GetChineseCharCount
        /// <summary>
        /// 获取字符串中的汉字字数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetChineseCharCount(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;

            int count = 0;

            foreach (char c in str)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    count++;
                }
            }

            return count;
        }
        #endregion

        #region GetTokenCharCount
        /// <summary>
        /// 获取字符串中的标点符号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetTokenCharCount(string str)
        {
            int count = 0;

            if (string.IsNullOrEmpty(str)) return 0;

            Regex seperatorReg = new Regex(@"[,;.!?'，。？：；‘’！“”—……、《》<>{}【】]", RegexOptions.IgnorePatternWhitespace);

            MatchCollection items = seperatorReg.Matches(str);

            count = items.Count;

            return count;
        }
        #endregion

        #region 将Text字符串转换成javascript格式字符串
        /// <summary>
        ///  将Text字符串转换成javascript格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToJSString(string str)
        {
            if (str == null || str == string.Empty) return string.Empty;
            return str.Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("\r\n", ""); ;
        }
        #endregion

        #region 判断字符是否为GUID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsGuid(string strSrc)
        {
            if (String.IsNullOrEmpty(strSrc)) { return false; }

            bool _result = false;
            try
            {
                Guid _t = new Guid(strSrc);
                _result = true;
            }
            catch { }
            return _result;
        }
        #endregion
    }
}
