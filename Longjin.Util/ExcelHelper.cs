using Aspose.Cells;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System;

namespace Longjin.Util
{
    /// <summary>
    /// Excel 帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// EXCEL 链接字符串
        /// </summary>
        private static string strConnFormat = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties=Excel 8.0;";
        
        /// <summary>
        /// 读取Excel文件内容到DataSet
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oneRowColumnName">第一行是否作为列名</param>
        /// <returns></returns>
        public static DataSet ExcelReadDataSet(string path, bool oneRowColumnName = false)
        {
            var ds = new DataSet();
            string strConn = string.Format(strConnFormat, path);
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();
                var tabList = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow dr in tabList.Rows)
                {
                    var tableName = dr[2].ToString().Trim();
                    string sql = string.Format("select * from [{0}]", tableName);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, strConn))
                    {
                        dataAdapter.Fill(ds, tableName);
                        if (oneRowColumnName)
                        {
                            if (ds.Tables[tableName].Rows.Count > 0)
                            {
                                var oneRow = ds.Tables[tableName].Rows[0];
                                for (var i = 0; i < ds.Tables[tableName].Columns.Count; i++)
                                {
                                    var columnname = oneRow[i].ToString();
                                    if (!string.IsNullOrEmpty(columnname))
                                    {
                                        ds.Tables[tableName].Columns[i].ColumnName = columnname;
                                    }
                                }
                                ds.Tables[tableName].Rows[0].Delete();//删除标题
                            }
                        }
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 获取Excel的工作簿名称列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetExcelTableNameList(string path)
        {
            string strConn = string.Format(strConnFormat, path);
            var tableNames = new List<string>();
            if (File.Exists(path))
            {
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow dr in dt.Rows)
                    {
                        tableNames.Add(dr[2].ToString().Trim());
                    }
                }
            }
            return tableNames;
        }


        /// <summary>
        /// 把DataSet类型数据导出为Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName">如果需要保存为文件则传此名称</param>
        /// <returns></returns>
        public static MemoryStream ExportExcel(DataSet ds, string fileName = null)
        {
            Workbook workbook = new Workbook();
            for (var i = 0; i < ds.Tables.Count; i++)
            {
                var dt = ds.Tables[i];
                Worksheet sheet = workbook.Worksheets[i];
                sheet.Name = dt.TableName;
                var col_num = dt.Columns.Count;
                for (var col = 1; col <= col_num; col++)
                {
                    sheet.Cells[0, col].PutValue(dt.Columns[col - 1].ColumnName);
                }
                var row_num = dt.Rows.Count;
                for (var row = 1; row <= row_num; row++)
                {
                    for (var col = 1; col <= col_num; col++)
                    {
                        var value = dt.Rows[row - 1][col - 1];
                        switch (value.GetType().Name)
                        {
                            case "DateTime":
                                var date = value.ToDateTime();
                                if (date.Second != 0)
                                {
                                    value = date.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (date.Minute != 0)
                                {
                                    value = date.ToString("yyyy-MM-dd HH:mm");
                                }
                                else if (date.Hour != 0)
                                {
                                    value = date.ToString("yyyy-MM-dd HH");
                                }
                                else
                                {
                                    value = date.ToString("yyyy-MM-dd");
                                }
                                break;
                            case "Int16":
                            case "Int32":
                            case "Byte":
                            case "Decimal":
                            case "Double":
                            case "Float":
                            case "Single":
                                var single = value.ToString().ToDecimal();
                                value = single;
                                break;
                        }
                        sheet.Cells[row, col].PutValue(value);
                    }
                }
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                workbook.Save(fileName);
            }
            return workbook.SaveToStream();
        }
    }
}
