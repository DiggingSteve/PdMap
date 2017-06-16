using System;
using System.Collections.Generic;
using System.Text;

namespace Longjin.Util
{
    /// <summary>
    /// 金额转换工具类
    /// </summary>
    public sealed class MoneyUtil
    {
        private MoneyUtil() { }

        #region ToUpper
        // 20 转换成 贰拾元整
        /// <summary>
        /// 小写金额转换为大写金额，其中金额小于一万亿，最多两位小数
        /// </summary>
        /// <param name="d">源金额(金额小于一万亿，最多两位小数)</param>
        /// <returns>结果，大写金额</returns>
        public static string ToUpper(decimal d)
        {
            if (d == 0)
            {
                return "零元整";
            }

            string je = d.ToString("####.00");
            if (je.Length > 15)
            {
                return string.Empty;
            }
            je = new String('0', 15 - je.Length) + je;      //若小于15位长，前面补0

            string stry = je.Substring(0, 4);        //取得'亿'单元
            string strw = je.Substring(4, 4);        //取得'万'单元
            string strg = je.Substring(8, 4);        //取得'元'单元
            string strf = je.Substring(13, 2);        //取得小数部分

            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;

            MoneyUtil objConverter = new MoneyUtil();
            str1 = objConverter.m_strConvertSingleUnit(stry, "亿");        //亿单元的大写
            str2 = objConverter.m_strConvertSingleUnit(strw, "万");        //万单元的大写
            str3 = objConverter.m_strConvertSingleUnit(strg, "元");        //元单元的大写


            string str_y = string.Empty;
            string str_w = string.Empty;
            if (je[3] == '0' || je[4] == '0')        //亿和万之间是否有0
            {
                str_y = "零";
            }
            if (je[7] == '0' || je[8] == '0')        //万和元之间是否有0
            {
                str_w = "零";
            }


            string strResult = str1 + str_y + str2 + str_w + str3;    //亿，万，元的三个大写合并

            for (int i = 0; i < strResult.Length; i++)        //去掉前面的"零"   
            {
                if (strResult[i] != '零')
                {
                    strResult = strResult.Substring(i);
                    break;
                }

            }
            for (int i = strResult.Length - 1; i > -1; i--)      //去掉最后的"零" 
            {
                if (strResult[i] != '零')
                {
                    strResult = strResult.Substring(0, i + 1);
                    break;
                }
            }

            if (strResult[strResult.Length - 1] != '元')        //若最后不位不是'元'，则加一个'元'字
            {
                strResult = strResult + "元";
            }

            if (strResult == "零零元")           //若为零元，则去掉"元数"，结果只要小数部分
            {
                strResult = string.Empty;
            }

            if (strf == "00")            //下面是小数部分的转换
            {
                strResult = strResult + "整";
            }
            else
            {
                string strTemp = "";
                strTemp = objConverter.m_strConvertSingleNumeric(strf[0]);
                if (strTemp == "零")
                {
                    strResult = strResult + strTemp;
                }
                else
                {
                    strResult = strResult + strTemp + "角";
                }

                strTemp = objConverter.m_strConvertSingleNumeric(strf[1]);
                if (strTemp == "零")
                {
                    strResult = strResult + "整";
                }
                else
                {
                    strResult = strResult + strTemp + "分";
                }
            }

            if (strResult[0] == '零')
            {
                strResult = strResult.Substring(1);          //防止0.03转为"零叁分"，而直接转为"叁分"
            }

            return strResult;
        }

        #region m_strConvertSingleUnit
        /// <summary>
        /// 把一个单元转为大写，如亿单元，万单元，个单元
        /// </summary>
        /// <param name="p_strNumeric">这个单元的小写数字（4位长，若不足，则前面补零）</param>
        /// <param name="p_strUnit">亿，万，元</param>
        /// <returns>转换结果</returns>
        private string m_strConvertSingleUnit(string p_strNumeric, string p_strUnit)
        {
            string strResult = string.Empty;
            if (p_strNumeric == "0000")
            {
                return strResult;
            }
            string strTemp1 = this.m_strConvertSingleNumeric(p_strNumeric[0]);
            string strTemp2 = this.m_strConvertSingleNumeric(p_strNumeric[1]);
            string strTemp3 = this.m_strConvertSingleNumeric(p_strNumeric[2]);
            string strTemp4 = this.m_strConvertSingleNumeric(p_strNumeric[3]);

            if (strTemp1 != "零")
            {
                strResult = strResult + strTemp1 + "仟";
            }
            else
            {
                strResult = strResult + strTemp1;
            }

            if (strTemp2 != "零")
            {
                strResult = strResult + strTemp2 + "佰";
            }
            else
            {
                if (strTemp1 != "零")           //保证若有两个零'00'，结果只有一个零，下同
                    strResult = strResult + strTemp2;
            }

            if (strTemp3 != "零")
            {
                strResult = strResult + strTemp3 + "拾";
            }
            else
            {
                if (strTemp2 != "零")
                    strResult = strResult + strTemp3;
            }

            if (strTemp4 != "零")
            {
                strResult = strResult + strTemp4;
            }

            if (strResult[0] == '零')            //若第一个字符是'零'，则去掉
            {
                strResult = strResult.Substring(1);
            }
            if (strResult[strResult.Length - 1] == '零')        //若最后一个字符是'零'，则去掉
            {
                strResult = strResult.Substring(0, strResult.Length - 1);
            }

            return strResult + p_strUnit;
        }
        #endregion

        #region m_strConvertSingleNumeric
        /// <summary>
        /// 单个数字转为大写
        /// </summary>
        /// <param name="p_chrSingleNumeric">小写阿拉伯数字 0---9</param>
        /// <returns>大写数字</returns>
        private string m_strConvertSingleNumeric(char p_chrSingleNumeric)
        {
            string strResult = "";
            switch (p_chrSingleNumeric)
            {
                case '0':
                    strResult = "零";
                    break;
                case '1':
                    strResult = "壹";
                    break;
                case '2':
                    strResult = "贰";
                    break;
                case '3':
                    strResult = "叁";
                    break;
                case '4':
                    strResult = "肆";
                    break;
                case '5':
                    strResult = "伍";
                    break;
                case '6':
                    strResult = "陆";
                    break;
                case '7':
                    strResult = "柒";
                    break;
                case '8':
                    strResult = "捌";
                    break;
                case '9':
                    strResult = "玖";
                    break;
            }
            return strResult;
        }
        #endregion

        #endregion


    }
}
