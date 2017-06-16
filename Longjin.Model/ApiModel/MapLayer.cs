using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Longjin.Model.ApiModel
{
    #region 地图类
    public class StreetLayer
    {
        public StreetLayer()
        {

        }
        public string Guid { get; set; }
        /// <summary>
        /// 街道名
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// 父街道或区
        /// </summary>
        public string ParentStreet { get; set; }
        /// <summary>
        /// 坐标数组
        /// </summary>
        public string Polygon { get; set; }
        /// <summary>
        ///  0--全区，1--街道
        /// </summary>
        public int AreaType { get; set; }
    }

    /// <summary>
    ///一个map对应一个图层所需要的数据
    /// </summary>
    public class MapLayer
    {            //构造函数
        public MapLayer(string key,string layerType)
        {
            this.Points = new List<object>();
            Key = key;
            LayerType = layerType;
        }
        /// <summary>
        /// 图层唯一键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        ///图层父键
        /// </summary>
        public string ParentKey { get; set; }
        /// <summary>
        /// 父图层
        /// </summary>
        public MapLayer ParentLayer { get; set; }
        /// <summary>
        /// 子图层
        /// </summary>
        public List<MapLayer> ChildrenLayers { get; set; }
        /// <summary>
        /// 图层的类型
        /// </summary>
        public string LayerType { get; set; }
        public List<object> Points { get; set; }

    }

    public class Point
    {
        public string Id { get; set; }
        public string Name { get; set; }     //点图
        /// <summary>
        ///     来源自那个model用来查询
        /// </summary>
        public string ClassSource { get; set; }
        public string Type { get; set; }      //点图类型
        public string ArchivesNo;
        public double BaiduLat { get; set; }

        public double BaiduLong { get; set; }

        public double GaoDeLat;
        public double GaoDeLong;
    }
    class Surface
    {
        public Surface()
        {
            this.Points = new List<Point>();
        }
        public string Id { get; set; }

        public string Name { get; set; }

        List<Point> Points { get; set; }
    }

    #endregion
}
