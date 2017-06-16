using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aspose.Cells;
using Longjin.Util;
using DevExpress.Xpo;
using Longjin.Model;
using System.Data;
using MapUtil;
using System.Text.RegularExpressions;


using Aspose.Words;
using Aspose.Words.Tables;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class ExcelTest
    {
        DevExpress.Xpo.Session session = null;

        public ExcelTest()
        {
            Longjin.Framework.DababaseSession.Init();
            session = new Session(System.Web.HttpRuntime.Cache.Get("XPO_LAYER") as IDataLayer);
        }
        [TestMethod]
        public void ImportData()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\Users\longjin\Downloads\案例-建筑垃圾、渣土运输企业排摸情况 .xls");
            DataTable dt = ds.Tables[0];
            int startRow = 3;




            while (dt.Rows[startRow][0].ToString() != "")
            {
                string enterpriseName = dt.Rows[startRow][0].ToString();
                string address = dt.Rows[startRow][1].ToString();
                string legalPerson = dt.Rows[startRow][2].ToString();
                string phone = dt.Rows[startRow][3].ToString();
                string ifMajor = dt.Rows[startRow][4].ToString();
                string ifOnline = dt.Rows[startRow][5].ToString();
                string carNo = dt.Rows[startRow][6].ToString();
                string driver = dt.Rows[startRow][7].ToString();
                string driveerPhone = dt.Rows[startRow][8].ToString();
                string tong = dt.Rows[startRow][9].ToString();
                string ifGps = dt.Rows[startRow][10].ToString();
                string topLight = dt.Rows[startRow][11].ToString();
                string structure = dt.Rows[startRow][12].ToString();
                string checkStitutation = dt.Rows[startRow][13].ToString();

                string blong;
                string blat;
                string longgitude;
                string latitud;
                BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                ConstructionWaste record = new ConstructionWaste(session);
                record.CarNo = carNo;
                record.Address = address;
                record.CheckSitutation = checkStitutation;
                record.Driver = driver;
                record.DriverPhone = driveerPhone;
                record.EnterpriseName = enterpriseName;
                record.IfGps = ifGps;
                record.IfMajor = ifMajor;
                record.IfOnlineRecord = ifOnline;
                record.LegalPerson = legalPerson;
                record.LegalPersonPhone = phone;
                record.Structure = structure;
                record.TopLightName = topLight;
                record.VehicleTonnage = tong;
                record.BaiduLat = blat;
                record.BaiduLong = blong;
                record.Longitude = longgitude;
                record.Latitude = latitud;
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0].ToString();
                record.GaodeLat = gaodeArr[1].ToString();

                record.Save();

            }

        }
        [TestMethod]
        public void ImportConstruction1()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\360安全浏览器下载\工地与车队提供材料\工地1.xlsx");
            DataTable dt = ds.Tables[0];
            int startRow = 3;




            while (startRow < dt.Rows.Count)
            {
                string archivesNo = dt.Rows[startRow][1].ToString();
                string projectNo = dt.Rows[startRow][2].ToString();
                string constructionName = dt.Rows[startRow][3].ToString();
                string districtName = dt.Rows[startRow][4].ToString();
                string address = dt.Rows[startRow][5].ToString();
                string buildingName = dt.Rows[startRow][6].ToString();
                string majorCompany = dt.Rows[startRow][7].ToString();
                string superviseCompany = dt.Rows[startRow][8].ToString();
                string areaAmount = dt.Rows[startRow][9].ToString();
                string expense = dt.Rows[startRow][10].ToString();
                string projectManager = dt.Rows[startRow][11].ToString();
                string phone = dt.Rows[startRow][12].ToString();
                string transportCompany = dt.Rows[startRow][13].ToString();
                string completeStatus = dt.Rows[startRow][14].ToString();
                string destination = dt.Rows[startRow][15].ToString();
                string status1 = dt.Rows[startRow][16].ToString();
                string status2 = dt.Rows[startRow][17].ToString();
                string status3 = dt.Rows[startRow][18].ToString();
                string status4 = dt.Rows[startRow][19].ToString();
                string blong;
                string blat;
                string longgitude;
                string latitud;
               BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                ConstructionSite record = new ConstructionSite(session);
                record.ArchivesNo = archivesNo;
                record.ProjectNo = projectNo;
                record.Name = constructionName;
                record.District = districtName;
                record.Address = address;
                record.BuildingCompany = buildingName;
                record.MajorCompany = majorCompany;
                record.SuperviseCompany = superviseCompany;
                record.AreaAmount = areaAmount;
                record.Expense = expense.ToNumber<double>();
                record.ProjectManager = projectManager;
                record.Phone = phone;
                record.TransportCompany = transportCompany;
                record.CompleteStatus = completeStatus;
                record.Destination = destination;
                if (status1 == "√")
                    record.Status = 0;
                if (status2 == "√")
                    record.Status = 1;
                if (status3 == "√")
                    record.Status = 2;
                if (status4 == "√")
                    record.Status = 3;
                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);
            }
          
        }

        public void ImportConstruction2()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\360安全浏览器下载\工地与车队提供材料\工地2.xlsx");
            DataTable dt = ds.Tables[0];
            int startRow = 3;




            while (startRow < dt.Rows.Count)
            {
                string archivesNo = dt.Rows[startRow][1].ToString();
                string projectNo = dt.Rows[startRow][2].ToString();
                string constructionName = dt.Rows[startRow][3].ToString();
                string districtName = dt.Rows[startRow][4].ToString();
                string address = dt.Rows[startRow][5].ToString();
                string buildingName = dt.Rows[startRow][6].ToString();
                string majorCompany = dt.Rows[startRow][7].ToString();
                string superviseCompany = dt.Rows[startRow][8].ToString();
                string areaAmount = dt.Rows[startRow][9].ToString();
                string expense = dt.Rows[startRow][10].ToString();
                string projectManager = dt.Rows[startRow][11].ToString();
                string phone = dt.Rows[startRow][12].ToString();
             
                string status1 = dt.Rows[startRow][13].ToString();
                string status2 = dt.Rows[startRow][14].ToString();
                string status3 = dt.Rows[startRow][15].ToString();
                string status4 = dt.Rows[startRow][16].ToString();
                string grouping = dt.Rows[startRow][17].ToString();
                string transportCompany = dt.Rows[startRow][18].ToString();
                string soil = dt.Rows[startRow][19].ToString();
                string license = dt.Rows[startRow][20].ToString();
                string destination = dt.Rows[startRow][21].ToString();
                string comments = dt.Rows[startRow][22].ToString();
                string blong;
                string blat;
                string longgitude;
                string latitud;
                BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                ConstructionSite record = new ConstructionSite(session);
                record.ArchivesNo = archivesNo;
                record.ProjectNo = projectNo;
                record.Name = constructionName;
                record.District = districtName;
                record.Address = address;
                record.BuildingCompany = buildingName;
                record.MajorCompany = majorCompany;
                record.SuperviseCompany = superviseCompany;
                record.AreaAmount = areaAmount;
                record.Expense = expense.ToNumber<double>();
                record.ProjectManager = projectManager;
                record.Phone = phone;
       
                record.Destination = destination;
                if (status1 == "√")
                    record.Status = 0;
                if (status2 == "√")
                    record.Status = 1;
                if (status3 == "√")
                    record.Status = 2;
                if (status4 == "√")
                    record.Status = 3;
                record.Grouping = grouping;
                record.TransportCompany = transportCompany;
                record.TransportSoil = soil;
                record.License = license;
                record.Destination = destination;
                record.Comments = comments;
                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);
            }

        }

        public void ImportConstruction3()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\360安全浏览器下载\工地与车队提供材料\工地3.xlsx");
            DataTable dt = ds.Tables[0];
            int startRow = 2;




            while (startRow < dt.Rows.Count)
            {
              
                string constructionName = dt.Rows[startRow][1].ToString();
                string districtName = dt.Rows[startRow][2].ToString();
                string address = dt.Rows[startRow][3].ToString();
                string buildingName = dt.Rows[startRow][4].ToString();
                string majorCompany = dt.Rows[startRow][5].ToString();
                string builgingLicence = dt.Rows[startRow][6].ToString();
                string areaAmount = dt.Rows[startRow][7].ToString();
           
                string projectManager = dt.Rows[startRow][9].ToString();
                string phone = dt.Rows[startRow][10].ToString();
                string soil = dt.Rows[startRow][11].ToString();


                string transportCompany = dt.Rows[startRow][12].ToString();
             
                string license = dt.Rows[startRow][13].ToString();
                string destination = dt.Rows[startRow][14].ToString();
                string comments = dt.Rows[startRow][15].ToString();
                string blong;
                string blat;
                string longgitude;
                string latitud;
                BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                ConstructionSite record = new ConstructionSite(session);
       
                record.Name = constructionName;
                record.District = districtName;
                record.Address = address;
                record.BuildingCompany = buildingName;
                record.MajorCompany = majorCompany;
          
                record.AreaAmount = areaAmount;
         
                record.ProjectManager = projectManager;
                record.Phone = phone;

                record.Destination = destination;
           
                record.TransportCompany = transportCompany;
                record.TransportSoil = soil;
                record.License = license;
                record.Destination = destination;
                record.Comments = comments;
                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);
            }

        }

        public void ImportTruck1()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\360安全浏览器下载\工地与车队提供材料\车队.xlsx");
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
                int oldAmount = dt.Rows[startRow][9].ToString().ToNumber <int>();
                string ton = dt.Rows[startRow][10].ToString();
                string gps = dt.Rows[startRow][11].ToString();
                string blong;
                string blat;
                string longgitude;
                string latitud;
               BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                TruckGroup record = new TruckGroup(session);

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

                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);
            }

        }

        public void ImportTruck2()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(@"C:\360安全浏览器下载\工地与车队提供材料\车队.xlsx");
            DataTable dt = ds.Tables[0];   //第一个sheet
            int startRow = 2;
            while (startRow < dt.Rows.Count)
            {

                string name = dt.Rows[startRow][1].ToString();
                string address = dt.Rows[startRow][2].ToString();

                string responsiblePerson = dt.Rows[startRow][3].ToString();

                string phone = dt.Rows[startRow][4].ToString();
                string majorBusiness = dt.Rows[startRow][5].ToString();


                string onlineRecord = dt.Rows[startRow][6].ToString();
                 string topLigth = dt.Rows[startRow][7].ToString();
                int total = dt.Rows[startRow][8].ToString().ToNumber<int>();
             
                string ton = dt.Rows[startRow][9].ToString();
          
                string blong;
                string blat;
                string longgitude;
                string latitud;
                BaiduCoordinateUtil.GetCoord(address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                TruckGroup record = new TruckGroup(session);

                record.Name = name;
                record.Address = address;

                record.ResponsiblePerson = responsiblePerson;
                record.Phone = phone;
                record.MajorBussiness = majorBusiness;
                record.OnlineRecord = onlineRecord;
                record.TotalAmount = total;

     

                record.Ton = ton;


                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);
            }

        }

        /// <summary>
        /// 浦东新区建筑垃圾和工程渣土卸点清单
        /// </summary>
        [TestMethod]
        public void ImportConstructionWastesLandfillPoint()
        {
            DataSet ds = ExcelHelper.ExcelReadDataSet(FileUtil.GetMapPath("~/浦东新区建筑垃圾和工程渣土卸点清单.xlsx"));
            DataTable dt = ds.Tables[1];   
            int startRow = 1;
            while (startRow < dt.Rows.Count)
            {

                string name = dt.Rows[startRow][1].ToString();
                string region= dt.Rows[startRow][2].ToString();
                string address = dt.Rows[startRow][3].ToString();
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                Regex reg = new Regex(@"\d{4}-\d{2}-\d{2}");
                
               MatchCollection col= reg.Matches( dt.Rows[startRow][4].ToString());
                
                foreach(Match item in col)
                {
                    if(item.Index==0)
                    start = item.Value.ToDateTime();
                  else
                        end = item.Value.ToDateTime();
                }
                string ton= dt.Rows[startRow][5].ToString();
                string type = dt.Rows[startRow][6].ToString();

                string blong;
                string blat;
                string longgitude;
                string latitud;
                BaiduCoordinateUtil.GetCoord(address.IndexOf("上海")!=-1?address:"上海市"+address, out blong, out blat, out longgitude, out latitud);
                startRow++;
                LandfillSite record = new LandfillSite(session);

                record.Name = name;
                record.Address = address;

                record.ExpiryDateBegin = start;
                record.ExpiryDateEnd = end;
                record.LandfillType = type;
                record.Region = region;
                record.SurplusTon = ton.ToNumber<float>();
                record.BaiduLat = blat.ToNumber<double>();
                record.BaiduLong = blong.ToNumber<double>();
                record.Longitude = longgitude.ToNumber<double>();
                record.Latitude = latitud.ToNumber<double>();
                double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                record.GaodeLong = gaodeArr[0];
                record.GaodeLat = gaodeArr[1];
                record.Save();
                Console.WriteLine(address);


            }
        }


        /// <summary>
        /// 导入车队信息，将原来的大于15吨小于十五吨划为渣土
        /// </summary>
        [TestMethod]
        
        public void ImportTruckBatch()
        {
            
            string path = FileUtil.GetMapPath("~/渣土-浦东新区建筑垃圾运输企业基本信息表.doc");
            try
            {
                Document doc = new Document(path);
                int pageCount = doc.PageCount-1;//31页读取出了32页
                NodeCollection col = doc.GetChildNodes(NodeType.Table, true);
                
                for (var i = 0; i < pageCount; i++)
                {
                    Table t = doc.GetChild(NodeType.Table, i, true) as Table;
                    var rows = t.Rows;
                    string name = rows[0].Cells[1].GetText().Replace("\a","");
                    string registerAddress = rows[1].Cells[1].GetText().Replace("\a", "");
                    string registerMoney = rows[1].Cells[3].GetText().Replace("\a", "");
                    string officeAddress = rows[2].Cells[1].GetText().Replace("\a", "");
                    string postCode = rows[2].Cells[3].GetText().Replace("\a", "");
                    string legalPerson = rows[3].Cells[1].GetText().Replace("\a", "");
                    string legalPersonPhone= rows[3].Cells[3].GetText().Replace("\a", "");
                    string personInCharge= rows[4].Cells[1].GetText().Replace("\a", "");//主要经负责人
                    string personInChargePhone= rows[4].Cells[3].GetText().Replace("\a", "");//
                    string contactPerson= rows[5].Cells[1].GetText().Replace("\a", "");//联系人
                    string contactPersonPhone= rows[5].Cells[3].GetText().Replace("\a", "");//联系人电话
                    string officeArea= rows[6].Cells[1].GetText().Replace("\a", "");//经营面积
                    string staffCount= rows[6].Cells[3].GetText().Replace("\a", "");//员工总数
                    string driver= rows[7].Cells[1].GetText().Replace("\a", "");//驾驶员数量
                    string certificatedDriver= rows[7].Cells[3].GetText().Replace("\a", "");//合格证数量
                    string truckCount= rows[8].Cells[1].GetText().Replace("\a", "");//车数量
                    string truckOnline= rows[8].Cells[3].GetText().Replace("\a", "");//录入市网车辆数
                    string newTruckCount= rows[9].Cells[2].GetText().Replace("\a", "");//新型车辆
                    string refitTruckCount= rows[10].Cells[2].GetText().Replace("\a", "");//改装车
                    string wateringCarCount = rows[11].Cells[2].GetText().Replace("\a", "");//洒水车
                    string diggingMachineCount = rows[12].Cells[2].GetText().Replace("\a", "");//挖机
                    string forkliftCount= rows[13].Cells[2].GetText().Replace("\a", "");//铲车
                  //  string comment= rows[14].Cells[2].GetText();//其他
                    string parkingLotAddressOne= rows[16].Cells[1].GetText();//停车场地址
                    string parkingOneArea = rows[17].Cells[1].GetText();//停车场面积
                    string parkingLotAddressTwo = rows[18].Cells[1].GetText();//停车场地址
                    string parkingTwoArea = rows[19].Cells[1].GetText();//停车场面积
                    Console.WriteLine(i);
                    string blong;
                    string blat;
                    string longgitude;
                    string latitud;
                    BaiduCoordinateUtil.GetCoord(officeAddress.IndexOf("上海") != -1 ? officeAddress : "上海市" + officeAddress, out blong, out blat, out longgitude, out latitud);
                    ConstructionWasteTruck record = new ConstructionWasteTruck(session);
                    record.Name = name;
                    record.Address = officeAddress;
                    record.Phone = legalPersonPhone;
                    record.ContactPerson = contactPerson;
                    record.ContactPhone = contactPersonPhone;
                    record.ResponsiblePerson = legalPerson;
                    record.OfficeArea = officeArea;
                    record.StaffCount = staffCount.ToInt();
                    record.DriverCount = driver.ToInt();
                    record.CertificatedDriverCount = certificatedDriver.ToInt();
                    record.TruckCount = truckCount.ToInt() ;
                    record.TruckOnlineCount = truckOnline.ToInt();
                    record.NewTruckCount = newTruckCount.ToInt();
                    record.RefitTruckCount = refitTruckCount.ToInt();
                    record.WateringTruckCount = wateringCarCount.ToInt();
                    record.DiggingMachineCount = diggingMachineCount.ToInt();
                    record.ForkliftCount = forkliftCount.ToInt();
                    record.BaiduLat = blat.ToNumber<double>();
                    record.BaiduLong = blong.ToNumber<double>();
                    record.Longitude = longgitude.ToNumber<double>();
                    record.Latitude = latitud.ToNumber<double>();
                    double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                    record.GaodeLong = gaodeArr[0];
                    record.GaodeLat = gaodeArr[1];
                    record.Save();
                }
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }


        /// <summary>
        /// 导入车队信息，泥浆车
        /// </summary>
        [TestMethod]

        public void ImportTruckBatch2()
        {

            string path = FileUtil.GetMapPath("~/泥浆车.doc");
            try
            {
                Document doc = new Document(path);
                int pageCount = doc.PageCount;
                NodeCollection col = doc.GetChildNodes(NodeType.Table, true);

                for (var i = 0; i < pageCount; i++)
                {
                    Table t = doc.GetChild(NodeType.Table, i, true) as Table;
                    var rows = t.Rows;
                    string name = rows[0].Cells[1].GetText().Replace("\a", "");
                    string registerAddress = rows[1].Cells[1].GetText().Replace("\a", "");
                    string registerMoney = rows[1].Cells[3].GetText().Replace("\a", "");
                    string officeAddress = rows[2].Cells[1].GetText().Replace("\a", "");
                    string postCode = rows[2].Cells[3].GetText().Replace("\a", "");
                    string legalPerson = rows[3].Cells[1].GetText().Replace("\a", "");
                    string legalPersonPhone = rows[3].Cells[3].GetText().Replace("\a", "");
                    string personInCharge = rows[4].Cells[1].GetText().Replace("\a", "");//主要经负责人
                    string personInChargePhone = rows[4].Cells[3].GetText().Replace("\a", "");//
                    string contactPerson = rows[5].Cells[1].GetText().Replace("\a", "");//联系人
                    string contactPersonPhone = rows[5].Cells[3].GetText().Replace("\a", "");//联系人电话
                    string officeArea = rows[6].Cells[1].GetText().Replace("\a", "");//经营面积
                    string staffCount = rows[6].Cells[3].GetText().Replace("\a", "");//员工总数
                    string driver = rows[7].Cells[1].GetText().Replace("\a", "");//驾驶员数量
                    string certificatedDriver = rows[7].Cells[3].GetText().Replace("\a", "");//合格证数量
                    string truckCount = rows[8].Cells[1].GetText().Replace("\a", "");//车数量
                    string truckOnline = rows[8].Cells[3].GetText().Replace("\a", "");//录入市网车辆数

                    string parkingLotAddressOne = rows[10].Cells[1].GetText().Replace("\a", ""); ;//停车场地址
                    string parkingOneArea = rows[11].Cells[1].GetText().Replace("\a", ""); ;//停车场面积
                    string parkingLotAddressTwo = rows[12].Cells[1].GetText().Replace("\a", ""); ;//停车场地址
                    string parkingTwoArea = rows[13].Cells[1].GetText().Replace("\a", "");//停车场面积
                    Console.WriteLine(i);
                    string blong;
                    string blat;
                    string longgitude;
                    string latitud;
                    BaiduCoordinateUtil.GetCoord(officeAddress.IndexOf("上海") != -1 ? officeAddress : "上海市" + officeAddress, out blong, out blat, out longgitude, out latitud);
                    MudTruck record = new MudTruck(session);
                    record.Name = name;
                    record.Address = officeAddress;
                    record.Phone = legalPersonPhone;
                    record.ContactPerson = contactPerson;
                    record.ContactPhone = contactPersonPhone;
                    record.ResponsiblePerson = legalPerson;
                    record.OfficeArea = officeArea;
                    record.StaffCount = staffCount.ToInt();
                    record.DriverCount = driver.ToInt();
                    record.CertificatedDriverCount = certificatedDriver.ToInt();
                    record.TruckCount = truckCount.ToInt();
                    record.TruckOnlineCount = truckOnline.ToInt();
                    record.ParkinglotOneAddress = parkingLotAddressOne;
                    record.ParkingLotOneArea = parkingOneArea;
                    record.ParkinglotTwoAddress = parkingLotAddressTwo;
                    record.ParkinglotTwoArea = parkingTwoArea;
                    record.BaiduLat = blat.ToNumber<double>();
                    record.BaiduLong = blong.ToNumber<double>();
                    record.Longitude = longgitude.ToNumber<double>();
                    record.Latitude = latitud.ToNumber<double>();
                    double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                    record.GaodeLong = gaodeArr[0];
                    record.GaodeLat = gaodeArr[1];
                    record.Save();
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        /// <summary>
        /// 导入车队信息，装修垃圾
        /// </summary>
        [TestMethod]

        public void ImportTruckBatch3()
        {

            string path = FileUtil.GetMapPath("~/装修垃圾-浦东新区建筑垃圾运输企业基本信息表.doc");
            try
            {
                Document doc = new Document(path);
                int pageCount = doc.PageCount;
                NodeCollection col = doc.GetChildNodes(NodeType.Table, true);

                for (var i = 0; i < pageCount; i++)
                {
                    Table t = doc.GetChild(NodeType.Table, i, true) as Table;
                    var rows = t.Rows;
                    string name = rows[0].Cells[1].GetText().Replace("\a", "");
                    string registerAddress = rows[1].Cells[1].GetText().Replace("\a", "");
                    string registerMoney = rows[1].Cells[3].GetText().Replace("\a", "");
                    string officeAddress = rows[2].Cells[1].GetText().Replace("\a", "");
                    string postCode = rows[2].Cells[3].GetText().Replace("\a", "");
                    string legalPerson = rows[3].Cells[1].GetText().Replace("\a", "");
                    string legalPersonPhone = rows[3].Cells[3].GetText().Replace("\a", "");
                    string personInCharge = rows[4].Cells[1].GetText().Replace("\a", "");//主要经负责人
                    string personInChargePhone = rows[4].Cells[3].GetText().Replace("\a", "");//
                    string contactPerson = rows[5].Cells[1].GetText().Replace("\a", "");//联系人
                    string contactPersonPhone = rows[5].Cells[3].GetText().Replace("\a", "");//联系人电话
                    string officeArea = rows[6].Cells[1].GetText().Replace("\a", "");//经营面积
                    string staffCount = rows[6].Cells[3].GetText().Replace("\a", "");//员工总数
                    string driver = rows[7].Cells[1].GetText().Replace("\a", "");//驾驶员数量
                    string certificatedDriver = rows[7].Cells[3].GetText().Replace("\a", "");//合格证数量
                    string truckCount = rows[8].Cells[1].GetText().Replace("\a", "");//车数量
                    string truckOnline = rows[10].Cells[1].GetText().Replace("\a", "");//录入市网车辆数
                    string truckEightTon = rows[8].Cells[3].GetText().Replace("\a", "");//录入市网车辆数
                    string truckFiveTon = rows[9].Cells[3].GetText().Replace("\a", "");//录入市网车辆数
                   string forkliftCOunt= rows[11].Cells[2].GetText().Replace("\a", "");//叉车
                    string parkingLotAddressOne = rows[13].Cells[1].GetText().Replace("\a", ""); ;//停车场地址
                    string parkingOneArea = rows[14].Cells[1].GetText().Replace("\a", ""); ;//停车场面积
                    string parkingLotAddressTwo = rows[15].Cells[1].GetText().Replace("\a", ""); ;//停车场地址
                    string parkingTwoArea = rows[16].Cells[1].GetText().Replace("\a", "");//停车场面积
                    Console.WriteLine(i);
                    string blong;
                    string blat;
                    string longgitude;
                    string latitud;
                    BaiduCoordinateUtil.GetCoord(officeAddress.IndexOf("上海") != -1 ? officeAddress : "上海市" + officeAddress, out blong, out blat, out longgitude, out latitud);
                    DecorationWasteTruck record = new DecorationWasteTruck(session);
                    record.Name = name;
                    record.Address = officeAddress;
                    record.Phone = legalPersonPhone;
                    record.ContactPerson = contactPerson;
                    record.ContactPhone = contactPersonPhone;
                    record.ResponsiblePerson = legalPerson;
                    record.OfficeArea = officeArea;
                    record.StaffCount = staffCount.ToInt();
                    record.DriverCount = driver.ToInt();
                    record.CertificatedDriverCount = certificatedDriver.ToInt();
                    record.TruckCount = truckCount.ToInt();
                    record.TruckOnlineCount = truckOnline.ToInt();
                    record.ForkliftCount = forkliftCOunt.ToInt();
                    record.FiveTonTruckCount = truckFiveTon.ToInt();
                    record.EightTonTruckCount = truckEightTon.ToInt();
                    record.ParkinglotOneAddress = parkingLotAddressOne;
                    record.ParkingLotOneArea = parkingOneArea;
                    record.ParkinglotTwoAddress = parkingLotAddressTwo;
                    record.ParkinglotTwoArea = parkingTwoArea;
                    record.BaiduLat = blat.ToNumber<double>();
                    record.BaiduLong = blong.ToNumber<double>();
                    record.Longitude = longgitude.ToNumber<double>();
                    record.Latitude = latitud.ToNumber<double>();
                    double[] gaodeArr = BaiduGdConverter.BaiduToGaode(blat.ToNumber<double>(), blong.ToNumber<double>());
                    record.GaodeLong = gaodeArr[0];
                    record.GaodeLat = gaodeArr[1];
                    record.Save();
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}
