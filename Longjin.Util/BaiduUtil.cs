using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Longjin.Util.BaiduUtil
{
    /// <summary>
    /// 坐标
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public float lat;

        /// <summary>
        /// 经度
        /// </summary>
        public float lng;

        /// <summary>
        /// 
        /// </summary>
        public Location() { }
    }


    /// <summary>
    /// 坐标结果详细信息
    /// </summary>
    public class PlaceResultDetailInfo
    {
        public int distance;
        public string type;
        public string tag;
        public string detail_url;
        public string price;
        public string shop_hours;
        public string overall_rating;
        public string taste_rating;
        public string service_rating;
        public string environment_rating;
        public string facility_rating;
        public string hygiene_rating;
        public string technology_rating;
        public string image_num;
        public int groupon_num;
        public int discount_num;
        public string comment_num;
        public string favorite_num;
        public string checkin_num;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PlaceResult
    {
        /// <summary>
        /// poi名称
        /// </summary>
        public string name;

        /// <summary>
        /// poi经纬度坐标 
        /// </summary>
        public Location location;

        /// <summary>
        /// poi地址信息 
        /// </summary>
        public string address;

        /// <summary>
        /// 
        /// </summary>
        public string street_id;

        /// <summary>
        /// poi电话信息 
        /// </summary>
        public string telephone;

        /// <summary>
        /// 
        /// </summary>
        public int detail;

        /// <summary>
        /// poi的唯一标示 
        /// </summary>
        public string uid;

        /// <summary>
        /// poi的扩展信息
        /// </summary>
        public PlaceResultDetailInfo detail_info;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PlaceSearchResult
    {
        public int status;
        public string message;
        public List<PlaceResult> results;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PlaceDetailResult
    {
        public int status;
        public string message;
        public PlaceResult result;
    }
    /// <summary>
    /// 
    /// </summary>
    public class PlaceDetailResults
    {
        public int status;
        public string message;
        public List<PlaceResult> result;
    }

    /// <summary>
    /// Place API 是一类简单的HTTP接口，用于返回查询某个区域的某类POI数据，且提供单个POI的详情查询服务，用户可以使用C#、C++、Java等开发语言发送HTTP请求且接收json、xml的数据。 
    /// </summary>
    public class Place
    {
        #region Search Place区域检索POI服务
        /// <summary>
        ///  Place API根据支持检索的区域类型，提供城市内方法
        /// </summary>
        /// <param name="region">城市，必填项</param>
        /// <param name="query">检索关键字，支持多个关键字并集检索，不同关键字间以$符号分隔，最多支持10个关键字检索，必填项</param>
        /// <param name="tag">标签项，与q组合进行检索，以“,”分隔</param>
        /// <param name="output">json或xml，选填项</param>
        /// <param name="scope">检索结果详细程度：1-基本信息 2-详细信息；取值为1 或空，则返回基本信息；取值为2，返回检索POI详细信息 ，必填项</param>
        /// <param name="filter">检索过滤条件，选填项</param>
        /// <param name="page_size">范围记录数量，默认为10条记录，最大返回20条。多关键字检索时，返回的记录数为关键字个数*page_size，选填项</param>
        /// <param name="page_num">分页页码，默认为0；0代表第一页，1代表第二页，以此类推，选填项 </param>
        /// <param name="ak">用户的访问密钥，必填项。</param>
        /// <param name="sn">用户的权限签名，选填项</param>
        /// <param name="timestamp">设置sn后该值必填。</param>
        /// <returns></returns>
        public static PlaceSearchResult SearchByRegion(string region, string query, string tag, string output, int scope, string filter, int page_size, int page_num, string ak, string sn, string timestamp)
        {
            if (string.IsNullOrEmpty(region)) return null;

            if (string.IsNullOrEmpty(query)) return null;

            if (string.IsNullOrEmpty(ak)) return null;

            region = HttpUtility.UrlEncode(region);

            query = HttpUtility.UrlEncode(query);

            string url = string.Format("http://api.map.baidu.com/place/v2/search?query={0}&region={1}&ak={2}", query, region, ak);

            if (!string.IsNullOrEmpty(tag)) url += "&tag=" + tag;
            if (!string.IsNullOrEmpty(output)) url += "&output=" + output;
            url += "&scope=" + scope;
            if (!string.IsNullOrEmpty(filter)) url += "&filter=" + filter;
            url += "&page_size=" + page_size;
            url += "&page_num=" + page_num;
            if (!string.IsNullOrEmpty(sn)) url += "&sn=" + sn;
            if (!string.IsNullOrEmpty(timestamp)) url += "&timestamp=" + timestamp;

            string _html = HttpUtil.GetHtml(url);

            PlaceSearchResult _result = null;
            _result = JsonConvert.DeserializeObject<PlaceSearchResult>(_html);
            return _result;
        }


        /// <summary>
        ///  Place API根据支持检索的区域类型，提供城市内方法
        /// </summary>
        /// <param name="left">区域左上角，必填项</param>
        /// <param name="right">区域右下角，必填项</param>
        /// <param name="query">检索关键字，支持多个关键字并集检索，不同关键字间以$符号分隔，最多支持10个关键字检索，必填项</param>
        /// <param name="tag">标签项，与q组合进行检索，以“,”分隔</param>
        /// <param name="output">json或xml，选填项</param>
        /// <param name="scope">检索结果详细程度：1-基本信息 2-详细信息；取值为1 或空，则返回基本信息；取值为2，返回检索POI详细信息 ，必填项</param>
        /// <param name="filter">检索过滤条件，选填项</param>
        /// <param name="page_size">范围记录数量，默认为10条记录，最大返回20条。多关键字检索时，返回的记录数为关键字个数*page_size，选填项</param>
        /// <param name="page_num">分页页码，默认为0；0代表第一页，1代表第二页，以此类推，选填项 </param>
        /// <param name="ak">用户的访问密钥，必填项。</param>
        /// <param name="sn">用户的权限签名，选填项</param>
        /// <param name="timestamp">设置sn后该值必填。</param>
        /// <returns></returns>
        public static PlaceSearchResult SearchByBounds(Location left, Location right, string query, string tag, string output, int scope, string filter, int page_size, int page_num, string ak, string sn, string timestamp)
        {
            if (left == null) return null;

            if (right == null) return null;

            if (string.IsNullOrEmpty(query)) return null;

            if (string.IsNullOrEmpty(ak)) return null;

            query = HttpUtility.UrlEncode(query);

            string url = string.Format("http://api.map.baidu.com/place/v2/search?bounds={0},{1},{2},{3}&query={4}&ak={5}", left.lat, left.lng, right.lat, right.lng, query, ak);

            if (!string.IsNullOrEmpty(tag)) url += "&tag=" + tag;
            if (!string.IsNullOrEmpty(output)) url += "&output=" + output;
            url += "&scope=" + scope;
            if (!string.IsNullOrEmpty(filter)) url += "&filter=" + filter;
            url += "&page_size=" + page_size;
            url += "&page_num=" + page_num;
            if (!string.IsNullOrEmpty(sn)) url += "&sn=" + sn;
            if (!string.IsNullOrEmpty(timestamp)) url += "&timestamp=" + timestamp;

            string _html = HttpUtil.GetHtml(url);

            PlaceSearchResult _result = null;
            _result = JsonConvert.DeserializeObject<PlaceSearchResult>(_html);
            return _result;
        }

        /// <summary>
        ///  Place API根据支持检索的区域类型，提供圆形区域检索方法
        /// </summary>
        /// <param name="location">区域，必填项</param>
        /// <param name="radius">区域，必填项</param>
        /// <param name="query">检索关键字，支持多个关键字并集检索，不同关键字间以$符号分隔，最多支持10个关键字检索，必填项</param>
        /// <param name="tag">标签项，与q组合进行检索，以“,”分隔</param>
        /// <param name="output">json或xml，选填项</param>
        /// <param name="scope">检索结果详细程度：1-基本信息 2-详细信息；取值为1 或空，则返回基本信息；取值为2，返回检索POI详细信息 ，必填项</param>
        /// <param name="filter">检索过滤条件，选填项</param>
        /// <param name="page_size">范围记录数量，默认为10条记录，最大返回20条。多关键字检索时，返回的记录数为关键字个数*page_size，选填项</param>
        /// <param name="page_num">分页页码，默认为0；0代表第一页，1代表第二页，以此类推，选填项 </param>
        /// <param name="ak">用户的访问密钥，必填项。</param>
        /// <param name="sn">用户的权限签名，选填项</param>
        /// <param name="timestamp">设置sn后该值必填。</param>
        /// <returns></returns>
        public static PlaceSearchResult SearchByRadius(Location location, int radius, string query, string tag, string output, int scope, string filter, int page_size, int page_num, string ak, string sn, string timestamp)
        {
            if (location == null) return null;

            if (string.IsNullOrEmpty(query)) return null;

            if (string.IsNullOrEmpty(ak)) return null;

            query = HttpUtility.UrlEncode(query);

            string url = string.Format("http://api.map.baidu.com/place/v2/search?location={0},{1}&radius={2}&query={3}&ak={4}", location.lat, location.lng, radius, query, ak);

            if (!string.IsNullOrEmpty(tag)) url += "&tag=" + tag;
            if (!string.IsNullOrEmpty(output)) url += "&output=" + output;
            url += "&scope=" + scope;
            if (!string.IsNullOrEmpty(filter)) url += "&filter=" + filter;
            url += "&page_size=" + page_size;
            url += "&page_num=" + page_num;
            if (!string.IsNullOrEmpty(sn)) url += "&sn=" + sn;
            if (!string.IsNullOrEmpty(timestamp)) url += "&timestamp=" + timestamp;

            string _html = HttpUtil.GetHtml(url);

            PlaceSearchResult _result = null;
            _result = JsonConvert.DeserializeObject<PlaceSearchResult>(_html);
            return _result;
        }
        #endregion

        #region Detail Place详情检索服务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="output"></param>
        /// <param name="scope"></param>
        /// <param name="ak"></param>
        /// <param name="sn"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static PlaceDetailResult DetailByUid(string uid, string output, int scope, string ak, string sn, string timestamp)
        {
            if (string.IsNullOrEmpty(uid)) return null;

            string url = string.Format("http://api.map.baidu.com/place/v2/detail?uid={0}&ak={1}", uid, ak);
            if (!string.IsNullOrEmpty(output)) url += "&output=" + output;
            url += "&scope=" + scope;
            if (!string.IsNullOrEmpty(sn)) url += "&sn=" + sn;
            if (!string.IsNullOrEmpty(timestamp)) url += "&timestamp=" + timestamp;

            string _html = HttpUtil.GetHtml(url);

            PlaceDetailResult _result = null;
            _result = JsonConvert.DeserializeObject<PlaceDetailResult>(_html);
            return _result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uids">uid的集合，最多可以传入10个uid，多个uid之间用英文逗号分隔。 </param>
        /// <param name="output"></param>
        /// <param name="scope"></param>
        /// <param name="ak"></param>
        /// <param name="sn"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static PlaceDetailResults DetailByUids(string uids, string output, int scope, string ak, string sn, string timestamp)
        {
            if (string.IsNullOrEmpty(uids)) return null;

            string url = string.Format("http://api.map.baidu.com/place/v2/detail?uids={0}&ak={1}", uids, ak);
            if (!string.IsNullOrEmpty(output)) url += "&output=" + output;
            url += "&scope=" + scope;
            if (!string.IsNullOrEmpty(sn)) url += "&sn=" + sn;
            if (!string.IsNullOrEmpty(timestamp)) url += "&timestamp=" + timestamp;

            string _html = HttpUtil.GetHtml(url);

            PlaceDetailResults _result = null;
            _result = JsonConvert.DeserializeObject<PlaceDetailResults>(_html);
            return _result;
        }
        #endregion

        #region eventsearch 团购信息检索服务
        #endregion
    }

    #region  地理编码结果
    /// <summary>
    /// 地理编码结果
    /// </summary>
    public class GeocodingAddressResultJson
    {
        /// <summary>
        /// 返回结果状态值， 成功返回0
        /// </summary>
        public int status;

        /// <summary>
        /// 
        /// </summary>
        public GeocodingAddressResult result;

    }
    /// <summary>
    /// 地理编码结果
    /// </summary>
    public class GeocodingAddressResult
    {

        /// <summary>
        /// 经纬度坐标
        /// </summary>
        public Location location;

        /// <summary>
        /// 位置的附加信息，是否精确查找。1为精确查找，0为不精确。
        /// </summary>
        public int precise;

        /// <summary>
        /// 可信度
        /// </summary>
        public int confidence;

        /// <summary>
        /// 地址类型
        /// </summary>
        public string level;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class GeocodingAddressComponent
    {
        public string country;
        public string province;
        public string city;
        public string district;
        public string street;
        public string street_number;
        public string country_code;
        public string direction;
        public string distance;
    }

    /// <summary>
    /// 
    /// </summary>
    public class GeocodingPoint
    {
        public float x;
        public float y;
    }

    /// <summary>
    /// 
    /// </summary>
    public class GeocodingPOI
    {
        public string addr;
        public string cp;
        public string direction;
        public string distance;
        public string name;
        public string poiType;
        public GeocodingPoint point;
        public string tel;
        public string uid;
        public string zip;
    }

    /// <summary>
    /// 
    /// </summary>
    public class GeocodingLocationResultJson
    {
        public int status;
        public GeocodingLocationResult result;
    }

    #region 
    /// <summary>
    /// 
    /// </summary>
    public class GeocodingLocationResult
    {
        public Location location;
        public string formatted_address;
        public string business;
        public GeocodingAddressComponent addressComponent;
        public string sematic_description;
        public List<GeocodingPOI> pois;
    }
    #endregion

    /// <summary>
    /// Geocoding API 是一类简单的HTTP接口，用于提供从地址到经纬度坐标或者从经纬度坐标到地址的转换服务，用户可以使用C# 、C++、Java等开发语言发送HTTP请求且接收JSON、XML的返回数据。 
    /// </summary>
    public class Geocoding
    {
        /// <summary>
        /// 地理编码 门牌地址转baidu坐标
        /// </summary>
        /// <param name="address">地址，必填</param>
        /// <param name="city">城市，选填</param>
        /// <param name="ak">秘钥，选填</param>
        public static GeocodingAddressResultJson AddressToLocation(string address, string city, string ak)
        {
            if (string.IsNullOrEmpty(address)) return null;
            if (string.IsNullOrEmpty(city)) return null;
            if (string.IsNullOrEmpty(ak)) return null;

            address = HttpUtility.UrlEncode(address);

            city = HttpUtility.UrlEncode(city);

            string url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=showLocation&output=json&address={0}&city={1} ", address, city, ak);

            string _html = HttpUtil.GetHtml(url);

            _html = _html.Replace("showLocation&&showLocation(", "").Replace(")", "");

            GeocodingAddressResultJson _result = null;
            _result = JsonConvert.DeserializeObject<GeocodingAddressResultJson>(_html);
            return _result;
        }

        /// <summary>
        /// 逆地理编码，GPS坐标转地址
        /// </summary>
        /// <param name="lat">维度</param>
        /// <param name="lng">经度</param>
        /// <param name="show">0-不显示周边信息 1-显示周边信息</param>
        /// <param name="ak">秘钥，选填</param>
        public static GeocodingLocationResultJson LocationToAddress(string lat, string lng, int show, string ak)
        {
            if (string.IsNullOrEmpty(lat)) return null;
            if (string.IsNullOrEmpty(lng)) return null;
            if (string.IsNullOrEmpty(ak)) return null;

            string url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=renderReverse&location={0},{1}&coordtype=wgs84ll&output=json&pois=1", lat, lng, ak);

            string _html = HttpUtil.GetHtml(url);

            _html = _html.Replace("renderReverse&&renderReverse(", "").Replace(")", "");

            GeocodingLocationResultJson _result = null;
            _result = JsonConvert.DeserializeObject<GeocodingLocationResultJson>(_html);
            return _result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1-百度经纬度坐标 2-国测局经纬度坐标 3-GPS经纬度</param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="show"></param>
        /// <param name="ak"></param>
        /// <returns></returns>
        public static GeocodingLocationResultJson LocationToAddress(int type, string lat, string lng, int show, string ak)
        {
            if (string.IsNullOrEmpty(lat)) return null;
            if (string.IsNullOrEmpty(lng)) return null;
            if (string.IsNullOrEmpty(ak)) return null;

            string url = "";
            if (type == 1)
                url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=renderReverse&location={0},{1}&coordtype=bd09ll&output=json&pois=1", lat, lng, ak);
            else if(type==2)
                url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=renderReverse&location={0},{1}&coordtype=gcj02ll&output=json&pois=1", lat, lng, ak);
            else if(type==3)
                url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=renderReverse&location={0},{1}&coordtype=wgs84ll&output=json&pois=1", lat, lng, ak);
            else
                url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={2}&callback=renderReverse&location={0},{1}&coordtype=bd09ll&output=json&pois=1", lat, lng, ak);

            string _html = HttpUtil.GetHtml(url);

            _html = _html.Replace("renderReverse&&renderReverse(", "").Replace(")", "");

            GeocodingLocationResultJson _result = null;
            _result = JsonConvert.DeserializeObject<GeocodingLocationResultJson>(_html);
            return _result;
        }
    }



    public class GeoConvResultJson
    {
        public int status;
        public List<GeocodingPoint> result;
    }

    public class GeoConv
    {
        #region 坐标转换API

        /// <summary>
        /// 将非百度坐标转换成百度坐标
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="ak"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static GeoConvResultJson Convert(string lng, string lat, string ak, int from, int to)
        {
            if (string.IsNullOrEmpty(lng)) return null;
            if (string.IsNullOrEmpty(lat)) return null;
            if (string.IsNullOrEmpty(ak)) return null;

            string url = string.Format("http://api.map.baidu.com/geoconv/v1/?coords={0},{1}&from={2}&to={3}&ak={4}", lng, lat, from, to, ak);

            string _html = HttpUtil.GetHtml(url);

            GeoConvResultJson _result = null;
            _result = JsonConvert.DeserializeObject<GeoConvResultJson>(_html);
            return _result;
        }
        #endregion

        /// <summary>
        /// 百度坐标转GSP坐标
        /// </summary>
        /// <param name="lng">百度经度</param>
        /// <param name="lat">百度维度</param>
        /// <param name="ak">百度秘钥</param>
        /// <returns></returns>
        public static Location ConvertToGPS(string lng, string lat, string ak)
        {
            Location result = new Location();

            GeoConvResultJson _json = Convert(lng, lat, ak, 1, 5);

            if (_json != null)
            {
                float x1 = float.Parse(lng);
                float y1 = float.Parse(lat);

                float x2 = _json.result[0].x;
                float y2 = _json.result[0].y;

                float x = 2 * x1 - x2;
                float y = 2 * y1 - y2;

                result.lat = y;
                result.lng = x;

            }

            return result;
        }


    }

    public class Geo
    {
        /// <summary>
        /// latitude
        /// </summary>
        private string _latitude = "";

        /// <summary>
        /// longtitude
        /// </summary>
        private string _longtitude = "";

        /// <summary>
        /// get latitude(纬度)
        /// </summary>
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// get longtitude(经度)
        /// </summary>
        public string Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }
         public Geo(string address) {
             WebClient client = new WebClient();//webclient客户端对象
             string url = "http://api.map.baidu.com/geocoder/v2/?address=" + address + "&output=json&ak=4DKNgUoGZhnKScxcH8t6wMQ6&callback=showLocation";
             client.Encoding = Encoding.UTF8;//编码格式 
             string responseTest = client.DownloadString(url);//下载xml响应数据  
             responseTest = responseTest.Replace("showLocation&&showLocation(", "").Replace(")", "");
             JsonCity jsonC = (JsonCity)JsonConvert.DeserializeObject(responseTest, typeof(JsonCity));
             if (jsonC == null || jsonC.status == null)
             {
                 _longtitude = "";
                 _latitude = "";
                 return;
             }
             _longtitude = jsonC.result.location.lng.ToString();
             _latitude = jsonC.result.location.lat.ToString();

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
      

    }

}