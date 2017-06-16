using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Longjin.Util
{
    public class retData
    {
        public string telString;
        public string province;
        public string carrier;
    }

    public class jsonResult
    {
        public int errNum;
        public string errMsg;
        public retData retData;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MobileUtil
    {
        private MobileUtil() { }

        /// <summary>
        /// 判断手机号码类型
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>0-非上海手机 1-电信 2-移动 3-联通 4-虚拟运营商(电信) 5-虚拟运营商(移动) 6-虚拟运营商(联通)</returns>
        public static int JudgeMobileType(string mobile)
        {
            int result = 0;

            string url = "http://apis.baidu.com/apistore/mobilephoneservice/mobilephone";
            string param = "tel=" + mobile;

            string strURL = url + '?' + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "44d273e613c2fea1435a9022a401170f");
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }

            jsonResult _json_result = JsonConvert.DeserializeObject<jsonResult>(strValue);

            try
            {
                if (_json_result.retData.province == "上海")
                {
                    string sub = mobile.Substring(0, 3);
                    if (sub == "133" || sub == "153" || sub == "173" || sub == "177" || sub == "180" || sub == "181" || sub == "189")
                    {
                        result = 1;
                    }
                    else if (sub == "134" || sub == "135" || sub == "136" || sub == "137" || sub == "138" || sub == "139" || sub == "147" || sub == "150" || sub == "151" || sub == "152" || sub == "157" || sub == "158" || sub == "159" || sub == "182" || sub == "183" || sub == "184" || sub == "187" || sub == "188")
                    {
                        result = 2;
                    }
                    else if (sub == "130" || sub == "131" || sub == "132" || sub == "145" || sub == "155" || sub == "156" || sub == "171" || sub == "175" || sub == "176" || sub == "185" || sub == "186")
                    {
                        result = 3;
                    }
                    else if (sub == "170")
                    {
                        string _sub = mobile.Substring(0, 4);
                        if (_sub == "1700" || _sub == "1701" || _sub == "1702")
                            result = 4;
                        else if (_sub == "1705")
                            result = 5;
                        else if (_sub == "1707" || _sub == "1708" || _sub == "1709")
                            result = 6;
                        //result = 4;
                    }
                }
            }
            catch
            {

            }

            return result;
        }



        /// <summary>
        /// 判断固定电话类型
        /// </summary>
        /// <param name="landline">固定电话</param>
        /// <returns>1-电信 2-移动 3-联通</returns>
        public static int JudgeLandLineType(string landline)
        {
            //--总队-龙君 31、51开头是移动、60、61是联通，其他基本都是电信
            int result = 0;
            try
            {
                string sub = landline.Substring(0, 2);

                if (sub == "31" || sub == "51")
                {
                    result = 2;
                }
                else if (sub == "60" || sub == "61")
                {
                    result = 3;
                }
                else
                {
                    result = 1;
                }
            }
            catch
            {

            }

            return result;
        }
    }
}
