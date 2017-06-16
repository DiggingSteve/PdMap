using DevExpress.Xpo;
using System;

namespace Longjin.Model
{
    /// <summary>
    /// 浦东新区建筑垃圾和工程渣土卸点清单
    /// </summary>
    [Persistent("LJ_MAP_LandfillSite")]
    public class LandfillSite:XPCustomObject
    {

        public LandfillSite(Session session) : base(session) { }
        [Key(AutoGenerate =true)]
        public int Id;

        
        public string Name;

        /// <summary>
        /// 归属区域
        /// </summary>
        
        public string Region;

        
        public string Address;

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ExpiryDateBegin;

        public DateTime ExpiryDateEnd;

        /// <summary>
        ///剩余吨位
        /// </summary>
        public float SurplusTon;

        public string PointType { get { return "landfillWaste"; } }

        /// <summary>
        /// 回填种类
        /// </summary>
        public string LandfillType;

        public double BaiduLat;

        public double BaiduLong;

        public double Longitude;

        public double Latitude;
        /// <summary>
        /// 高德坐标
        /// </summary>
        public double GaodeLat;
        /// <summary>
        /// 高德经度
        /// </summary>
        public double GaodeLong;

        public object Clone()
        {

            return new
            {
                Id = this.Id,
                GaodeLat = this.GaodeLat,
                GaodeLong = this.GaodeLong,
                Region = this.Region ?? "",
                Name = this.Name ?? "",
                ExpiryDateBegin = this.ExpiryDateBegin.ToString("yyyy-MM-dd"),
                ExpiryDateEnd = this.ExpiryDateEnd.ToString("yyyy-MM-dd"),
                SurplusTon = this.SurplusTon,
                LandfillType = this.LandfillType ?? "",
                Address = this.Address ?? "",

                Class = "LandfillSite",
                ClassName = "渣土回填",
                PointType = this.PointType

            };
        }
    }
}
