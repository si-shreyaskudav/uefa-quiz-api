using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gaming.Quiz.Library.Documents
{
    public class Excel
    {
        public static byte[] Export(DataSet ds)
        {
            byte[] fileContents = null;

            try
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    using (var package = new ExcelPackage())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(dt.TableName);
                            workSheet.Cells["A1"].LoadFromDataTable(dt, true);
                        }

                        fileContents = package.GetAsByteArray();
                    }
                }
            }
            catch (Exception ex) { fileContents = null; }

            return fileContents;
        }
    }
}
