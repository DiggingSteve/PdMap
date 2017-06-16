
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;

namespace Longjin.Util
{
    public sealed class FileUtil
    {

        #region 记录日志文件
        /// <summary>
        /// 记录日志文件
        /// </summary>
        /// <param name="logpath">日志文件存放路径</param>
        /// <param name="content">日志内容</param>
        public static void Log(string logpath, string content)
        {
            try
            {
                string path = logpath + "\\" + DateTime.Today.ToString("yyyyMMdd");
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                string fileName = "log_" + DateTime.Now.ToString("HHmmssffff") + ".txt";

                System.IO.FileInfo file = new System.IO.FileInfo(path + "\\" + fileName);
                System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(file.Open(System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite));
                StringBuilder sb = new StringBuilder();
                sb.Append("-----------------------------------");
                sb.Append("\r\n");
                sb.Append(content);
                sb.Append("\r\n");
                sb.Append("-----------------------------------");
                sb.Append("\r\n");
                fileWriter.Write(sb.ToString());
                fileWriter.Flush();
                fileWriter.Close();
            }
            catch
            {
            }
        }
        #endregion

        #region 文件下载
        // 使用方法：
        // Page.Response.Clear();
        // bool success = ResponseFile(Page.Request, Page.Response, "目的文件名称", @"源文件路径", 1024000); 源文件路径为完整路径
        // if (!success)
        //      Response.Write("下载文件出错！");
        // Page.Response.End();
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="_Request">HttpRequest</param>
        /// <param name="_Response">HttpResponse</param>
        /// <param name="_fileName">目的文件名称</param>
        /// <param name="_fullPath">源文件路径</param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                if (_Request == null || _Response == null) return false;

                if (!File.Exists(_fullPath)) return false;

                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    double pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    if (_speed == 0) _speed = 1024000;
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        //Partial Content 客户发送了一个带有Range头的GET请求（分块请求），服务器完成了它（HTTP 1.1新）。
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    //_Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + _fileName);

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();

                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;

            //
        }
        #endregion

        #region 文件大小转换
        // WLN.Util.FilesUtil.ConvertToFileSize(2048) 输出 2 KB
        // WLN.Util.FilesUtil.ConvertToFileSize(960) 输出 960 Bytes
        /// <summary>
        /// 文件大小转换
        /// </summary>
        /// <param name="size">文件大小</param>
        /// <returns></returns>
        public static string ConvertToFileSize(long size)
        {
            string[] hz = new string[] { " Bytes", " KB", " MB", " GB", " TB", " PB", " EB", " ZB", " YB" };

            int index = Convert.ToInt32(System.Math.Floor(System.Math.Log(size, 1024)));

            double divisor = size / System.Math.Pow(1024, System.Math.Floor(System.Math.Log(size, 1024)));

            return divisor.ToString("0.##" + hz[index]);
        }
        #endregion

