using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapUtil
{
    /// <summary>
    /// 高德百度坐标互转类
    /// </summary>
    public class BaiduGdConverter
    {
        /// <summary>
        /// 高德转百度
        /// </summary>
        /// <param name="gdLong"></param>
        /// <param name="gdLat"></param>
        /// <returns></returns>
        public static double[] GaodeToBaidu(double gdLong, double gdLat)
        {
            double[] bdCoornidate = new double[2];
            double PI = 3.14159265358979324 * 3000.0 / 180.0;
            double x = gdLong, y = gdLat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * PI);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * PI);
            bdCoornidate[0] = z * Math.Cos(theta) + 0.0065;
            bdCoornidate[1] = z * Math.Sin(theta) + 0.006;
            return bdCoornidate;
        }

        /// <summary>
        /// 百度转高德
        /// </summary>
        /// <param name="bdLat"></param>
        /// <param name="bdLong"></param>
        /// <returns>数组第一个经度，第二个纬度</returns>
        public static double[] BaiduToGaode(double bdLat, double bdLong)
        {
            double[] gdCoornidate = new double[2];
            double PI = 3.14159265358979324 * 3000.0 / 180.0;
            double x = bdLong - 0.0065, y = bdLat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * PI);
            gdCoornidate[0] = z * Math.Cos(theta);
            gdCoornidate[1] = z * Math.Sin(theta);
            return gdCoornidate;
        }
    }
}
