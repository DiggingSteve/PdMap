/*
* 数字工具类
* 田野 2015-11-4
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Longjin.Util
{
    /// <summary>
    /// 数字工具类
    /// </summary>
    public sealed class NumberUtil
    {
        private NumberUtil() { }

        #region ToChineseNumber
        // 20 转换成 二十
        /// <summary>
        /// 阿拉伯数字转换成中文数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToChineseNumber(long number)
        {
            return ToChineseNumber(number, false);
        }

        /// <summary>
        /// 阿拉伯数字转换成中文数字
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="isupper">是否中文大写</param>
        /// <returns></returns>
        public static string ToChineseNumber(long number, bool isupper)
        {
            string[] cStr = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "", "十", "百", "千" };
            if (isupper) cStr = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "", "拾", "佰", "仟" };
            string[] unitStr = new string[] { "", "万", "亿", "万", "兆" };
            string result = string.Empty;
            for (int i = 0; i < number.ToString().Length; i++)
            {
                int temp = (int)((long)(number / (long)Math.Pow(10, i)) % 10);//获取第i位的数字  
                if (i % 4 == 0) result = unitStr[(int)i / 4] + result;//检查是否需要加上万或亿等  
                result = cStr[temp] + cStr[10 + i % 4] + result;
            }
            result = Regex.Replace(result, "(零[十百千])+", "零");
            if (isupper) result = Regex.Replace(result, "(零[拾佰仟])+", "零");
            result = Regex.Replace(result, "零{2,}", "零");
            result = Regex.Replace(result, "零([万亿兆])", "$1");
            if (result.Length > 1) result = result.TrimEnd('零');
            return result;
        }
        #endregion
    }
}
