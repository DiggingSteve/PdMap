using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MapUtil
{
    /// <summary>
    /// dzHelper 的摘要说明
    /// </summary>
    public class BaiduCoordinateUtil
    {
        public BaiduCoordinateUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取gps
        /// </summary>
        /// <param name="dz">地址</param>
        ///<param name="blat">返回的百度坐标x</param>
        ///<param name="blng">返回的百度坐标y</param>
        /// <returns></returns>
        public static void GetCityInfo(string dz, out string blng, out string blat)
        {
            try
            {
                WebClient client = new WebClient();//webclient客户端对象
                string url = "http://api.map.baidu.com/geocoder/v2/?address= " + dz + "&output=json&ak=4DKNgUoGZhnKScxcH8t6wMQ6&callback=showLocation";
                client.Encoding = Encoding.UTF8;//编码格式 
                string responseTest = client.DownloadString(url);//下载xml响应数据  
                responseTest = responseTest.Replace("showLocation&&showLocation(", "").Replace(")", "");
                JsonCity jsonC = (JsonCity)JsonConvert.DeserializeObject(responseTest, typeof(JsonCity));
                if (jsonC == null || jsonC.status == null)
                {
                    blng = "";
                    blat = "";
                    return;
                }
                blng = jsonC.result.location.lng;
                blat = jsonC.result.location.lat;
            }
            catch (Exception ex)
            {
                blng = "";
                blat = "";
                return;
            }
        }
        /// <summary>
        /// 获取gps
        /// 返回内容lng:lat
        /// </summary>
        /// <param name="dz">输入得地址</param>
        /// <param name="blat">百度地图Y轴坐标</param>
        /// <param name="blng">百度地图X轴坐标</param>
        /// <param name="glat">Gps经度坐标</param>
        /// <param name="glng">Gps纬度坐标</param>
        /// <returns></returns>
        public static void GetCoord(string dz, out string blng, out string blat, out string glng, out string glat)
        {
            try
            {
                GetCityInfo(dz, out blng, out blat);

                WebClient client = new WebClient();//webclient客户端对象
                string url = "http://api.map.baidu.com/geoconv/v1/?coords=" + blng + "," + blat + "&ak=CGdCkuzlgfQY1refO8QxoYGd&output=json";
                client.Encoding = Encoding.UTF8;//编码格式 
                string responseTest = client.DownloadString(url);//下载xml响应数据  
                JsonGpx jsonGs = (JsonGpx)JsonConvert.DeserializeObject(responseTest, typeof(JsonGpx));

                List<LocationGps> results = (List<LocationGps>)jsonGs.result;
                if (results == null)
                {
                    glng = "";
                    glat = "";
                    return;
                }
                LocationGps jsonG = results[0];
                string blng2 = jsonG.x;
                string blat2 = jsonG.y;

                MathGps(blng, blat, blng2, blat2, out glng, out glat);

                return;

            }
            catch (Exception ex)
            {
                blng = "";
                blat = "";
                glng = "";
                glat = "";
                return;
            }
        }
        /// <summary>
        /// 将坐标转换为gps坐标计算
        /// </summary>
        /// <param name="x1">百度原始坐标x</param>
        /// <param name="y1">百度原始坐标y</param>
        /// <param name="x2">百度转换后坐标x</param>
        /// <param name="y2">百度转换后坐标x</param>
        /// <param name="glng">输出计算后gps坐标经度</param>
        /// <param name="glat">输出计算后gps坐标纬度</param>
        public static void MathGps(string x1, string y1, string x2, string y2, out string glng, out string glat)
        {
            //x = 2*x1-x2，y = 2*y1-y2
            float mathglng = 2 * float.Parse(x1) - float.Parse(x2);
            float mathglat = 2 * float.Parse(y1) - float.Parse(y2);
            glng = mathglng.ToString();
            glat = mathglat.ToString();
        }


    }
    public class JsonGpx
    {
        private string _status;
        public string status
        {
            set { _status = value; }
            get { return _status; }
        }
        private List<LocationGps> _result;
        public List<LocationGps> result
        {
            set { _result = value; }
            get { return _result; }
        }

    }
    public class LocationGps
    {
        private string _x;
        public string x
        {
            set { _x = value; }
            get { return _x; }
        }
        private string _y;
        public string y
        {
            set { _y = value; }
            get { return _y; }
        }
    }
    public class JsonCity
    {
        private string _status;
        public string status
        {
            set { _status = value; }
            get { return _status; }
        }
        private Result _result;
        public Result result
        {
            set { _result = value; }
            get { return _result; }
        }

    }
    public class Result
    {
        private Location _location;
        public Location location
        {
            set { _location = value; }
            get { return _location; }
        }
        private string _formatted_address;
        public string formatted_address
        {
            set { _formatted_address = value; }
            get { return _formatted_address; }
        }
        private AddressComponent _addressComponent;
        public AddressComponent addressComponent
        {
            set { _addressComponent = value; }
            get { return _addressComponent; }
        }

    }
    public class Location
    {
        private string _lng;
        public string lng
        {
            set { _lng = value; }
            get { return _lng; }
        }
        private string _lat;
        public string lat
        {
            set { _lat = value; }
            get { return _lat; }
        }
    }
    public class AddressComponent
    {
        private string _city;
        public string city
        {
            set { _city = value; }
            get { return _city; }
        }
        private string _district;
        public string district
        {
            set { _district = value; }
            get { return _district; }
        }
        private string _province;
        public string province
        {
            set { _province = value; }
            get { return _province; }
        }
        private string _street;
        public string street
        {
            set { _street = value; }
            get { return _street; }
        }
        private string _street_number;
        public string street_number
        {
            set { _street_number = value; }
            get { return _street_number; }
        }

    }
}