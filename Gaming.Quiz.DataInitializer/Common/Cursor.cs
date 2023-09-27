using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace  Gaming.Quiz.DataInitializer.Common
{
    public class Cursor
    {
        public static DataTable InitializeCursor(DataSet ds)
        {
            try
            {
                if (ds != null)
                    if (ds.Tables != null)
                        return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new DataTable();
        }

        public static DataTable InitializeCursor(NpgsqlCommand mNpgsqlCmd, List<String> cursors)
        {
            DataSet ds = null;
            try
            {
                ds = Utility.GetDataSet(mNpgsqlCmd, cursors);

                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new DataTable();
        }
    }
}
