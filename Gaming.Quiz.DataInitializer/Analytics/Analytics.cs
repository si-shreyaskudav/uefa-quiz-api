using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Amazon.EC2.Model;
using Gaming.Quiz.Library.Utility;

namespace Gaming.Quiz.DataInitializer.Analytics
{
    public class Analytics
    {
        public static Contracts.Analytics.Analytics InitializeAnalyticsGet(DataSet ds, out Int32 retVal)
        {
            retVal = -70;
            Contracts.Analytics.Analytics vAnalytics = new Contracts.Analytics.Analytics();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vAnalytics.OverallStats = (from a in ds.Tables[0].AsEnumerable()
                                                       select new Contracts.Analytics.OverallStats
                                                       {
                                                           Stat_Name = Convert.IsDBNull(a["Stat Name"]) ? "" : a["Stat Name"].ToString(),
                                                           Stat_Count = Convert.IsDBNull(a["Stat Count"]) ? "" : a["Stat Count"].ToString()
                                                       }).ToList();
                        }

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            vAnalytics.DayStats = (from a in ds.Tables[1].AsEnumerable()
                                                   select new Contracts.Analytics.DayStats
                                                   {
                                                       GameDate = Convert.IsDBNull(a["Game Date"]) ? "" : Convert.ToDateTime(a["Game Date"]).ToShortDateString(),
                                                       Attempt_1 = Convert.IsDBNull(a["Attempt (1)"]) ? "" : a["Attempt (1)"].ToString(),
                                                       Attempt_2 = Convert.IsDBNull(a["Attempt (2)"]) ? "" : a["Attempt (2)"].ToString(),
                                                       Attempt_3 = Convert.IsDBNull(a["Attempt (3)"]) ? "" : a["Attempt (3)"].ToString(),
                                                       ActiveUsers = Convert.IsDBNull(a["Active User"]) ? "" : a["Active User"].ToString(),
                                                       Total_Attempts = Convert.IsDBNull(a["Total Attempts"]) ? "" : a["Total Attempts"].ToString(),
                                                       Unique_User = Convert.IsDBNull(a["Unique User"]) ? "" : a["Unique User"].ToString(),
                                                       Recurring_User = Convert.IsDBNull(a["Recurring User"]) ? "" : a["Recurring User"].ToString()
                                                   }).ToList();


                        }
                    }
                }

                if (vAnalytics.OverallStats != null || vAnalytics.DayStats != null)
                    retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message + "== DB output===> Table 1 " + GenericFunctions.DebugTable(ds.Tables[0]) + "==> Table 2 ==>" + GenericFunctions.DebugTable(ds.Tables[1]));
            }
            return vAnalytics;
        }

        public static Contracts.Analytics.AnalyticsNew InitializeAnalyticsGetNew(DataSet ds, out Int32 retVal)
        {
            retVal = -70;
            Contracts.Analytics.AnalyticsNew vAnalytics = new Contracts.Analytics.AnalyticsNew();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vAnalytics.OverallStats = (from a in ds.Tables[0].AsEnumerable()
                                                       select new Contracts.Analytics.OverallStatsNew
                                                       {
                                                           FromDate = Convert.IsDBNull(a["start_date"]) ? "" : a["start_date"].ToString(),

                                                           ToDate = Convert.IsDBNull(a["end_date"]) ? "" : a["end_date"].ToString(),
                                                           Total_Registrants = Convert.IsDBNull(a["total_registrants"]) ? "" : a["total_registrants"].ToString(),
                                                           Attempts_Played = Convert.IsDBNull(a["attempts_played"]) ? "" : a["attempts_played"].ToString(),
                                                           Incomplete_Attempts = Convert.IsDBNull(a["incomplete_attempts"]) ? "" : a["incomplete_attempts"].ToString()

                                                       }).ToList();
                        }

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            vAnalytics.UserStats = (from a in ds.Tables[1].AsEnumerable()
                                                    select new Contracts.Analytics.UserStats
                                                    {
                                                        UserId = Convert.IsDBNull(a["qz_userid"]) ? "" : a["qz_userid"].ToString(),
                                                        Name = Convert.IsDBNull(a["name"]) ? "" : a["name"].ToString(),
                                                        EmailId = Convert.IsDBNull(a["email_id"]) ? "" : a["email_id"].ToString() != "" ? BareEncryption.BaseDecrypt(a["email_id"].ToString()) : a["email_id"].ToString(),
                                                        RegisterDate = Convert.IsDBNull(a["time_of_registration"]) ? "" : a["time_of_registration"].ToString(),
                                                        Total_Points = Convert.IsDBNull(a["total_point_earned"]) ? "" : a["total_point_earned"].ToString(),
                                                        PlayedGames = Convert.IsDBNull(a["no_of_gamedays_played"]) ? "" : a["no_of_gamedays_played"].ToString(),
                                                        Total_Qes_Attempted = Convert.IsDBNull(a["total_questions_attempted"]) ? "" : a["total_questions_attempted"].ToString(),
                                                        Correct_Answers = Convert.IsDBNull(a["questions_answered_correctly"]) ? "" : a["questions_answered_correctly"].ToString(),
                                                        Longest_Streak = Convert.IsDBNull(a["longest_streak"]) ? "" : a["longest_streak"].ToString(),
                                                        Streak_Points = Convert.IsDBNull(a["streak_points_scored"]) ? "" : a["streak_points_scored"].ToString(),
                                                        Time_Bonus_Points = Convert.IsDBNull(a["time_bonus_points"]) ? "" : a["time_bonus_points"].ToString(),
                                                        Fastest_Time_Complete = Convert.IsDBNull(a["fastest_time_to_complete"]) ? "" : a["fastest_time_to_complete"].ToString(),
                                                        Monthly_Highest_Rank = Convert.IsDBNull(a["month_highest_rank"]) ? "" : a["month_highest_rank"].ToString(),
                                                        Daily_Highest_Rank = Convert.IsDBNull(a["gameday_highest_rank"]) ? "" : a["gameday_highest_rank"].ToString(),
                                                        Month_Rank = Convert.IsDBNull(a["month_rank"]) ? "" : a["month_rank"].ToString(),
                                                        Month = Convert.IsDBNull(a["month"]) ? "" : a["month"].ToString()

                                                    }).ToList();


                        }
                    }
                }

                if (vAnalytics.OverallStats != null || vAnalytics.UserStats != null)
                    retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message + "== DB output===> Table 1 " + GenericFunctions.DebugTable(ds.Tables[0]) + "==> Table 2 ==>" + GenericFunctions.DebugTable(ds.Tables[1]));
            }
            return vAnalytics;
        }

        public static Contracts.Analytics.QPLAnalytics InitializeQPLAnalyticsGet(DataSet ds, out Int32 retVal)
        {
            retVal = -70;
            Contracts.Analytics.QPLAnalytics vAnalytics = new Contracts.Analytics.QPLAnalytics();

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            vAnalytics.OverallQPLStats = (from a in ds.Tables[0].AsEnumerable()
                                                          select new Contracts.Analytics.OverallQPLStats
                                                          {
                                                              Date = Convert.IsDBNull(a["gameday_date"]) ? "" : a["gameday_date"].ToString(),

                                                              GamedayId = Convert.IsDBNull(a["gamedayid"]) ? "" : a["gamedayid"].ToString(),
                                                              Total_Registrants = Convert.IsDBNull(a["registered_unique_users"]) ? "" : a["registered_unique_users"].ToString(),
                                                              Attempts_Played = Convert.IsDBNull(a["users_who_made_an_attempts"]) ? "" : a["users_who_made_an_attempts"].ToString(),
                                                              Attempts_Played_Perday = Convert.IsDBNull(a["total_attempts_per_day"]) ? "" : a["total_attempts_per_day"].ToString(),
                                                              Lifeline_Used_Count = Convert.IsDBNull(a["lifeline_used_count"]) ? "" : a["lifeline_used_count"].ToString()

                                                          }).ToList();
                        }


                    }
                }

                if (vAnalytics.OverallQPLStats != null || vAnalytics.OverallQPLStats != null)
                    retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message + "== DB output===> Table 1 " + GenericFunctions.DebugTable(ds.Tables[0]));
            }
            return vAnalytics;
        }

    }
}
