using System;
using DevExpress.Xpo;

namespace Longjin.Model
{
    /// <summary>
    /// 渣土车
    /// </summary>
    [Persistent("LJ_MAP_ConstructionWasteTruck")]
    public class ConstructionWasteTruck : XPCustomObject
    {
        public ConstructionWasteTruck(Session session) : base(session) { }

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
        /// 联系人
        /// </summary>
        public string ContactPerson;
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone;

        /// <summary>
        /// 办公面积
        /// </summary>
        public string OfficeArea;

        /// <summary>
        /// 员工数
        /// </summary>
        public int StaffCount;

        /// <summary>
        /// 驾驶员数
        /// </summary>
        public int DriverCount;

        /// <summary>
        /// 有培训合格证的驾驶员
        /// </summary>
        public int CertificatedDriverCount;
        /// <summary>
        /// 卡车数量
        /// </summary>
        public int TruckCount;

        /// <summary>
        /// 注册在网的数量
        /// </summary>
        public int TruckOnlineCount;

        /// <summary>
        /// 新型车辆
        /// </summary>
        public int NewTruckCount;

        /// <summary>
        /// 改装车
        /// </summary>
        public int RefitTruckCount;

        /// <summary>
        /// 洒水车
        /// </summary>
        public int WateringTruckCount;

        /// <summary>
        /// 挖机
        /// </summary>
        public int DiggingMachineCount;

        /// <summary>
        /// 叉车
        /// </summary>
        public int ForkliftCount;

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
                return "constructionWaste";//blueCircle,渣土车
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
                ContactPerson = this.ContactPerson ?? "",
                ContactPhone = this.ContactPhone ?? "",
                TruckCount = this.TruckCount,
                TruckOnlineCount = this.TruckOnlineCount,
                StaffCount = this.StaffCount,
                DriverCount = this.DriverCount,
                CertificatedDriverCount = this.CertificatedDriverCount,
               
                PointType = PointType,

                Class = "ConstructionWasteTruck",
                ClassName = "渣土车"
            };
        }

    }
}
