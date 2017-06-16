using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;

namespace Longjin.Util
{
    /*
    1)直接执行sql语句
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetSqlStringCommond("insert t1 (id)values('haha')");
        db.ExecuteNonQuery(cmd);
     * 
    2)执行存储过程
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetStoredProcCommond("t1_insert");
        db.AddInParameter(cmd, "@id", DbType.String, "heihei");
        db.ExecuteNonQuery(cmd);
     * 
    3)返回DataSet
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetSqlStringCommond("select * from t1");或者DbCommand cmd = db.GetStoredProcCommond("t1_findall");
        DataSet ds = db.ExecuteDataSet(cmd);
     * 
    4)返回DataTable
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetSqlStringCommond("t1_findall");
        DataTable dt = db.ExecuteDataTable(cmd);
     * 
    5)输入参数/输出参数/返回值的使用(比较重要哦)
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetStoredProcCommond("t2_insert");
        db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
        db.AddOutParameter(cmd, "@outString", DbType.String, 20);
        db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);

        db.ExecuteNonQuery(cmd);

        string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
        int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value
     * 
    6)DataReader使用
        DbHelper db = new DbHelper();
        DbCommand cmd = db.GetStoredProcCommond("t2_insert");
        db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
        db.AddOutParameter(cmd, "@outString", DbType.String, 20);
        db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);

        using (DbDataReader reader = db.ExecuteReader(cmd))
        {
            dt.Load(reader);
        }        
        string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
        int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value

    7)事务的使用.(项目中需要将基本的数据库操作组合成一个完整的业务流时,代码级的事务是必不可少的哦)
        pubic void DoBusiness()
        {
            using (Trans t = new Trans())
            {
                try
                {
                    D1(t);
                    throw new Exception();//如果有异常,会回滚滴
                    D2(t);
                    t.Commit();
                }
                catch
                {
                    t.RollBack();
                }
            }
        }
        public void D1(Trans t)
        {
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetStoredProcCommond("t2_insert");
            db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
            db.AddOutParameter(cmd, "@outString", DbType.String, 20);
            db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);

            if (t == null) db.ExecuteNonQuery(cmd);
            else db.ExecuteNonQuery(cmd,t);

            string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
            int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value
        }
        public void D2(Trans t)
        {
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetSqlStringCommond("insert t1 (id)values('..')");        
            if (t == null) db.ExecuteNonQuery(cmd);
            else db.ExecuteNonQuery(cmd, t);
        }
    */
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DbHelper
    {
        private static string dbProviderName = ConfigurationManager.AppSettings["DbHelperProvider"];
        private static string dbConnectionString = ConfigurationManager.AppSettings["DbHelperConnectionString"];

        private DbConnection connection;

        /// <summary>
        /// 
        /// </summary>
        public DbHelper()
        {
            this.connection = CreateConnection(DbHelper.dbConnectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DbHelper(string connectionString)
        {
            this.connection = CreateConnection(connectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbConnection CreateConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = DbHelper.dbConnectionString;
            return dbconn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection CreateConnection(string connectionString)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = connectionString;
            return dbconn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns></returns>
        public DbCommand GetStoredProcCommond(string storedProcedure)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = sqlQuery;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        #region 增加参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dbParameterCollection"></param>
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }

        #endregion

        #region 执行
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            cmd.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand cmd)
        {
            cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand cmd)
        {
            cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
        #endregion

        #region 执行事务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            return reader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            object ret = cmd.ExecuteScalar();
            return ret;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Trans : IDisposable
    {
        private DbConnection conn;
        private DbTransaction dbTrans;
        /// <summary>
        /// 
        /// </summary>
        public DbConnection DbConnection
        {
            get { return this.conn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DbTransaction DbTrans
        {
            get { return this.dbTrans; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Trans()
        {
            conn = DbHelper.CreateConnection();
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public Trans(string connectionString)
        {
            conn = DbHelper.CreateConnection(connectionString);
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            dbTrans.Commit();
            this.Colse();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RollBack()
        {
            dbTrans.Rollback();
            this.Colse();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Colse();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Colse()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }


    /// <summary>
    /// 数据库操作类
    /// </summary>
    public sealed class SQLServerDAL
    {
        private SQLServerDAL() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int ExecuteSql(string conn, string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string conn, string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, connection))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            return dt;
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            throw new Exception(E.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int GetCount(string conn, string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    int i = 0;
                    while (result.Read())
                    {
                        if (!result.IsDBNull(0))
                            i = result.GetInt32(0);
                    }
                    result.Close();
                    return i;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static double GetDouble(string conn, string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    double i = 0;
                    while (result.Read())
                    {
                        if (!result.IsDBNull(0))
                            i = Convert.ToDouble(result[0]);
                    }
                    result.Close();
                    return i;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string conn, string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    decimal i = 0;
                    while (result.Read())
                    {
                        if (!result.IsDBNull(0))
                            i = Convert.ToDecimal(result[0]);//result.GetDecimal(0);
                    }
                    result.Close();
                    return i;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static long GetLong(string conn, string strSQL)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                try
                {
                    connection.Open();
                    SqlDataReader result = cmd.ExecuteReader();
                    long i = 0;
                    while (result.Read())
                    {
                        if (!result.IsDBNull(0))
                            i = result.GetInt64(0);
                    }
                    result.Close();
                    return i;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }


    }
}
