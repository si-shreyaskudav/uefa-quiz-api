using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Leaderboard;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.DataInitializer.Leaderboard
{
    public class Leaderboard
    {

        public static ResponseObject InitializerGetMonth(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<Month> monthDet = new List<Month>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            monthDet = (from a in ds.Tables[0].AsEnumerable()
                                        select new Month
                                        {
                                            MonthId = Convert.IsDBNull(a["qz_month_id"]) ? "" : a["qz_month_id"].ToString(),
                                            Month_Desc = Convert.IsDBNull(a["month_desc"]) ? "" : a["month_desc"].ToString()
                                        }).ToList();
                        }



                        retVal = 1;
                       
                        data.Value = monthDet;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGetMonth: " + ex.Message);
            }

            return data;

        }

        public static ResponseObject InitializerGetWeek(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<Week> weekDet = new List<Week>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            weekDet = (from a in ds.Tables[0].AsEnumerable()
                                        select new Week
                                        {
                                            WeekId = Convert.IsDBNull(a["qz_week_id"]) ? "" : a["qz_week_id"].ToString(),
                                            Week_Desc = Convert.IsDBNull(a["week_desc"]) ? "" : a["week_desc"].ToString()
                                        }).ToList();
                        }



                        retVal = 1;

                        data.Value = weekDet;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGetMonth: " + ex.Message);
            }

            return data;

        }

        public static ResponseObject InitializerGetGamedays(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<Gameday> GamedayDetails = new List<Gameday>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            GamedayDetails = (from a in ds.Tables[0].AsEnumerable()
                                        select new Gameday
                                        {
                                            GamedayId = Convert.IsDBNull(a["gamedayid"]) ? "" : a["gamedayid"].ToString(),
                                            Gameday_Desc = Convert.IsDBNull(a["gameday"]) ? "" : a["gameday"].ToString(),
                                            Gameday_Date = Convert.IsDBNull(a["gameday_date"]) ? "" : a["gameday_date"].ToString()
                                        }).ToList();
                        }



                        retVal = 1;

                        data.Value = GamedayDetails;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGetGamedays: " + ex.Message);
            }

            return data;

        }

        public static ResponseObject InitializerGetFavPlayers(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<FavPlayer> FavPlayerDetails = new List<FavPlayer>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            FavPlayerDetails = (from a in ds.Tables[0].AsEnumerable()
                                              select new FavPlayer
                                              {
                                                  PlayerId = Convert.IsDBNull(a["qz_playerid"]) ? "" : a["qz_playerid"].ToString(),
                                                  PlayerName = Convert.IsDBNull(a["player_name"]) ? "" : a["player_name"].ToString(),
                                                  IsActive = Convert.IsDBNull(a["is_active"]) ? "" : a["is_active"].ToString()
                                              }).ToList();
                        }



                        retVal = 1;

                        data.Value = FavPlayerDetails;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGetFavPlayers: " + ex.Message);
            }

            return data;

        }

        public static ResponseObject InitializerGetUserRank(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<UserRank> userRank = new List<UserRank>();
            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            userRank = (from a in ds.Tables[0].AsEnumerable()
                                            select new UserRank
                                            {
                                                UserId = Convert.IsDBNull(a["qz_userid"]) ? "" : a["qz_userid"].ToString(),
                                                UserName = Convert.IsDBNull(a["user_name"]) ? "" : a["user_name"].ToString(),
                                                Cur_Rno = Convert.IsDBNull(a["cur_rno"]) ? "" : a["cur_rno"].ToString(),
                                                Points = Convert.IsDBNull(a["points"]) ? "" : a["points"].ToString(),
                                                Rank = Convert.IsDBNull(a["rank"]) ? "" : a["rank"].ToString(),
                                                GUID = Convert.IsDBNull(a["guid"]) ? "" : a["guid"].ToString(),
                                                TotalAttempts = Convert.IsDBNull(a["total_attempts"]) ? 0 : Convert.ToInt32(a["total_attempts"]),
                                                Trend = Convert.IsDBNull(a["trend"]) ? 0 : Convert.ToInt32(a["trend"])

                                            }).ToList();
                        }


                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            retVal = Convert.IsDBNull(ds.Tables[1].Rows[0]["ret_type"]) ? 0 : Convert.ToInt64(ds.Tables[1].Rows[0]["ret_type"]);
                        }

                        data.Value = userRank;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGetUserRank: " + ex.Message);
            }

            return data;

        }

        public static ResponseObject InitializerGenLeaderboard(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<GenLeaderbaord> userRank = new List<GenLeaderbaord>();
            Int32 totalMembers = default(Int32);

            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            totalMembers = Convert.IsDBNull(ds.Tables[1].Rows[0]["total_member"]) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["total_member"]);
                            retVal = Convert.IsDBNull(ds.Tables[1].Rows[0]["ret_type"]) ? 0 : Convert.ToInt64(ds.Tables[1].Rows[0]["ret_type"]);
                        }

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            userRank = (from a in ds.Tables[0].AsEnumerable()
                                        select new GenLeaderbaord
                                        {
                                            UserId = Convert.IsDBNull(a["qz_userid"]) ? "" : a["qz_userid"].ToString(),
                                            UserName = Convert.IsDBNull(a["user_name"]) ? "" :a["user_name"].ToString(),
                                            Cur_Rno = Convert.IsDBNull(a["cur_rno"]) ? "" : a["cur_rno"].ToString(),
                                            Points = Convert.IsDBNull(a["points"]) ? "" : a["points"].ToString(),
                                            Rank = Convert.IsDBNull(a["rank"]) ? "" : a["rank"].ToString(),
                                            GUID = Convert.IsDBNull(a["guid"]) ? "" : a["guid"].ToString(),
                                            TotalAttempts = Convert.IsDBNull(a["total_attempts"]) ? 0 : Convert.ToInt32(a["total_attempts"]),
                                            Trend = Convert.IsDBNull(a["trend"]) ? 0 : Convert.ToInt32(a["trend"]),
                                            TotalMember = totalMembers
                                        }).ToList();
                        }

                        data.Value = userRank;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGenLeaderboard: " + ex.Message + GenericFunctions.DebugTable(ds.Tables[0])+"====="+ GenericFunctions.DebugTable(ds.Tables[1]));
            }

            return data;

        }

        public static ResponseObject InitializerGenPlayerLeaderboard(DataSet ds, ref Int64 retVal)
        {
            ResponseObject data = new ResponseObject();

            List<GenPlayerLeaderboard> userRank = new List<GenPlayerLeaderboard>();
            Int32 totalMembers = default(Int32);

            retVal = -70;

            try
            {
                if (ds != null)
                {
                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            totalMembers = Convert.IsDBNull(ds.Tables[1].Rows[0]["total_member"]) ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["total_member"]);
                            retVal = Convert.IsDBNull(ds.Tables[1].Rows[0]["ret_type"]) ? 0 : Convert.ToInt64(ds.Tables[1].Rows[0]["ret_type"]);
                        }

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            userRank = (from a in ds.Tables[0].AsEnumerable()
                                        select new GenPlayerLeaderboard
                                        {
                                            PlayerId = Convert.IsDBNull(a["qz_playerid"]) ? "" : a["qz_playerid"].ToString(),
                                            PlayerName = Convert.IsDBNull(a["player_name"]) ? "" : a["player_name"].ToString(),
                                            Cur_Rno = Convert.IsDBNull(a["cur_rno"]) ? "" : a["cur_rno"].ToString(),
                                            Points = Convert.IsDBNull(a["points"]) ? "" : a["points"].ToString(),
                                            Rank = Convert.IsDBNull(a["rank"]) ? "" : a["rank"].ToString(),
                                            GUID = Convert.IsDBNull(a["guid"]) ? "" : a["guid"].ToString(),
                                            TotalAttempts = Convert.IsDBNull(a["total_attempts"]) ? 0 : Convert.ToInt64(a["total_attempts"]),
                                            Trend = Convert.IsDBNull(a["trend"]) ? 0 : Convert.ToInt32(a["trend"]),
                                            TotalMember = totalMembers
                                        }).ToList();
                        }

                        data.Value = userRank;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("DataInitializer.Leaderboard.Leaderboards.InitializerGenPlayerLeaderboard: " + ex.Message + GenericFunctions.DebugTable(ds.Tables[0]) + "=====" + GenericFunctions.DebugTable(ds.Tables[1]));
            }

            return data;

        }

    }
}
