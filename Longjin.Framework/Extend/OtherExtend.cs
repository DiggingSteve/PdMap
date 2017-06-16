using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// 其它扩展
    /// </summary>
    public static class OtherExtend
    {
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearHtmlTag(this string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }
            string strText = Regex.Replace(html, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");
            strText = strText.EscapeChars();
            strText = Regex.Replace(strText, "&[^;]+;", "");
            strText = strText.Trim();
            return strText;
        }

        /// <summary>
        /// 截取制定长度的字符串
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="length"></param>
        ///<param name="ellipsis">是否加省略号</param>
        /// <returns></returns>
        public static string SubLenght(this string _this, int length = 100, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(_this))
            {
                return string.Empty;
            }
            if (_this.Length > length)
            {
                if (ellipsis)
                {
                    return string.Format("{0}......", _this.Substring(0, length));
                }
                else
                {
                    return _this.Substring(0, length);
                }
            }
            else
            {
                return _this;
            }
        }

        /// <summary>
        /// 获取 Dictionary 的值  不抛异常
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="_this"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> _this, TKey key, TValue def = default(TValue))
        {
            if (_this.ExistsData())
            {
                if (_this.ContainsKey(key))
                {
                    return _this[key];
                }
            }
            return def;
        }

        /// <summary>
        /// 是否是默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string IsDefaultReturnDefT<T>(this T _this, string def = "")
        {
            if (_this == null)
            {
                return def;
            }
            if (_this.Equals(default(T)))
            {
                return def;
            }
            var typeName = _this.GetType().FullName;
            if (typeName == "System.String")
            {
                if (string.IsNullOrEmpty(_this as string))
                {
                    return def;
                }
            }
            return _this.ToString();
        }

        /// <summary>
        /// 数字转汉字数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToZH(this int num)
        {
            switch (num)
            {
                case 0:
                    return "零";
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
            }
            return num.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string IsNullOrEmptyReturnDef(this string _this, string def = "")
        {
            if (string.IsNullOrEmpty(_this))
            {
                return def;
            }
            return _this;
        }

        public static string IsDefaultEmptyReturnDef(this decimal _this, string def = "")
        {
            if (_this == 0)
            {
                return def;
            }
            return _this.ToString();
        }

        /// <summary>
        /// 获取Cookice 值
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TryGetValue(this HttpCookie _this)
        {
            return _this == null ? null : _this.Value;
        }

        /// <summary>
        /// 获取指定时间的 汉字星期N
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string GetWeek(this DateTime _this)
        {
            var week = _this.DayOfWeek;
            switch (week)
            {
                case DayOfWeek.Monday:
                    return "一";
                case DayOfWeek.Tuesday:
                    return "二";
                case DayOfWeek.Wednesday:
                    return "三";
                case DayOfWeek.Thursday:
                    return "四";
                case DayOfWeek.Friday:
                    return "五";
                case DayOfWeek.Saturday:
                    return "六";
                case DayOfWeek.Sunday:
                    return "日";
            }
            return string.Empty;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page">从1开始</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int page, int size)
        {
            return source.Skip((page - 1) * size).Take(size);
        }
    }
}
