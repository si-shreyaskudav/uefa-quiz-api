using Gaming.Quiz.Contracts.Admin;
using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.DataInitializer.Admin
{
   public class AdminServices
    {

        public static List<vGamedayDetails> InitializeGamedayDetails(DataSet ds, out Int32 retVal)
        {
            List<vGamedayDetails> vGddet = new List<vGamedayDetails>();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vGddet = (from a in ds.Tables[0].AsEnumerable()
                                            select new vGamedayDetails
                                            {
                                                vGameday = Convert.IsDBNull(a["qz_gameday_id"]) ? "" : a["qz_gameday_id"].ToString(),
                                                vGamedayNo = Convert.IsDBNull(a["gameday_number"]) ? "" : a["gameday_number"].ToString(),
                                                vStartDate = Convert.IsDBNull(a["start_date"]) ? "" : a["start_date"].ToString(),
                                                vEndDate = Convert.IsDBNull(a["end_date"]) ? "" : a["end_date"].ToString(),
                                                vQuestionReady = Convert.IsDBNull(a["question_ready"]) ? "" : a["question_ready"].ToString(),
                                                vTotCtg = Convert.IsDBNull(a["total_category"]) ? "" : a["total_category"].ToString(),
                                                vTotSports = Convert.IsDBNull(a["total_sport"]) ? "" : a["total_sport"].ToString(),
                                                vQUpdDate = Convert.IsDBNull(a["qus_update_date"]) ? "" : a["qus_update_date"].ToString(),
                                                vQBnkPrsnt = Convert.IsDBNull(a["ques_bank_present"]) ? "" : a["ques_bank_present"].ToString(),
                                                vQsnCnt = Convert.IsDBNull(a["ques_cnt"]) ? "" : a["ques_cnt"].ToString(),
                                                vTotAttempt = Convert.IsDBNull(a["total_attempt"]) ? "" : a["total_attempt"].ToString()
                                            }).ToList();
                        }
                    }
                }
                retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message);
            }
            return vGddet;
        }

        public static List<vMonthDetails> InitializeMonthDetails(DataSet ds, out Int32 retVal)
        {
            List<vMonthDetails> vGddet = new List<vMonthDetails>();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vGddet = (from a in ds.Tables[0].AsEnumerable()
                                      select new vMonthDetails
                                      {
                                          vMonthId = Convert.IsDBNull(a["qz_month_id"]) ? "" : a["qz_month_id"].ToString(),
                                          vMonthNo = Convert.IsDBNull(a["month_number"]) ? "" : a["month_number"].ToString(),
                                          vMnStartDate = Convert.IsDBNull(a["month_startdate"]) ? "" : a["month_startdate"].ToString(),
                                          vMnEndDate = Convert.IsDBNull(a["month_enddate"]) ? "" : a["month_enddate"].ToString(),
                                          vIsCurrent = Convert.IsDBNull(a["is_current"]) ? "" : a["is_current"].ToString(),
                                          vIsLocked = Convert.IsDBNull(a["is_locked"]) ? "" : a["is_locked"].ToString(),
                                          vIsLeaderbordShow = Convert.IsDBNull(a["is_leaderboard_show"]) ? "" : a["is_leaderboard_show"].ToString(),
                                      }).ToList();
                        }
                    }
                }
                retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message);
            }
            return vGddet;
        }

        public static List<vAdminLLget> InitializeAdminLLGet(DataSet ds, out Int32 retVal)
        {
            List<vAdminLLget> vGddet = new List<vAdminLLget>();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vGddet = (from a in ds.Tables[0].AsEnumerable()
                                      select new vAdminLLget
                                      {
                                          vLifelineMid = Convert.IsDBNull(a["qz_lifeline_masterid"]) ? "" : a["qz_lifeline_masterid"].ToString(),
                                          vMid = Convert.IsDBNull(a["qz_quiz_masterid"]) ? "" : a["qz_quiz_masterid"].ToString(),
                                          vDesc = Convert.IsDBNull(a["lifeline_desc"]) ? "" : a["lifeline_desc"].ToString(),
                                          vShortDesc = Convert.IsDBNull(a["short_desc"]) ? "" : a["short_desc"].ToString(),
                                          vIsActive = Convert.IsDBNull(a["is_active"]) ? "" : a["is_active"].ToString()
                                      }).ToList();
                        }
                    }
                }
                retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message);
            }
            return vGddet;
        }

    }
}
