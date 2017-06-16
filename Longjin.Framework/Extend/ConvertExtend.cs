using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 数据类型转换扩展
    /// </summary>
    public static class ConvertExtend
    {

        /// <summary>
        /// 转换为INT类型  失败为0
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def">转换失败的默认值</param>
        /// <returns></returns>
        public static int ToInt(this object _this, int def = 0)
        {
            return _this.ToNumber<int>(def);
        }

        public static decimal ToDecimal(this object _this, int def = 0)
        {
            return _this.ToNumber<decimal>(def);
        }

        public static long ToLong(this object _this, long def = 0)
        {
            return _this.ToNumber<long>(def);
        }

        public static byte ToByte(this object _this, byte def = 0)
        {
            return _this.ToNumber<byte>(def);
        }

        /// <summary>
        /// 转换为数值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ToNumber<T>(this object _this, T def = default(T)) where T : struct
        {
            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            T result = def;
            if (_this is String)
            {
                try
                {
                    var _ret = Convert.ChangeType(_this, typeof(T));
                    return (T)_ret;
                }
                catch (Exception ex)
                {
                    return def;
                }
            }
            else
            {
                if (_this is Boolean)
                {
                    return (T)((bool)_this ? (object)1 : (object)0);
                }
                return (T)_this;
            }
        }

        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def">转换失败的默认值</param>
        /// <returns></returns>
        public static bool ToBool(this object _this, bool def = false)
        {
            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            bool result = false;
            if (_this is String)
            {
                if (bool.TryParse(_this.ToString(), out result))
                {
                    return result;
                }
            }
            else
            {
                return (bool)_this;
            }
            return def;
        }

        /// <summary>
        /// 转换为GUID
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object _this, Guid def = default(Guid))
        {
            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            Guid result = def;
            if (_this is String)
            {
                if (Guid.TryParse(_this.ToString(), out result))
                {
                    return result;
                }
            }
            else
            {
                return (Guid)_this;
            }
            return def;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Guid? ToGuidCanNull(this object _this, Guid? def = null)
        {
            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            Guid result = Guid.Empty;
            if (_this is String)
            {
                if (Guid.TryParse(_this.ToString(), out result))
                {
                    return result;
                }
            }
            else {
                return _this as Guid?;
            }
            return def;
        }

        /// <summary>
        /// 转换为DateTime 类型 失败 为NULL
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def">转换失败的默认值</param>
        /// <returns></returns>
        public static DateTime? ToDateTimeCanNull(this object _this, DateTime? def = null)
        {
            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            DateTime result;
            if (_this is String)
            {
                if (DateTime.TryParse(_this.ToString(), out result))
                {
                    return result;
                }
            }
            else {
                return _this as DateTime?;
            }
            return def;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="def">转换失败的默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object _this, DateTime def = default(DateTime))
        {

            if (_this == null || _this == DBNull.Value)
            {
                return def;
            }
            DateTime result = def;
            if (_this is String)
            {
                if (!DateTime.TryParse(_this.ToString(), out result))
                {
                    return def;
                }
            }
            else
            {
                if (_this is Int64)
                {
                    DateTime dateTimeStart = new DateTime(1970, 1, 1);
                    TimeSpan toNow = new TimeSpan((long)_this);
                    return dateTimeStart.Add(toNow).ToLocalTime();
                }
                return (DateTime)_this;
            }
            return result;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static long GetTime(this DateTime _this)
        {
            TimeSpan time = _this.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return time.Ticks;
        }

        /// <summary>
        /// 指定separator 分隔
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="separator">分隔符</param>
        /// <param name="def">转换失败的默认值</param>
        /// <returns></returns>
        public static List<string> ToListData(this string _this, char separator = ',', List<string> def = null)
        {
            if (string.IsNullOrEmpty(_this))
            {
                return def;
            }
            var result = _this.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return result;
        }

        /// <summary>
        /// DateTime 转换为UInt64
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this DateTime _this)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var time = (UInt64)(_this - startTime).TotalMilliseconds;
            return time;
        }

        /// <summary>
        /// 格式化附件大小 获取文件Size的单位描述
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string GetFileSizeDesc(this long _this)
        {
            if (_this == 0)
            {
                return "0KB";
            }
            var kb = _this / 1024;
            if (kb < 1024)
            {
                if (kb == (int)kb)
                {
                    return string.Format("{0}KB", kb);
                }
                else
                {
                    return string.Format("{0:.00}KB", kb);
                }
            }
            var mb = kb / 1024;
            if (mb == (int)mb)
            {
                return string.Format("{0}MB", mb);
            }
            else
            {
                return string.Format("{0:.00}MB", mb);
            }
        }

        /// <summary>
        /// 将字符串转为utf-8
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static byte[] ToUtfEight(this string _this)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(_this);
            return encodedBytes;
        }

        /// <summary>
        /// byte数组转成16进制字符串
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2 } ", InByte);
            }
            return StringOut;
        }

        /// <summary>
        /// 16进制字符串转换成数组
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static byte[] ToByteFromHex(this string InString)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(" ".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length - 1];
            for (int i = 0; i == ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(("0x" + ByteStrings[i]));
            }
            return ByteOut;
        }


    }
}
