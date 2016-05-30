using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.PorterHandler
{
    class ExcelFileManager
    {
        public static List<String> ReadColumnInExcelFile(String filePath, String workSheetName, String columnUserString, String columnExcelString)
        {
            try
            {
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", filePath);
                var adapter = new OleDbDataAdapter("SELECT * FROM [" + workSheetName + "$]", connectionString);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, columnExcelString);
                var data = dataSet.Tables[columnExcelString].AsEnumerable().AsEnumerable();
                var words = (from item in data
                             where item.Table.TableName == columnExcelString
                             select item.Field<String>(columnUserString).ToString()).ToList();
                return words;
            }
            catch (Exception)
            {
                throw new Exception("Can't Parse Excel File Correctly.");
            }
        }

        public static List<String> ReadColumnInExcelFile(String filePath, String workSheetName, String columnExcelString)
        {
            try
            {
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", filePath);
                var adapter = new OleDbDataAdapter("SELECT * FROM [" + workSheetName + "$]", connectionString);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, columnExcelString);
                var data = dataSet.Tables[columnExcelString].AsEnumerable().AsEnumerable();
                var words = (from item in data
                             where item.Table.TableName == columnExcelString
                             select item[0].ToString()).ToList();
                return words;
            }
            catch (Exception)
            {
                throw new Exception("Can't Parse Excel File Correctly.");
            }
        }


        public static void WriteToColumnInExcelFile(String filePath, String workSheetName, String columnExcelString, String columnUserString, List<String> words)
        {
            try
            {
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", filePath);
                var myConnection = new OleDbConnection(connectionString); 
                var adapter = new OleDbDataAdapter("SELECT * FROM [" + workSheetName + "$]", connectionString);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, columnExcelString);
                for (int i = 0; i < words.Count; i++)
                {
                    myConnection.Open();
                    adapter.InsertCommand = new System.Data.OleDb.OleDbCommand("insert into [Sheet1$] ([" + columnUserString  + "]) values('" + words[i] + "')", myConnection);
                    adapter.InsertCommand.ExecuteNonQuery();
                    adapter.Update(dataSet, columnExcelString);
                    myConnection.Close(); 
                }
            }
            catch (Exception)
            {
                throw new Exception("Can't Manipulate Excel File Correctly.");
            }
        }
    }
}
