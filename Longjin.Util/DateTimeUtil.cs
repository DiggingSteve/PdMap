/*
* 时间工具类
* 田野 2015-11-4
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Longjin.Util
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public sealed class DateTimeUtil
    {
        private DateTimeUtil() { }

        #region 转换成大写日期
        /// <summary>
        /// 转换成大写日期
        /// false 2011-1-1 转换成 二〇一一年一月一日
        /// true 2011-1-1 转换成 贰零壹壹年壹月壹日
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">是否中文大写</param>
        /// <returns></returns>
        public static string ConvertToCHNDate(DateTime date, bool type)
        {
            StringBuilder sb = new StringBuilder();

            string[] chinese = new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            if (type) chinese = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾" };

            Regex regex = new Regex(@"(d{2}|d{4})(-|/)(d{1}|d{2})(-|/)(d{1}|d{2})");
            string strDate = date.ToString("yyyy-MM-dd");
            if (!regex.IsMatch(strDate) == true)
            {
                string[] str = null;
                if (strDate.Contains("-"))
                {
                    str = strDate.Split('-');
                }
                else
                {
                    if (strDate.Contains("/"))
                    {
                        str = strDate.Split('/');

                    }
                }

                #region //convert year
                for (int i = 0; i < str[0].Length; i++)
                {
                    sb.Append(chinese[int.Parse(str[0][i].ToString())]);
                }
                sb.Append("年");
                #endregion

                #region convert month
                int monthod = int.Parse(str[1]);
                int MN1 = monthod / 10;
                int MN2 = monthod % 10;

                if (MN1 > 1)
                {
                    sb.Append(chinese[MN1]);
                }
                if (MN1 > 0)
                {
                    sb.Append(chinese[10]);
                }
                if (MN2 != 0)
                {
                    sb.Append(chinese[MN2]);
                }
                sb.Append("月");
                #endregion

                #region //convert day
                int day = int.Parse(str[2]);
                int day1 = day / 10;
                int day2 = day % 10;

                if (day1 > 1)
                {
                    sb.Append(chinese[day1]);
                }
                if (day1 > 0)
                {
                    sb.Append(chinese[10]);
                }
                if (day2 > 0)
                {
                    sb.Append(chinese[day2]);
                }
                sb.Append("日");
                #endregion

            }

            return sb.ToString();
        }
        #endregion

        #region 转换成大写日期
        // 2011-1-1 转换成 二〇一一年一月一日
        /// <summary>
        /// 转换成大写日期 
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string ConvertToCHNDate(DateTime date)
        {
            return ConvertToCHNDate(date, false);
        }
        #endregion

        #region 日期转换成中文大写（含时间）
        /// <summary>
        /// 日期转换成中文大写（含时间）
        /// </summary>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ConvertToCHNDateTime(DateTime date, bool type)
        {
            StringBuilder sb = new StringBuilder();

            DateTime day = date.Date;

            int hour = date.Hour;
            int minute = date.Minute;
            
            sb.Append(ConvertToCHNDate(day, type));


            string _s_hour = "";
            if (hour.ToString().Length == 2 && hour.ToString().Substring(0, 1) == "1")
            {
                _s_hour = NumberUtil.ToChineseNumber(hour).Remove(0, 1);
            }
            else 
            {
                _s_hour = NumberUtil.ToChineseNumber(hour);
            }

            string _s_minute = "";
            if (minute.ToString().Length == 2 && minute.ToString().Substring(0, 1) == "1")
            {
                _s_minute = NumberUtil.ToChineseNumber(minute).Remove(0, 1);
            }
            else 
            {
                _s_minute = NumberUtil.ToChineseNumber(minute);
            }

            if (minute == 0)
            {
                sb.Append(_s_hour).Append("时整");
            }
            else
            {
                sb.Append(_s_hour).Append("时").Append(_s_minute).Append("分");
            }

            return sb.ToString();
        }
        #endregion
        
        #region UnixTimeStampToDateTime
        /// <summary>
        /// 将unix时间戳转成成日期时间类型
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime _dt = new DateTime(1970, 1, 1, 0, 0, 0);
            _dt = _dt.AddSeconds(unixTimeStamp).ToLocalTime();

            return _dt;
        }
        /// <summary>
        /// 将日期类型转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds;
        }
        #endregion
    }
}