        #region 获取文件的mimetype
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            #region Big freaking list of mime types
             // combination of values from Windows 7 Registry and
             // from C:\Windows\System32\inetsrv\config\applicationHost.config
             // some added, including .7z and .dat
             {".323", "text/h323"},
             {".3g2", "video/3gpp2"},
             {".3gp", "video/3gpp"},
             {".3gp2", "video/3gpp2"},
             {".3gpp", "video/3gpp"},
             {".7z", "application/x-7z-compressed"},
             {".aa", "audio/audible"},
             {".AAC", "audio/aac"},
             {".aaf", "application/octet-stream"},
             {".aax", "audio/vnd.audible.aax"},
             {".ac3", "audio/ac3"},
             {".aca", "application/octet-stream"},
             {".accda", "application/msaccess.addin"},
             {".accdb", "application/msaccess"},
             {".accdc", "application/msaccess.cab"},
             {".accde", "application/msaccess"},
             {".accdr", "application/msaccess.runtime"},
             {".accdt", "application/msaccess"},
             {".accdw", "application/msaccess.webapplication"},
             {".accft", "application/msaccess.ftemplate"},
             {".acx", "application/internet-property-stream"},
             {".AddIn", "text/xml"},
             {".ade", "application/msaccess"},
             {".adobebridge", "application/x-bridge-url"},
             {".adp", "application/msaccess"},
             {".ADT", "audio/vnd.dlna.adts"},
             {".ADTS", "audio/aac"},
             {".afm", "application/octet-stream"},
             {".ai", "application/postscript"},
             {".aif", "audio/x-aiff"},
             {".aifc", "audio/aiff"},
             {".aiff", "audio/aiff"},
             {".air", "application/vnd.adobe.air-application-installer-package+zip"},
             {".amc", "application/x-mpeg"},
             {".application", "application/x-ms-application"},
             {".art", "image/x-jg"},
             {".asa", "application/xml"},
             {".asax", "application/xml"},
             {".ascx", "application/xml"},
             {".asd", "application/octet-stream"},
             {".asf", "video/x-ms-asf"},
             {".ashx", "application/xml"},
             {".asi", "application/octet-stream"},
             {".asm", "text/plain"},
             {".asmx", "application/xml"},
             {".aspx", "application/xml"},
             {".asr", "video/x-ms-asf"},
             {".asx", "video/x-ms-asf"},
             {".atom", "application/atom+xml"},
             {".au", "audio/basic"},
             {".avi", "video/x-msvideo"},
             {".axs", "application/olescript"},
             {".bas", "text/plain"},
             {".bcpio", "application/x-bcpio"},
             {".bin", "application/octet-stream"},
             {".bmp", "image/bmp"},
             {".c", "text/plain"},
             {".cab", "application/octet-stream"},
             {".caf", "audio/x-caf"},
             {".calx", "application/vnd.ms-office.calx"},
             {".cat", "application/vnd.ms-pki.seccat"},
             {".cc", "text/plain"},
             {".cd", "text/plain"},
             {".cdda", "audio/aiff"},
             {".cdf", "application/x-cdf"},
             {".cer", "application/x-x509-ca-cert"},
             {".chm", "application/octet-stream"},
             {".class", "application/x-java-applet"},
             {".clp", "application/x-msclip"},
             {".cmx", "image/x-cmx"},
             {".cnf", "text/plain"},
             {".cod", "image/cis-cod"},
             {".config", "application/xml"},
             {".contact", "text/x-ms-contact"},
             {".coverage", "application/xml"},
             {".cpio", "application/x-cpio"},
             {".cpp", "text/plain"},
             {".crd", "application/x-mscardfile"},
             {".crl", "application/pkix-crl"},
             {".crt", "application/x-x509-ca-cert"},
             {".cs", "text/plain"},
             {".csdproj", "text/plain"},
             {".csh", "application/x-csh"},
             {".csproj", "text/plain"},
             {".css", "text/css"},
             {".csv", "text/csv"},
             {".cur", "application/octet-stream"},
             {".cxx", "text/plain"},
             {".dat", "application/octet-stream"},
             {".datasource", "application/xml"},
             {".dbproj", "text/plain"},
             {".dcr", "application/x-director"},
             {".def", "text/plain"},
             {".deploy", "application/octet-stream"},
             {".der", "application/x-x509-ca-cert"},
             {".dgml", "application/xml"},
             {".dib", "image/bmp"},
             {".dif", "video/x-dv"},
             {".dir", "application/x-director"},
             {".disco", "text/xml"},
             {".dll", "application/x-msdownload"},
             {".dll.config", "text/xml"},
             {".dlm", "text/dlm"},
             {".doc", "application/msword"},
             {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
             {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
             {".dot", "application/msword"},
             {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
             {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
             {".dsp", "application/octet-stream"},
             {".dsw", "text/plain"},
             {".dtd", "text/xml"},
             {".dtsConfig", "text/xml"},
             {".dv", "video/x-dv"},
             {".dvi", "application/x-dvi"},
             {".dwf", "drawing/x-dwf"},
             {".dwp", "application/octet-stream"},
             {".dxr", "application/x-director"},
             {".eml", "message/rfc822"},
             {".emz", "application/octet-stream"},
             {".eot", "application/octet-stream"},
             {".eps", "application/postscript"},
             {".etl", "application/etl"},
             {".etx", "text/x-setext"},
             {".evy", "application/envoy"},
             {".exe", "application/octet-stream"},
             {".exe.config", "text/xml"},
             {".fdf", "application/vnd.fdf"},
             {".fif", "application/fractals"},
             {".filters", "Application/xml"},
             {".fla", "application/octet-stream"},
             {".flr", "x-world/x-vrml"},
             {".flv", "video/x-flv"},
             {".fsscript", "application/fsharp-script"},
             {".fsx", "application/fsharp-script"},
             {".generictest", "application/xml"},
             {".gif", "image/gif"},
             {".group", "text/x-ms-group"},
             {".gsm", "audio/x-gsm"},
             {".gtar", "application/x-gtar"},
             {".gz", "application/x-gzip"},
             {".h", "text/plain"},
             {".hdf", "application/x-hdf"},
             {".hdml", "text/x-hdml"},
             {".hhc", "application/x-oleobject"},
             {".hhk", "application/octet-stream"},
             {".hhp", "application/octet-stream"},
             {".hlp", "application/winhlp"},
             {".hpp", "text/plain"},
             {".hqx", "application/mac-binhex40"},
             {".hta", "application/hta"},
             {".htc", "text/x-component"},
             {".htm", "text/html"},
             {".html", "text/html"},
             {".htt", "text/webviewhtml"},
             {".hxa", "application/xml"},
             {".hxc", "application/xml"},
             {".hxd", "application/octet-stream"},
             {".hxe", "application/xml"},
             {".hxf", "application/xml"},
             {".hxh", "application/octet-stream"},
             {".hxi", "application/octet-stream"},
             {".hxk", "application/xml"},
             {".hxq", "application/octet-stream"},
             {".hxr", "application/octet-stream"},
             {".hxs", "application/octet-stream"},
             {".hxt", "text/html"},
             {".hxv", "application/xml"},
             {".hxw", "application/octet-stream"},
             {".hxx", "text/plain"},
             {".i", "text/plain"},
             {".ico", "image/x-icon"},
             {".ics", "application/octet-stream"},
             {".idl", "text/plain"},
             {".ief", "image/ief"},
             {".iii", "application/x-iphone"},
             {".inc", "text/plain"},
             {".inf", "application/octet-stream"},
             {".inl", "text/plain"},
             {".ins", "application/x-internet-signup"},
             {".ipa", "application/x-itunes-ipa"},
             {".ipg", "application/x-itunes-ipg"},
             {".ipproj", "text/plain"},
             {".ipsw", "application/x-itunes-ipsw"},
             {".iqy", "text/x-ms-iqy"},
             {".isp", "application/x-internet-signup"},
             {".ite", "application/x-itunes-ite"},
             {".itlp", "application/x-itunes-itlp"},
             {".itms", "application/x-itunes-itms"},
             {".itpc", "application/x-itunes-itpc"},
             {".IVF", "video/x-ivf"},
             {".jar", "application/java-archive"},
             {".java", "application/octet-stream"},
             {".jck", "application/liquidmotion"},
             {".jcz", "application/liquidmotion"},
             {".jfif", "image/pjpeg"},
             {".jnlp", "application/x-java-jnlp-file"},
             {".jpb", "application/octet-stream"},
             {".jpe", "image/jpeg"},
             {".jpeg", "image/jpeg"},
             {".jpg", "image/jpeg"},
             {".js", "application/x-javascript"},
             {".jsx", "text/jscript"},
             {".jsxbin", "text/plain"},
             {".latex", "application/x-latex"},
             {".library-ms", "application/windows-library+xml"},
             {".lit", "application/x-ms-reader"},
             {".loadtest", "application/xml"},
             {".lpk", "application/octet-stream"},
             {".lsf", "video/x-la-asf"},
             {".lst", "text/plain"},
             {".lsx", "video/x-la-asf"},
             {".lzh", "application/octet-stream"},
             {".m13", "application/x-msmediaview"},
             {".m14", "application/x-msmediaview"},
             {".m1v", "video/mpeg"},
             {".m2t", "video/vnd.dlna.mpeg-tts"},
             {".m2ts", "video/vnd.dlna.mpeg-tts"},
             {".m2v", "video/mpeg"},
             {".m3u", "audio/x-mpegurl"},
             {".m3u8", "audio/x-mpegurl"},
             {".m4a", "audio/m4a"},
             {".m4b", "audio/m4b"},
             {".m4p", "audio/m4p"},
             {".m4r", "audio/x-m4r"},
             {".m4v", "video/x-m4v"},
             {".mac", "image/x-macpaint"},
             {".mak", "text/plain"},
             {".man", "application/x-troff-man"},
             {".manifest", "application/x-ms-manifest"},
             {".map", "text/plain"},
             {".master", "application/xml"},
             {".mda", "application/msaccess"},
             {".mdb", "application/x-msaccess"},
             {".mde", "application/msaccess"},
             {".mdp", "application/octet-stream"},
             {".me", "application/x-troff-me"},
             {".mfp", "application/x-shockwave-flash"},
             {".mht", "message/rfc822"},
             {".mhtml", "message/rfc822"},
             {".mid", "audio/mid"},
             {".midi", "audio/mid"},
             {".mix", "application/octet-stream"},
             {".mk", "text/plain"},
             {".mmf", "application/x-smaf"},
             {".mno", "text/xml"},
             {".mny", "application/x-msmoney"},
             {".mod", "video/mpeg"},
             {".mov", "video/quicktime"},
             {".movie", "video/x-sgi-movie"},
             {".mp2", "video/mpeg"},
             {".mp2v", "video/mpeg"},
             {".mp3", "audio/mpeg"},
             {".mp4", "video/mp4"},
             {".mp4v", "video/mp4"},
             {".mpa", "video/mpeg"},
             {".mpe", "video/mpeg"},
             {".mpeg", "video/mpeg"},
             {".mpf", "application/vnd.ms-mediapackage"},
             {".mpg", "video/mpeg"},
             {".mpp", "application/vnd.ms-project"},
             {".mpv2", "video/mpeg"},
             {".mqv", "video/quicktime"},
             {".ms", "application/x-troff-ms"},
             {".msi", "application/octet-stream"},
             {".mso", "application/octet-stream"},
             {".mts", "video/vnd.dlna.mpeg-tts"},
             {".mtx", "application/xml"},
             {".mvb", "application/x-msmediaview"},
             {".mvc", "application/x-miva-compiled"},
             {".mxp", "application/x-mmxp"},
             {".nc", "application/x-netcdf"},
             {".nsc", "video/x-ms-asf"},
             {".nws", "message/rfc822"},
             {".ocx", "application/octet-stream"},
             {".oda", "application/oda"},
             {".odc", "text/x-ms-odc"},
             {".odh", "text/plain"},
             {".odl", "text/plain"},
             {".odp", "application/vnd.oasis.opendocument.presentation"},
             {".ods", "application/oleobject"},
             {".odt", "application/vnd.oasis.opendocument.text"},
             {".one", "application/onenote"},
             {".onea", "application/onenote"},
             {".onepkg", "application/onenote"},
             {".onetmp", "application/onenote"},
             {".onetoc", "application/onenote"},
             {".onetoc2", "application/onenote"},
             {".orderedtest", "application/xml"},
             {".osdx", "application/opensearchdescription+xml"},
             {".p10", "application/pkcs10"},
             {".p12", "application/x-pkcs12"},
             {".p7b", "application/x-pkcs7-certificates"},
             {".p7c", "application/pkcs7-mime"},
             {".p7m", "application/pkcs7-mime"},
             {".p7r", "application/x-pkcs7-certreqresp"},
             {".p7s", "application/pkcs7-signature"},
             {".pbm", "image/x-portable-bitmap"},
             {".pcast", "application/x-podcast"},
             {".pct", "image/pict"},
             {".pcx", "application/octet-stream"},
             {".pcz", "application/octet-stream"},
             {".pdf", "application/pdf"},
             {".pfb", "application/octet-stream"},
             {".pfm", "application/octet-stream"},
             {".pfx", "application/x-pkcs12"},
             {".pgm", "image/x-portable-graymap"},
             {".pic", "image/pict"},
             {".pict", "image/pict"},
             {".pkgdef", "text/plain"},
             {".pkgundef", "text/plain"},
             {".pko", "application/vnd.ms-pki.pko"},
             {".pls", "audio/scpls"},
             {".pma", "application/x-perfmon"},
             {".pmc", "application/x-perfmon"},
             {".pml", "application/x-perfmon"},
             {".pmr", "application/x-perfmon"},
             {".pmw", "application/x-perfmon"},
             {".png", "image/png"},
             {".pnm", "image/x-portable-anymap"},
             {".pnt", "image/x-macpaint"},
             {".pntg", "image/x-macpaint"},
             {".pnz", "image/png"},
             {".pot", "application/vnd.ms-powerpoint"},
             {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
             {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
             {".ppa", "application/vnd.ms-powerpoint"},
             {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
             {".ppm", "image/x-portable-pixmap"},
             {".pps", "application/vnd.ms-powerpoint"},
             {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
             {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
             {".ppt", "application/vnd.ms-powerpoint"},
             {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
             {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
             {".prf", "application/pics-rules"},
             {".prm", "application/octet-stream"},
             {".prx", "application/octet-stream"},
             {".ps", "application/postscript"},
             {".psc1", "application/PowerShell"},
             {".psd", "application/octet-stream"},
             {".psess", "application/xml"},
             {".psm", "application/octet-stream"},
             {".psp", "application/octet-stream"},
             {".pub", "application/x-mspublisher"},
             {".pwz", "application/vnd.ms-powerpoint"},
             {".qht", "text/x-html-insertion"},
             {".qhtm", "text/x-html-insertion"},
             {".qt", "video/quicktime"},
             {".qti", "image/x-quicktime"},
             {".qtif", "image/x-quicktime"},
             {".qtl", "application/x-quicktimeplayer"},
             {".qxd", "application/octet-stream"},
             {".ra", "audio/x-pn-realaudio"},
             {".ram", "audio/x-pn-realaudio"},
             {".rar", "application/octet-stream"},
             {".ras", "image/x-cmu-raster"},
             {".rat", "application/rat-file"},
             {".rc", "text/plain"},
             {".rc2", "text/plain"},
             {".rct", "text/plain"},
             {".rdlc", "application/xml"},
             {".resx", "application/xml"},
             {".rf", "image/vnd.rn-realflash"},
             {".rgb", "image/x-rgb"},
             {".rgs", "text/plain"},
             {".rm", "application/vnd.rn-realmedia"},
             {".rmi", "audio/mid"},
             {".rmp", "application/vnd.rn-rn_music_package"},
             {".roff", "application/x-troff"},
             {".rpm", "audio/x-pn-realaudio-plugin"},
             {".rqy", "text/x-ms-rqy"},
             {".rtf", "application/rtf"},
             {".rtx", "text/richtext"},
             {".ruleset", "application/xml"},
             {".s", "text/plain"},
             {".safariextz", "application/x-safari-safariextz"},
             {".scd", "application/x-msschedule"},
             {".sct", "text/scriptlet"},
             {".sd2", "audio/x-sd2"},
             {".sdp", "application/sdp"},
             {".sea", "application/octet-stream"},
             {".searchConnector-ms", "application/windows-search-connector+xml"},
             {".setpay", "application/set-payment-initiation"},
             {".setreg", "application/set-registration-initiation"},
             {".settings", "application/xml"},
             {".sgimb", "application/x-sgimb"},
             {".sgml", "text/sgml"},
             {".sh", "application/x-sh"},
             {".shar", "application/x-shar"},
             {".shtml", "text/html"},
             {".sit", "application/x-stuffit"},
             {".sitemap", "application/xml"},
             {".skin", "application/xml"},
             {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
             {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
             {".slk", "application/vnd.ms-excel"},
             {".sln", "text/plain"},
             {".slupkg-ms", "application/x-ms-license"},
             {".smd", "audio/x-smd"},
             {".smi", "application/octet-stream"},
             {".smx", "audio/x-smd"},
             {".smz", "audio/x-smd"},
             {".snd", "audio/basic"},
             {".snippet", "application/xml"},
             {".snp", "application/octet-stream"},
             {".sol", "text/plain"},
             {".sor", "text/plain"},
             {".spc", "application/x-pkcs7-certificates"},
             {".spl", "application/futuresplash"},
             {".src", "application/x-wais-source"},
             {".srf", "text/plain"},
             {".SSISDeploymentManifest", "text/xml"},
             {".ssm", "application/streamingmedia"},
             {".sst", "application/vnd.ms-pki.certstore"},
             {".stl", "application/vnd.ms-pki.stl"},
             {".sv4cpio", "application/x-sv4cpio"},
             {".sv4crc", "application/x-sv4crc"},
             {".svc", "application/xml"},
             {".swf", "application/x-shockwave-flash"},
             {".t", "application/x-troff"},
             {".tar", "application/x-tar"},
             {".tcl", "application/x-tcl"},
             {".testrunconfig", "application/xml"},
             {".testsettings", "application/xml"},
             {".tex", "application/x-tex"},
             {".texi", "application/x-texinfo"},
             {".texinfo", "application/x-texinfo"},
             {".tgz", "application/x-compressed"},
             {".thmx", "application/vnd.ms-officetheme"},
             {".thn", "application/octet-stream"},
             {".tif", "image/tiff"},
             {".tiff", "image/tiff"},
             {".tlh", "text/plain"},
             {".tli", "text/plain"},
             {".toc", "application/octet-stream"},
             {".tr", "application/x-troff"},
             {".trm", "application/x-msterminal"},
             {".trx", "application/xml"},
             {".ts", "video/vnd.dlna.mpeg-tts"},
             {".tsv", "text/tab-separated-values"},
             {".ttf", "application/octet-stream"},
             {".tts", "video/vnd.dlna.mpeg-tts"},
             {".txt", "text/plain"},
             {".u32", "application/octet-stream"},
             {".uls", "text/iuls"},
             {".user", "text/plain"},
             {".ustar", "application/x-ustar"},
             {".vb", "text/plain"},
             {".vbdproj", "text/plain"},
             {".vbk", "video/mpeg"},
             {".vbproj", "text/plain"},
             {".vbs", "text/vbscript"},
             {".vcf", "text/x-vcard"},
             {".vcproj", "Application/xml"},
             {".vcs", "text/plain"},
             {".vcxproj", "Application/xml"},
             {".vddproj", "text/plain"},
             {".vdp", "text/plain"},
             {".vdproj", "text/plain"},
             {".vdx", "application/vnd.ms-visio.viewer"},
             {".vml", "text/xml"},
             {".vscontent", "application/xml"},
             {".vsct", "text/xml"},
             {".vsd", "application/vnd.visio"},
             {".vsi", "application/ms-vsi"},
             {".vsix", "application/vsix"},
             {".vsixlangpack", "text/xml"},
             {".vsixmanifest", "text/xml"},
             {".vsmdi", "application/xml"},
             {".vspscc", "text/plain"},
             {".vss", "application/vnd.visio"},
             {".vsscc", "text/plain"},
             {".vssettings", "text/xml"},
             {".vssscc", "text/plain"},
             {".vst", "application/vnd.visio"},
             {".vstemplate", "text/xml"},
             {".vsto", "application/x-ms-vsto"},
             {".vsw", "application/vnd.visio"},
             {".vsx", "application/vnd.visio"},
             {".vtx", "application/vnd.visio"},
             {".wav", "audio/wav"},
             {".wave", "audio/wav"},
             {".wax", "audio/x-ms-wax"},
             {".wbk", "application/msword"},
             {".wbmp", "image/vnd.wap.wbmp"},
             {".wcm", "application/vnd.ms-works"},
             {".wdb", "application/vnd.ms-works"},
             {".wdp", "image/vnd.ms-photo"},
             {".webarchive", "application/x-safari-webarchive"},
             {".webtest", "application/xml"},
             {".wiq", "application/xml"},
             {".wiz", "application/msword"},
             {".wks", "application/vnd.ms-works"},
             {".WLMP", "application/wlmoviemaker"},
             {".wlpginstall", "application/x-wlpg-detect"},
             {".wlpginstall3", "application/x-wlpg3-detect"},
             {".wm", "video/x-ms-wm"},
             {".wma", "audio/x-ms-wma"},
             {".wmd", "application/x-ms-wmd"},
             {".wmf", "application/x-msmetafile"},
             {".wml", "text/vnd.wap.wml"},
             {".wmlc", "application/vnd.wap.wmlc"},
             {".wmls", "text/vnd.wap.wmlscript"},
             {".wmlsc", "application/vnd.wap.wmlscriptc"},
             {".wmp", "video/x-ms-wmp"},
             {".wmv", "video/x-ms-wmv"},
             {".wmx", "video/x-ms-wmx"},
             {".wmz", "application/x-ms-wmz"},
             {".wpl", "application/vnd.ms-wpl"},
             {".wps", "application/vnd.ms-works"},
             {".wri", "application/x-mswrite"},
             {".wrl", "x-world/x-vrml"},
             {".wrz", "x-world/x-vrml"},
             {".wsc", "text/scriptlet"},
             {".wsdl", "text/xml"},
             {".wvx", "video/x-ms-wvx"},
             {".x", "application/directx"},
             {".xaf", "x-world/x-vrml"},
             {".xaml", "application/xaml+xml"},
             {".xap", "application/x-silverlight-app"},
             {".xbap", "application/x-ms-xbap"},
             {".xbm", "image/x-xbitmap"},
             {".xdr", "text/plain"},
             {".xht", "application/xhtml+xml"},
             {".xhtml", "application/xhtml+xml"},
             {".xla", "application/vnd.ms-excel"},
             {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
             {".xlc", "application/vnd.ms-excel"},
             {".xld", "application/vnd.ms-excel"},
             {".xlk", "application/vnd.ms-excel"},
             {".xll", "application/vnd.ms-excel"},
             {".xlm", "application/vnd.ms-excel"},
             {".xls", "application/vnd.ms-excel"},
             {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
             {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
             {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
             {".xlt", "application/vnd.ms-excel"},
             {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
             {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
             {".xlw", "application/vnd.ms-excel"},
             {".xml", "text/xml"},
             {".xmta", "application/xml"},
             {".xof", "x-world/x-vrml"},
             {".XOML", "text/plain"},
             {".xpm", "image/x-xpixmap"},
             {".xps", "application/vnd.ms-xpsdocument"},
             {".xrm-ms", "text/xml"},
             {".xsc", "application/xml"},
             {".xsd", "text/xml"},
             {".xsf", "text/xml"},
             {".xsl", "text/xml"},
             {".xslt", "text/xml"},
             {".xsn", "application/octet-stream"},
             {".xss", "application/xml"},
             {".xtp", "application/octet-stream"},
             {".xwd", "image/x-xwindowdump"},
             {".z", "application/x-compress"},
             {".zip", "application/x-zip-compressed"},
             #endregion
        };

        /// <summary>
        /// 获取文件的MimeType
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
        #endregion

        #region 真正判断文件类型的关键函数
        /// <summary>
        /// 真正判断文件类型的关键函数
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string GetFileRealType(string filepath)
        {
            #region 文件类型说明
            //jpg: 255,216
            //gif: 71,73
            //bmp: 66,77
            //png: 137,80
            //doc: 208,207
            //docx: 80,75
            //xls: 208,207
            //xlsx: 80,75
            //js: 239,187
            //swf: 67,87
            //txt: 70,67
            //mp3: 73,68
            //wma: 48,38
            //mid: 77,84
            //rar: 82,97
            //zip: 80,75
            //xml: 60,63
            #endregion

            string result = ""; ;
            string[] code = { "255216", "7173", "6677", "13780", "208207", "8075", "208207", "8075", "239187", "6787", "7067", "7368", "4838", "7784", "8297", "8075", "6063" };
            string[] name = { "jpg", "gif", "bmp", "png", "doc", "docx", "xls", "xlsx", "js", "swf", "txt", "mp3", "wma", "mid", "rar", "zip", "xml" };

            FileStream fs = new FileStream(filepath, FileMode.Open);

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            //这里的位长要具体判断.
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {

            }
            r.Close();
            fs.Close();

            bool isHas = false;
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == fileclass)
                {
                    isHas = true;
                    result = "." + name[i];
                    break;
                }
            }

            if (!isHas)
            {
                result = ".unknown";
            }

            return result;
        }
        #endregion

        #region 文件操作

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                if (strPath.StartsWith("~"))
                {
                    strPath = strPath.TrimStart('~');
                }
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > -1)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        public static string GetFileExtByURi(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return "";
            }
            var param_index = uri.IndexOf("?");
            var ext_index = uri.LastIndexOf(".");
            if (param_index != -1)
            {
                uri = uri.Substring(0, param_index);
                ext_index = uri.LastIndexOf(".");
            }
            if (ext_index != -1)
            {
                return uri.Substring(ext_index + 1);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取文件的真实扩展名  手动改变无效
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRealExt(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);
            var fileClass = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                fileClass += reader.ReadByte().ToString();
            }
            return ((FileExtension)fileClass.ToInt()).ToString();
        }

        /// <summary>
        /// 获取文件真是扩展名
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetRealExt(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            var fileClass = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                fileClass += reader.ReadByte().ToString();
            }
            return ((FileExtension)fileClass.ToInt()).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileClass"></param>
        /// <returns></returns>
        public static string GetRealExtByFileClass(string fileClass)
        {
            return ((FileExtension)fileClass.ToInt()).ToString();
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileExt(HttpPostedFile file)
        {
            if (file == null)
            {
                throw new ArgumentException("无效集合数据", "file");
            }

            if (!string.IsNullOrEmpty(file.FileName))
            {
                return file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
            }
            else
            {
                throw new ArgumentException("无效的文件名");
            }

        }

        /// <summary>
        /// 获取文件名 不包含 扩展名
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(HttpPostedFile file)
        {
            if (file == null)
            {
                throw new ArgumentException("无效集合数据", "file");
            }

            if (!string.IsNullOrEmpty(file.FileName))
            {
                return file.FileName.Substring(0, file.FileName.LastIndexOf("."));
            }
            else
            {
                throw new Exception("无效的文件名");
            }

        }

        /// <summary>
        /// 返回文件名不包含扩展名
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string filepath)
        {
            if (filepath.IndexOf("/") != -1)
            {
                filepath = filepath.Substring(filepath.LastIndexOf(@"/") + 1);
            }
            if (filepath.IndexOf(".") != -1)
            {
                filepath = filepath.Substring(0, filepath.LastIndexOf("."));
            }
            return filepath;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string filepath)
        {
            //string fullpath = GetMapPath(_filepath);
            return File.Exists(filepath);
        }

        #endregion
    }

    /// <summary>
    /// 文件扩展名枚举
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        COM = 7790,
        EXE = 7790,
        DLL = 7790,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        JS = 119105,
        TXT = 210187,
        SQL = 255254,
        BAT = 64101,
        BTSEED = 10056,
        RDP = 255254,
        PSD = 5666,
        PDF = 3780,
        CHM = 7384,
        LOG = 70105,
        REG = 8269,
        HLP = 6395,
        DOC = 208207,
        XLS = 208207,
        DOCX = 208207,
        XLSX = 208207,
    }
}
