using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gaming.Quiz.DataInitializer.Datapopulation
{
    public class Datapopulation
    {
        public static DataSet InitializeQuestionSet(DataSet ds, out Int32 retVal)
        {
            DataSet newDs = new DataSet();

            DataTable dtCloned = ds.Tables[0].Clone();
            dtCloned.Columns[24].DataType = typeof(string);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dtCloned.ImportRow(row);
            }

            try
            {

                if (dtCloned != null && dtCloned.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCloned.Rows)
                    {
                        dr["date"] = Convert.ToDateTime(dr["date"]).ToString("yyyyMMdd");
                    }
                }

                newDs.Tables.Add(dtCloned);

                retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message);
            }
            return newDs;
        }

    }
}
