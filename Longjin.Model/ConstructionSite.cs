using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

namespace Longjin.Model
{
    [Persistent("LJ_MAP_ConstructionSite")]
    public class ConstructionSite : XPCustomObject
    {
        public ConstructionSite(Session session) : base(session) { }

        [Key(AutoGenerate = true)]
        public int Key;

        /// <summary>
        /// 档案号
        /// </summary>
        public string ArchivesNo;

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectNo;

        /// <summary>
        /// 工地名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 监督区县
        /// </summary>
        public string District;

        public string Address;

        /// <summary>
        /// 建设单位
        /// </summary>
        public string BuildingCompany;

        /// <summary>
        /// 总包单位名称
        /// </summary>
        public string MajorCompany;

        /// <summary>
        /// 监理单位名称
        /// </summary>
        public string SuperviseCompany;

        /// <summary>
        /// 建筑面积
        /// </summary>
        public string AreaAmount;

        /// <summary>
        /// 造价
        /// </summary>
        public double Expense;
        /// <summary>
        /// 项目经理
        /// </summary>
        public string ProjectManager;

        public string Phone;

        /// <summary>
        /// 运输单位
        /// </summary>
        public string TransportCompany;

        /// <summary>
        /// 渣土来去地点
        /// </summary>
        public string Destination;
        /// <summary>
        /// 完成情况
        /// </summary>
        public string CompleteStatus;
        /// <summary>
        /// 工程状态名
        /// </summary>
        public string StatusName
        {
            get
            {
                string[] name = new string[] { "开工", "开工", "停工", "未验" ,"进出土"};       //提前与正常合并为开工
                return name[Status];
            }
        }

        /// <summary>
        /// 分组情况
        /// </summary>
        public string Grouping;

        /// <summary>
        /// 出土情况
        /// </summary>
        public string TransportSoil;

        /// <summary>
        /// 渣土许可证
        /// </summary>
        public string License;

        /// <summary>
        /// 备注
        /// </summary>
        public string Comments;

        /// <summary>
        /// 施工许可证
        /// </summary>
        public string BuildingLicence;

        /// <summary>
        /// 工程状态0-正常,1-提前,2-停工,3-未验 ,4-进出土,0和1合并为开工
        /// </summary>
        public int Status;

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
        public string PointType { get { return this.StatusName ?? ""; } }
        public object Clone()
        {

            return new
            {
                Id = this.Key,
                GaodeLat = this.GaodeLat,
                GaodeLong = this.GaodeLong,
                ArchivesNo = this.ArchivesNo ?? "",
                Name = this.Name ?? "",
                District = this.District ?? "",
                Address = this.Address ?? "",
                BuildingCompany = this.BuildingCompany ?? "",
                MajorCompany = this.MajorCompany ?? "",
                ProjectManager = this.ProjectManager ?? "",
                Phone = this.Phone ?? "",
                Status = this.StatusName ?? "",
                PointType = this.StatusName ?? "",
                TransportSoil = this.TransportSoil ?? "",
                TransportCompany = this.TransportCompany ?? "",
                Destination = this.Destination ?? "",
                Class = "ConstructionSite",
                ClassName = "工地",
                BuildingLicence = this.BuildingLicence ?? "",
                AreaAmount = this.AreaAmount ?? "",
                License = this.License ?? "" ,
                Comments=this.Comments??""
                
            };
        }
    }
}
