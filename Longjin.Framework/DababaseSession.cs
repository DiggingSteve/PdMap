using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Reflection;
using System.Web;

namespace Longjin.Framework
{
    public class DababaseSession
    {
        /// <summary>
        /// 数据库初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                connectionStr = connectionStr.DecryptDES();

                // 建数据库
                DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();

                using (DevExpress.Xpo.IDataLayer simpleLayer = DevExpress.Xpo.XpoDefault.GetDataLayer(connectionStr, dict, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema))
                {

                    var assembly = Assembly.Load("Longjin.Model");
                    var class_list = assembly.GetTypes();
                    foreach (var type in class_list)
                    {
                        if (typeof(XPCustomObject).IsAssignableFrom(type))
                        {
                            simpleLayer.UpdateSchema(false, dict.QueryClassInfo(type));
                        }
                    }
                }

                // 获取数据存储
                var store = XpoDefault.GetConnectionProvider(connectionStr, AutoCreateOption.DatabaseAndSchema);

                // 数据访问层
                var layer = new ThreadSafeDataLayer(dict, store);
                HttpRuntime.Cache.Insert("XPO_LAYER", layer);
                XpoDefault.Session = new Session(layer);
            }
            catch (Exception ex)
            {
                Log4net.Fatal("数据库初始化异常", ex);
            }
        }

        /// <summary>
        /// 创建并返回一个Session
        /// </summary>
        /// <returns></returns>
        public static Session CreateSession()
        {
            Session dbsession = null;
            var xpo_layer = HttpRuntime.Cache["XPO_LAYER"];
            if (xpo_layer != null)
            {
                dbsession = new Session(xpo_layer as IDataLayer);
            }
            else
            {
                Init();
                dbsession=new Session( HttpRuntime.Cache["XPO_LAYER"] as IDataLayer);
            }
  
            return dbsession;
        }
    }
}
