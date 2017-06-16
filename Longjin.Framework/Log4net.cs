using System;
using log4net;
using System.Web;

namespace Longjin.Framework
{
    public class Log4net
    {
        private static object _lock = new object();

        private static ILog _infoLogger = null;
        private static ILog infoLogger
        {
            get
            {
                if (_infoLogger == null)
                {
                    lock (_lock)
                    {
                        if (_infoLogger == null)
                        {
                            _infoLogger = LogManager.GetLogger("InfoLogger");
                        }
                    }
                }
                return _infoLogger;
            }
        }

        private static ILog _fatalLogger = null;
        private static ILog fatalLogger
        {
            get
            {
                if (_fatalLogger == null)
                {
                    lock (_lock)
                    {
                        if (_fatalLogger == null)
                        {
                            _fatalLogger = LogManager.GetLogger("FatalLogger");
                        }
                    }
                }
                return _fatalLogger;
            }
        }

        private static ILog _debugLogger = null;
        private static ILog debugLogger
        {
            get
            {
                if (_debugLogger == null)
                {
                    lock (_lock)
                    {
                        if (_debugLogger == null)
                        {
                            _debugLogger = LogManager.GetLogger("DebugLogger");
                        }
                    }
                }
                return _debugLogger;
            }
        }

        private static string Ip
        {
            get
            {
                try
                {
                    return HttpHelper.GetIP();
                }
                catch (Exception ex)
                {
                    return "IP";
                }
            }
        }

        private static string RequestType
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.RequestType;
                }
                catch (Exception ex)
                {
                    return "RequestType";
                }
            }
        }

        private static string Url
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.Url.AbsolutePath;
                }
                catch (Exception ex)
                {
                    return "URL";
                }
            }
        }

        private static string UserAgent
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.UserAgent;
                }
                catch (Exception ex)
                {
                    return "UserAgent";
                }
            }
        }

        /// <summary>
        /// 获取日志记录的完整前缀信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static string GetLogCompleteHeaders(string msg)
        {
            return string.Format("[{0}] \"{1} {2}\" {3} \"{4}\"", Ip, RequestType, Url, msg, UserAgent);
        }

        /// <summary>
        /// 获取日志记录的基础前缀信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static string GetLogBasicHeaders(string msg)
        {
            return string.Format("[{0}] \"{1} {2}\" {3}", Ip, RequestType, Url, msg);
        }

        /// <summary>
        /// 程序运行时执行
        /// </summary>
        /// <param name="log4netConfigPath">log4net.config文件的绝对路径</param>
        public static void Application_Start(string log4netConfigPath)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(log4netConfigPath));
        }

        #region Info 记录信息

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            infoLogger.Info(msg);
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Info(string msg, Exception ex)
        {
            infoLogger.Info(msg, ex);
        }

        #endregion

        #region Fatal 致命错误

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Fatal(string msg)
        {
            fatalLogger.Fatal(GetLogCompleteHeaders(msg));
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="ex"></param>
        public static void Fatal(Exception ex)
        {
            fatalLogger.Fatal(ex);
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Fatal(string msg, Exception ex)
        {
            fatalLogger.Fatal(GetLogCompleteHeaders(msg), ex);
        }
        #endregion

        #region Debug 调试 记录程序运行和终止运行情况

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            debugLogger.Debug(msg);
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Debug(string msg, Exception ex)
        {
            debugLogger.Debug(GetLogCompleteHeaders(msg), ex);
        }
        #endregion

        /// <summary>
        /// 公共日志记录方法 会根据异常类型判断 存放到那个级别
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Write(string msg, Exception ex)
        {
            if (ex.InnerException != null)
            {
                Write(msg, ex.InnerException);
            }
            switch (ex.GetType().Name)
            {
                case "AuthException":
                    infoLogger.Info(string.Format("{0} \"{1}\" {2}", GetLogBasicHeaders(msg), ex.Message, SerializeData(ex)));
                    break;
                case "ArgumentException":
                case "ParamException":
                    var argument = ex as ArgumentException;
                    if (argument != null)
                    {
                        infoLogger.Info(string.Format("{0} \"{1}\" \"{2}\" {3}", GetLogBasicHeaders(msg), argument.ParamName, ex.Message.Replace("\r\n", " "), SerializeData(ex)));
                    }
                    else
                    {
                        fatalLogger.Fatal(GetLogCompleteHeaders(msg), ex);
                    }
                    break;
                case "ExecuteException":
                default:
                    fatalLogger.Fatal(GetLogCompleteHeaders(msg), ex);
                    break;
            }
        }

        /// <summary>
        /// 序列化ex.Data
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string SerializeData(Exception ex)
        {
            if (ex.Data != null && ex.Data.Count > 0)
            {
                return string.Format("[{0}]", Newtonsoft.Json.JsonConvert.SerializeObject(ex.Data));
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
