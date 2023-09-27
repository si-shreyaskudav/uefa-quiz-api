using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.Library.Utility
{
   public  class Excel
    {
        public static void newExtarctExcel()
        {
            //create a list to hold all the values
            List<string> excelData = new List<string>();

            //read the Excel file as byte array
            byte[] bin = File.ReadAllBytes(@"D:\Office\Quizz\DataImport2.xlsx");

            //or if you use asp.net, get the relative path
            //byte[] bin = File.ReadAllBytes(Server.MapPath("ExcelDemo.xlsx"));

            //create a new Excel package in a memorystream
            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                    {
                        //loop all columns in a row
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            //add the cell data to the List
                            if (worksheet.Cells[i, j].Value != null)
                            {
                                excelData.Add(worksheet.Cells[i, j].Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        public static DataTable ExcelPackageToDataTable(string path)
        {
            DataTable dt = new DataTable();
            FileInfo file = new FileInfo(@path);
            ExcelPackage excelPackage = new ExcelPackage();
            //create a list to hold all the values
            List<string> excelData = new List<string>();

            //read the Excel file as byte array
            //byte[] bin = File.ReadAllBytes(@path);

            //using (MemoryStream stream = new MemoryStream(bin))

            using (excelPackage = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];


                Int32 rowCount = worksheet.Dimension.End.Row;
                Int32 colCount = worksheet.Dimension.End.Column;

                for (int i = 2; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (worksheet.Cells[i, j].Value == null)
                        {
                            worksheet.Cells[i, j].Value = "-";
                        }
                    }
                }

                excelPackage.Save();
                //check if the worksheet is completely empty
                if (worksheet.Dimension == null)
                {
                    return dt;
                }


                //create a list to hold the column names
                List<string> columnNames = new List<string>();

                //needed to keep track of empty column headers
                int currentColumn = 1;

                //loop all columns in the sheet and add them to the datatable
                foreach (var cell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    string columnName = cell.Text.Trim();

                    //check if the previous header was empty and add it if it was
                    if (cell.Start.Column != currentColumn)
                    {
                        columnNames.Add("Header_" + currentColumn);
                        dt.Columns.Add("Header_" + currentColumn);
                        currentColumn++;
                    }

                    //add the column name to the list to count the duplicates
                    columnNames.Add(columnName);

                    //count the duplicate column names and make them unique to avoid the exception
                    //A column named 'Name' already belongs to this DataTable
                    int occurrences = columnNames.Count(x => x.Equals(columnName));
                    if (occurrences > 1)
                    {
                        columnName = columnName + "_" + occurrences;
                    }

                    //add the column to the datatable
                    dt.Columns.Add(columnName);

                    currentColumn++;
                }

                //start adding the contents of the excel file to the datatable
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {

                    var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
                    DataRow newRow = dt.NewRow();

                    //loop all cells in the row
                    foreach (var cell in row)
                    {
                        if (cell.Value.ToString() !="-")
                            newRow[cell.Start.Column - 1] = cell.Value;
                        else
                            newRow[cell.Start.Column - 1] = "";
                    }

                    dt.Rows.Add(newRow);
                }
            }
            return dt;
        }
    }
}
