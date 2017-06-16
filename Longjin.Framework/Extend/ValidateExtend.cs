using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 验证扩展
    /// </summary>
    public static class ValidateExtend
    {
        /// <summary>
        /// GUID
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsGuid(this string _this, string err = "非法的参数类型：GUID")
        {
            var value = Guid.Empty;
            if (Guid.TryParse(_this, out value))
            {
                return _this;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 检查是否是合法GUID
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsGuidValid(this string _this)
        {
            var value = Guid.Empty;
            return Guid.TryParse(_this, out value);
        }

        /// <summary>
        /// 时间
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsDateTime(this string _this, string err = "非法的参数类型：DateTime")
        {
            var value = default(DateTime);
            if (DateTime.TryParse(_this, out value))
            {
                return _this;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 时间
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static DateTime IsToDateTime(this string _this, string err = "非法的参数类型：DateTime")
        {
            var value = default(DateTime);
            if (DateTime.TryParse(_this, out value))
            {
                return value;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 身份证
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsIDCardNo(this string _this, string err = "非法的参数类型：身份证号码")
        {
            if (_this.Length == 18)
            {
                long n = 0;
                if (long.TryParse(_this.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(_this.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    throw new ParamException(err, _this);//数字验证
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(_this.Remove(2)) == -1)
                {
                    throw new ParamException(err, _this);//省份验证
                }
                string birth = _this.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    throw new ParamException(err, _this);//生日验证
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = _this.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != _this.Substring(17, 1).ToLower())
                {
                    throw new ParamException(err, _this);//校验码验证
                }
                return _this;
            }
            else if (_this.Length == 15)
            {
                long n = 0;
                if (long.TryParse(_this, out n) == false || n < Math.Pow(10, 14))
                {
                    throw new ParamException(err, _this);//数字验证
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(_this.Remove(2)) == -1)
                {
                    throw new ParamException(err, _this);//省份验证
                }
                string birth = _this.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    throw new ParamException(err, _this);//生日验证
                }
                return _this;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsEmail(this string _this, string err = "非法的参数类型：Email")
        {
            var reg = new Regex(@"([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$", RegexOptions.IgnoreCase);
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                throw new ParamException(err, _this);
            }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>

        public static string IsPhone(this string _this, string err = "非法的参数类型：Phone")
        {
            var reg = new Regex(@"^[1][23456789][0-9]{9}$");
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                throw new ParamException(err, _this);
            }
        }

        /// <summary>
        /// 是不是电话号码：包括手机号码+固定电话
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsTelephone(this string _this, string err = "非法的参数类型：电话号码")
        {
            var reg = new Regex(@"^[1][23456789][0-9]{9}$");
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                reg = new Regex(@"0\d{2,3}\d{5,9}|0\d{2,3}-\d{5,9}");
                if (reg.IsMatch(_this))
                {
                    return _this;
                }
                else
                {
                    throw new ParamException(err, _this);
                }
            }
        }

        /// <summary>
        /// 固定电话
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string IsLandline(this string _this, string err = "非法的参数类型：Landline")
        {
            var reg = new Regex(@"0\d{2,3}\d{5,9}|0\d{2,3}-\d{5,9}");
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                throw new ParamException(err, _this);
            }
        }

        /// <summary>
        /// 整形Int64
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static long IsToLong(this string _this, string err = "非法参数类型：long")
        {
            long value = 0;
            if (long.TryParse(_this, out value))
            {
                return value;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsLong(this string _this, string err = "非法参数类型：long")
        {
            long value = 0;
            if (long.TryParse(_this, out value))
            {
                return true;
            }
            if (string.IsNullOrEmpty(err))
            {
                return false;
            }

            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 整形Int32
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static int IsToInt(this string _this, string err = "非法参数类型：int")
        {
            var value = 0;
            if (int.TryParse(_this, out value))
            {
                return value;
            }
            throw new ParamException(err, _this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsInt(this string _this, string err = "非法参数类型：int")
        {
            var value = 0;
            if (int.TryParse(_this, out value))
            {
                return true;
            }
            if (string.IsNullOrEmpty(err))
            {
                return false;
            }

            throw new ParamException(err, _this);
        }

        public static byte IsToByte(this string _this, string err = "非法参数类型：byte")
        {
            byte value = 0;
            if (byte.TryParse(_this, out value))
            {
                return value;
            }
            throw new ParamException(err);
        }

        //var chinese = /^[0-9\u4e00-\u9faf]+$/;
        public static string IsContainChinese(this string _this, string err = "含有中文字符")
        {
            var reg = new Regex(@"[0-9\u4e00-\u9faf]+");
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                throw new ParamException(err, _this);
            }
        }

        //  var specialwords= /[~#^$@%&!*]/gi;
        public static string IsContainSpecialWords(this string _this, string err = "含有特殊字符")
        {
            var reg = new Regex(@" [~#^$@%&!*]+");
            if (reg.IsMatch(_this))
            {
                return _this;
            }
            else
            {
                throw new ParamException(err, _this);
            }
        }

        /// <summary>
        /// 检查指定文件 是不是图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImgExt(this string fileName)
        {
            return false;
        }
    }
}
