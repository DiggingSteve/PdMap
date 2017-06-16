using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Longjin.Util;

namespace UnitTest
{
    [TestClass]
    public class DbTest
    {
        [TestMethod]
        public void CreateDbCon()
        {
            var database = "pdmap";
            var uid = "sa";
            var pwd = "123456";
            var server = ".";
            var str = string.Format("XpoProvider=MSSqlServer;data source={0};user id={1};password={2};initial catalog={3};Persist Security Info=true", server, uid, pwd, database);
            str = SecureUtil.EncryptStringByDES(str);
            Console.WriteLine(str);
            str = SecureUtil.EncryptStringByDES("server=192.168.10.127;database=pdmap;uid=sa;pwd=123456;");
            Console.WriteLine(str);

        }
    }
}
