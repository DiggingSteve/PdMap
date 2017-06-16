using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Longjin.Util
{
    /// <summary>
    /// HTTP
    /// </summary>
    public class HttpUtil
    {
        private HttpUtil() { }

        /// <summary>
        /// HTTP GET方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string result = client.DownloadString(url);
            return result;
        }

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="data">POST的数据</param>
        /// <param name="headers">需要添加的头部信息</param>
        /// <param name="charset">字符编码</param>
        /// <returns></returns>
        public static string HttpPost(string url, string data, Dictionary<string, string> headers = null, string charset = "UTF-8")
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
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
            string responseStr = null;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(paramBytes, 0, paramBytes.Length);
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            string responseStr = null;
            var response = request.GetResponse();
            if (response != null)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            return responseStr;
        }
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        /// <summary>
        /// https Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strData"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string HttpsPost(string url, string strData)
        {
            Encoding charset=Encoding.GetEncoding("utf-8");
         HttpWebRequest request = null;
            //HTTPSQ请求
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
              
            request.UserAgent = DefaultUserAgent;
           
                byte[] data = charset.GetBytes(strData);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            string responseStr = null;
            var response= request.GetResponse() as HttpWebResponse;
            if (response != null)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            return responseStr;
        }
    }
}
