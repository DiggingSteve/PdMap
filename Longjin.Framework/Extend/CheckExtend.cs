using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 检查类扩展
    /// </summary>
    public static class CheckExtend
    {
        /// <summary>
        /// 检查list t中是否有数据 有true  无false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool ExistsData<T>(this ICollection<T> _this)
        {
            if (_this == null || _this.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ExistsData<T>(this IEnumerable<T> _this)
        {
            if (_this == null || _this.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///  检查List<T>是否有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static void ExistsData<T>(this ICollection<T> _this, string err = "无数据")
        {
            if (_this == null || _this.Count() == 0)
            {
                throw new ParamException(err);
            }
        }

        /// <summary>
        /// 检查是否为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static T CheckNull<T>(this T _this, string err = "指定对象为NULL")
        {
            if (_this == null)
            {
                throw new ParamException(err);
            }
            if (_this.GetType().FullName == "System.String")
            {
                if (string.IsNullOrEmpty(_this.ToString()))
                {
                    throw new ParamException(err);
                }
            }
            return _this;
        }
    }
}
