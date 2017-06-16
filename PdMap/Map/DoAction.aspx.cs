using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using Longjin.Model;
using Longjin.Model.ApiModel;
using Newtonsoft.Json;

namespace PdMap.Web.Map
{
    public partial class DoAction : BasePage
    {
        public void GetTruckAndConstruction()
        {

            #region 建筑工地
            IQueryable<ConstructionSite> constructionCol = new XPQuery<ConstructionSite>(DBSession);
            constructionCol = constructionCol.Where(t => t.GaodeLat > 30.40 && t.GaodeLat < 31.53 && t.GaodeLong < 122.12 && t.GaodeLong > 121.20);//过滤空的坐标

            List<ConstructionSite> constructionLst = constructionCol.ToList();
            #endregion
            #region 渣土
            //渣土卸点集合
            IQueryable<LandfillSite> landfillCol = new XPQuery<LandfillSite>(DBSession);
            //landfillCol= landfillCol.Where(t => t.GaodeLat > 30.40 
            //&& t.GaodeLat < 31.53 && t.GaodeLong < 122.12 && t.GaodeLong > 121.20);//过滤空的坐标
            List<LandfillSite> landfillList = landfillCol.ToList();
            #endregion

            #region 车队
            IQueryable<ConstructionWasteTruck> constructionWasteTruckCol = new XPQuery<ConstructionWasteTruck>(DBSession);
            constructionWasteTruckCol = constructionWasteTruckCol.Where(t => t.GaodeLat > 30.40 && t.GaodeLat < 31.53 && t.GaodeLong < 122.12 && t.GaodeLong > 121.20);//过滤空的坐标
            List<ConstructionWasteTruck> constructionWasteTruckLst = constructionWasteTruckCol.ToList();

            IQueryable<MudTruck> mudTruckCol = new XPQuery<MudTruck>(DBSession);
            mudTruckCol = mudTruckCol.Where(t => t.GaodeLat > 30.40 && t.GaodeLat < 31.53 && t.GaodeLong < 122.12 && t.GaodeLong > 121.20);//过滤空的坐标
            List<MudTruck> mudTruckList = mudTruckCol.ToList();

            IQueryable<DecorationWasteTruck> decorationTruckCol = new XPQuery<DecorationWasteTruck>(DBSession);
            decorationTruckCol = decorationTruckCol.Where(t => t.GaodeLat > 30.40 && t.GaodeLat < 31.53 && t.GaodeLong < 122.12 && t.GaodeLong > 121.20);//过滤空的坐标
            List<DecorationWasteTruck> decorationTruckList = decorationTruckCol.ToList();

            #endregion

     

            List<MapLayer> layerLst = new List<MapLayer>();
            #region 工地
            foreach (var item in constructionLst)
            {
                string layerType = item.PointType;
                if (layerType == "未验") continue;
                MapLayer layer = layerLst.Find(delegate (MapLayer p) { return p.Key == layerType; });
                if (layer == null)
                {
                    layer = new MapLayer(layerType, "construction");
                    layerLst.Add(layer);
                }
                layer.Points.Add(item.Clone());
            }
            #endregion

            #region 车队
            foreach (var item in constructionWasteTruckLst)
            {
                string layerType = item.PointType;
                
                MapLayer layer = layerLst.Find(delegate (MapLayer p) { return p.Key == layerType; });
                if (layer == null)
                {
                    layer = new MapLayer(layerType, "truck");
                    layerLst.Add(layer);
                }
                layer.Points.Add(item.Clone());
            }
            foreach (var item in mudTruckList)
            {
                string layerType = item.PointType;
                if (layerType == "未验") continue;
                MapLayer layer = layerLst.Find(delegate (MapLayer p) { return p.Key == layerType; });
                if (layer == null)
                {
                    layer = new MapLayer(layerType, "truck");
                    layerLst.Add(layer);
                }
                layer.Points.Add(item.Clone());
            }
            foreach (var item in decorationTruckList)
            {
                string layerType = item.PointType;
                if (layerType == "未验") continue;
                MapLayer layer = layerLst.Find(delegate (MapLayer p) { return p.Key == layerType; });
                if (layer == null)
                {
                    layer = new MapLayer(layerType, "truck");
                    layerLst.Add(layer);
                }
                layer.Points.Add(item.Clone());
            }
            #endregion

            #region 渣土卸点
            foreach (var item in landfillList)
            {
                string layerType = item.PointType;
                MapLayer layer = layerLst.Find(delegate (MapLayer p) { return p.Key == layerType; });
                if (layer == null)
                {
                    layer = new MapLayer(layerType, "landfill");
                    layerLst.Add(layer);
                }
                layer.Points.Add(item.Clone());
            }
            #endregion
            Response.Write(JsonConvert.SerializeObject(layerLst));
        }


    }
}