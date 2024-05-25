using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace ExcelToCsvConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string excelFilePath = "cac.xls"; // Provide the path to your Excel file
            string csvOutputFile = "output.csv"; // Specify the output CSV file name

            try
            {
                ConvertExcelToCsv(excelFilePath, csvOutputFile);
                Console.WriteLine("Conversion completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ConvertExcelToCsv(string excelFilePath, string csvOutputFile, int worksheetNumber = 1)
        {
            if (!File.Exists(excelFilePath))
                throw new FileNotFoundException(excelFilePath);

            if (File.Exists(csvOutputFile))
                throw new ArgumentException("File exists: " + csvOutputFile);

            // Connection string for Excel
            var cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"", excelFilePath);
            var cnn = new OleDbConnection(cnnStr);

            // Get schema and data
            var dt = new DataTable();
            try
            {
                cnn.Open();
                var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (schemaTable.Rows.Count < worksheetNumber)
                    throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");

                string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
                string sql = String.Format("select * from [{0}]", worksheet);

                var da = new OleDbDataAdapter(sql, cnn);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                // Handle exceptions
                throw e;
            }
            finally
            {
                // Free resources
                cnn.Close();
            }

            // Write out CSV data
            using (var wtr = new StreamWriter(csvOutputFile))
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool firstLine = true;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!firstLine)
                            wtr.Write(",");
                        else
                            firstLine = false;

                        var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
                        wtr.Write(String.Format("\"{0}\"", data));
                    }
                    wtr.WriteLine();
                }
            }
        }
    }
}


