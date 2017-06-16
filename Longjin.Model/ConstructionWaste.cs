using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

namespace Longjin.Model
{
    [Persistent("LJ_MAP_ConstructionWaste")]
    public class ConstructionWaste : XPCustomObject
    {
        public ConstructionWaste(Session session) : base(session) { }
        [Key(AutoGenerate = true)]
        public int Id;

        /// <summary>
        /// 企业名
        /// </summary>
        public string EnterpriseName;

        public string Address;
        /// <summary>
        /// 法人
        /// </summary>
        public string LegalPerson;

        /// <summary>
        /// 法人联系电话
        /// </summary>
        public string LegalPersonPhone;

        /// <summary>
        /// 是否专营单位
        /// </summary>
        public string IfMajor;

        /// <summary>
        /// 是否网上备案
        /// </summary>
        public string IfOnlineRecord;

        /// <summary>
        /// 车牌
        /// </summary>
        public string CarNo;

        /// <summary>
        /// 驾驶员
        /// </summary>
        public string Driver;


        /// <summary>
        /// 联系方式
        /// </summary>
        public string DriverPhone;

        /// <summary>
        /// 车辆吨位
        /// </summary>
        public string VehicleTonnage;

        /// <summary>
        /// 是否有gps
        /// </summary>
        public string IfGps;

        /// <summary>
        /// 顶灯表示名称
        /// </summary>
        public string TopLightName;
        /// <summary>
        /// 密闭结构 软，硬
        /// </summary>
        public string Structure;

        /// <summary>
        /// 检查情况
        /// </summary>
        public string CheckSitutation;


        public string BaiduLat;

        public string BaiduLong;

        public string Longitude;

        public string Latitude;
        /// <summary>
        /// 高德坐标
        /// </summary>
        public string GaodeLat;
        /// <summary>
        /// 高德经度
        /// </summary>
        public string GaodeLong;

    }
}
