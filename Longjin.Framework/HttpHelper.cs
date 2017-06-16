using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Longjin.Framework
{
    public class HttpHelper
    {
        /// <summary>
        /// 获取当前请求的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip4address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ip4address = IPA.ToString();
                    break;
                }
            }

            if (ip4address != String.Empty)
            {
                return ip4address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ip4address = IPA.ToString();
                    break;
                }
            }

            return ip4address;
        }

        /// <summary>
        /// 获取Request所有参数 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetRequestParam()
        {
            var request = HttpContext.Current.Request;
            var ret = new Dictionary<string, Dictionary<string, string>>();
            if (request.QueryString.AllKeys.Count() > 0)
            {
                var param = new Dictionary<string, string>();
                foreach (var item in request.QueryString.AllKeys)
                {
                    var key = item;
                    if (string.IsNullOrEmpty(key))
                    {
                        key = "NullKey";
                    }
                    if (param.ContainsKey(key))
                    {
                        param[key] = param[key] + " " + request.QueryString[key] ?? string.Empty;
                    }
                    else
                    {
                        param.Add(key, request.QueryString[key] ?? string.Empty);
                    }
                }
                ret.Add("QueryString", param);
            }
            if (request.Form.AllKeys.Count() > 0)
            {
                var param = new Dictionary<string, string>();
                foreach (var item in request.Form.AllKeys)
                {
                    var key = item;
                    if (string.IsNullOrEmpty(key))
                    {
                        key = "NullKey";
                    }
                    if (param.ContainsKey(key))
                    {
                        param[key] = param[key] + " " + request.Form[key] ?? string.Empty;
                    }
                    else
                    {
                        param.Add(key, request.Form[key] ?? string.Empty);
                    }
                }
                ret.Add("Form", param);
            }
            return ret;
        }

        /// <summary>
        /// 发送POTS 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string data, Dictionary<string, string> headers = null, string charset = "UTF-8", int timeout = 15000)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = timeout;
            request.AllowAutoRedirect = false;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            byte[] paramBytes = Encoding.GetEncoding(charset).GetBytes(data);
            request.ContentLength = paramBytes.Length;
            WebResponse response = null;
            string responseStr = string.Empty;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(paramBytes, 0, paramBytes.Length);
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = reader.ReadToEnd();
                    }
                }
            }
            return responseStr;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <param name="headers"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string UploadFile(string url, string data, string filePath, Dictionary<string, string> headers = null, string charset = "UTF-8")
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            int pos = filePath.LastIndexOf("\\");
            string fileName = filePath.Substring(pos + 1);
            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    postStream.Write(bArr, 0, bArr.Length);
                    postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                    //发送请求并获取相应回应数据
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        //直到request.GetResponse()程序才开始向目标网页发送Post请求
                        using (Stream instream = response.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(instream, Encoding.UTF8))
                            {
                                //返回结果网页（html）代码
                                string content = sr.ReadToEnd();
                                return content;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 对参数进行升序排序并拼接字符串p1=?&p2=
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GenderParamString(Dictionary<string, string> param)
        {
            var paramStr = string.Empty;
            if (param.ExistsData())
            {
                var data = new StringBuilder();
                var keys = param.Keys.ToList().OrderBy(item => item);
                foreach (var key in keys)
                {
                    data.AppendFormat("&{0}={1}", key, param[key]);
                }
                paramStr = data.ToString();
                paramStr = paramStr.Remove(0, 1);
            }
            return paramStr;
        }
    }
}
