using Gaming.Quiz.Contracts.PointCalculation;
using Gaming.Quiz.Contracts.Leaderboard;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.DataInitializer.PointCalculation
{
    public class PointCalculation
    {

        public static List<PntPrcGet> InitializerGetUserPointProcess(DataSet ds, ref Int32 retVal)
        {
            List<PntPrcGet> ProcessGet = new List<PntPrcGet>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            retVal = Convert.IsDBNull(ds.Tables[0].Rows[0]["ret_type"]) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["ret_type"]);
                            if (retVal == 1)
                            {
                                ProcessGet = (from a in ds.Tables[0].AsEnumerable()
                                            select new PntPrcGet
                                            {
                                                QzMId = Convert.IsDBNull(a["qz_quiz_masterid"]) ? 0 : Convert.ToInt32(a["qz_quiz_masterid"]),
                                                MonthId = Convert.IsDBNull(a["month_id"]) ? 0 : Convert.ToInt32(a["month_id"]),
                                                WeekId = Convert.IsDBNull(a["week_id"]) ? 0 : Convert.ToInt32(a["week_id"]),
                                                GamedayId = Convert.IsDBNull(a["gameday_id"]) ? 0 : Convert.ToInt32(a["gameday_id"]),
                                                SettleProcessFlag = Convert.IsDBNull(a["settle_process_flag"]) ? "0" :a["settle_process_flag"].ToString(),
                                                retVal = Convert.IsDBNull(a["ret_type"]) ? 0 : Convert.ToInt32(a["ret_type"])
                                            }).ToList();
                            }
                        }

                        retVal = 1;
                       
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.PointCalculation.PointCalculation.InitializerGetUserPointProcess: " + ex.Message);
            }
            return ProcessGet;
        }

        public static List<string> InitializerGetPendingDays(DataSet ds, ref Int32 retVal)
        {
            List<string> PendingDays = new List<string>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                                PendingDays = ds.Tables[0].AsEnumerable()
                                               .Select(r => r.Field<string>("game_date"))
                                               .ToList();
                        }

                        retVal = 1;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.BGService.GetPendingDays.InitializerGetPendingDays: " + ex.Message);
            }
            return PendingDays;
        }

    }
}
