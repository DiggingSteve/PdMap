using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Xpo;
using Newtonsoft.Json;
using Longjin.Framework;

namespace PdMap.Web
{
    public class BasePage : System.Web.UI.Page
    {
    
     


        private Session _dbSession;

        public Session DBSession
        {
            get
            {
                if (_dbSession == null)
                {
                    _dbSession = Longjin.Framework.DababaseSession.CreateSession();
                }
                return _dbSession;
            }
        }

        private string _rootPath;
        public string RootPath
        {
            get
            {
                if (_rootPath == null)
                {
                    _rootPath = Request.ApplicationPath;
                    if (_rootPath == "/")
                    {
                        _rootPath = "";
                    }
                }
                return _rootPath;
            }
        }

        private string _method;
        /// <summary>
        /// AJAX请求方法名
        /// </summary>
        public string Method
        {
            get
            {
                if (string.IsNullOrEmpty(_method))
                {
                    _method = Request["method"];
                }
                return _method;
            }
            set
            {
                _method = value;
            }
        }

        /// <summary>
        /// 页面加载时执行   会执行方法的路由，如果不希望使用方法路由，在页面重写该方法即可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var method = this.GetType().GetMethod(Method);
            if (method == null)
            {
                throw new Exception(string.Format("{0}方法不存在", Method));
            }
            method.Invoke(this, null);
            // Response.Write(RetModel.ToJson());
            Response.ContentType = "application/json; charset=utf-8";
            Response.End();
        }

        /// <summary>
        /// 页面异常时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Log4net.Write("页面执行异常", ex);
            Log4net.Debug(string.Format("Method:{0} Param:{1}", "Page_Error", JsonConvert.SerializeObject(HttpHelper.GetRequestParam())));
            if (Request.UrlReferrer != null)
            {
                Response.Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                Response.Redirect(ResolveUrl("~/Index.aspx"));
            }
        }
    }
}