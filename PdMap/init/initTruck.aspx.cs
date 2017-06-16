using DevExpress.Xpo;
using Longjin.Model;
using Longjin.Util;
using MapUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PdMap.Web
{
    public partial class initTruck : BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(Server.MapPath( @"车队.xlsx"));
            DataTable dt = ds.Tables[1];   //第一个sheet
            int startRow = 2;




            while (startRow < dt.Rows.Count)
            {

                string name = dt.Rows[startRow][1].ToString();
                string address = dt.Rows[startRow][2].ToString();

                string responsiblePerson = dt.Rows[startRow][3].ToString();

                string phone = dt.Rows[startRow][4].ToString();
                string majorBusiness = dt.Rows[startRow][5].ToString();


                string onlineRecord = dt.Rows[startRow][6].ToString();

                int total = dt.Rows[startRow][7].ToString().ToNumber<int>();
                int newAmount = dt.Rows[startRow][8].ToString().ToNumber<int>();
                int oldAmount = dt.Rows[startRow][9].ToString().ToNumber<int>();
                string ton = dt.Rows[startRow][10].ToString();
                string gps = dt.Rows[startRow][11].ToString();
             
                startRow++;
                var record = new XPQuery<TruckGroup>(DBSession).Where(t=>t.Name.Trim()==name.Trim()).FirstOrDefault();

                if (record == null)
                    record = new TruckGroup(DBSession);
                record.Name = name;
                record.Address = address;

                record.ResponsiblePerson = responsiblePerson;
                record.Phone = phone;
                record.MajorBussiness = majorBusiness;
                record.OnlineRecord = onlineRecord;
                record.TotalAmount = total;

                record.NewAmount = newAmount;
                record.OldAmount = oldAmount;

                record.Ton = ton;
                if (gps == "有")
                    record.GpsBolen = true;
                else
                    record.GpsBolen = false;
                //string blong;             
                //string blat;
                //string longgitude;
                //string latitud;
                //BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                //record.BaiduLat = blat.ToNumber<double>();
                //record.BaiduLong = blong.ToNumber<double>();
                //record.Longitude = longgitude.ToNumber<double>();
                //record.Latitude = latitud.ToNumber<double>();
                //double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                //record.GaodeLong = gaodeArr[0];
                //record.GaodeLat = gaodeArr[1];
                record.Save();
                Response.Write(record.Name);
            }
        }
    }
}