using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;


namespace Longjin.Model
{
    /// <summary>
    /// 车队
    /// </summary>
    [Persistent("LJ_MAP_TruckGroup")]
    public class TruckGroup : XPCustomObject
    {
        public TruckGroup(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        public int Id;

        /// <summary>
        /// 车队名称
        /// </summary>
        public string Name;


        public string Address;

        /// <summary>
        /// 负责人
        /// </summary>
        public string ResponsiblePerson;

        public string Phone;

        /// <summary>
        /// 是否专营
        /// </summary>
        public string MajorBussiness;

        /// <summary>
        /// 是否网上备案
        /// </summary>
        public string OnlineRecord;

        /// <summary>
        /// 总车辆数
        /// </summary>
        public int TotalAmount;
        /// <summary>
        /// 新车数
        /// </summary>
        public int NewAmount;
        /// <summary>
        /// 旧车数
        /// </summary>
        public int OldAmount;

        /// <summary>
        /// 吨位
        /// </summary>
        public string Ton;

        /// <summary>
        /// 顶灯标识名称
        /// </summary>
        public string TopLightName;

        /// <summary>
        /// 是否有gps
        /// </summary>
        public bool GpsBolen;

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

        public string PointType
        {
            get
            {
                return this.Ton.ToNumber<double>() > 15 ? "blueCircle" : "yellowCircle";
            }
        }

        public object Clone()
        {

            return new
            {
                Id = this.Id,
                GaodeLat = this.GaodeLat,
                GaodeLong = this.GaodeLong,
                Name = this.Name ?? "",
                Address = this.Address ?? "",
                ResponsiblePerson = this.ResponsiblePerson ?? "",
                Phone = this.Phone ?? "",
                MajorBussiness = this.MajorBussiness ?? "",
                OnlineRecord = this.OnlineRecord ?? "",
                Total = this.TotalAmount,
                New = this.NewAmount,
                PointType = PointType,
                Old = this.OldAmount,
                Ton = this.Ton ?? "",
                Gps = this.GpsBolen?"有":"无",
                Class="TruckGroup" ,
                ClassName = "车队"
            };
        }
    }
}
