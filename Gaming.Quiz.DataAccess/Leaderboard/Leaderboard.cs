using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace Gaming.Quiz.DataAccess.Leaderboard
{
   public class Leaderboard : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public Leaderboard(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }


        public ResponseObject GetMonth(Int64 OptType, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_month_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_curr_month_details" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_month_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetMonth(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetWeekFile(Int64 OptType, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_week_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_curr_week_details" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_week_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetWeek(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetGamedays(Int64 OptType, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_gameday_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_curr_gameday_details" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_gameday_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetGamedays(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetFavPlayer(Int64 OptType, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_quiz_player_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_player" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_player", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetFavPlayers(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetUserRank(Int64 OptType, Int64 UserId, Int64 SportsId, Int64 CtgId, Int64 QzMId, Int64 GamedayId, Int64 WeekId, Int64 MonthId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_rank_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetUserRank(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetUserTeamRank(Int64 OptType, Int64 UserId, Int64 SportsId, Int64 CtgId, Int64 QzMId, Int64 PlayerId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_player_rank_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_playerid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlayerId;
                        
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetUserRank(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }


        public ResponseObject GetBestScoreUserRank(Int64 OptType, Int64 UserId, Int64 SportsId, Int64 CtgId, Int64 QzMId, Int64 GamedayId, Int64 WeekId, Int64 MonthId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_best_score_rank_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CtgId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGetUserRank(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject MonthLeaderboard(Int32 OptType, Int64 GamedayId, Int64 WeekId, Int64 MonthId, Int64 SportsId, Int64 CategoryId, Int64 MasterId, Int64 pageNo, Int64 mTopNo, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_rank_top_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_top_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MasterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_page_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = pageNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_top_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mTopNo;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_top_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGenLeaderboard(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject TeamLeaderboard(Int32 OptType,Int64 PlayerId, Int64 GamedayId, Int64 WeekId, Int64 SportsId, Int64 CategoryId, Int64 MasterId, Int64 pageNo, Int64 mTopNo, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_player_rank_top_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_top_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MasterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_playerid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = PlayerId;
                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_page_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = pageNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_top_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mTopNo;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_top_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGenLeaderboard(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject BestScoreLeaderboard(Int32 OptType, Int64 GamedayId, Int64 WeekId, Int64 MonthId, Int64 SportsId, Int64 CategoryId, Int64 MasterId, Int64 pageNo, Int64 mTopNo, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_rank_best_score_top_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_top_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MasterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_page_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = pageNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_top_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mTopNo;
                      
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_top_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGenLeaderboard(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject OverallLeaderboard(Int32 OptType, Int64 SportsId, Int64 CategoryId, Int64 MasterId, Int64 pageNo, Int64 mTopNo, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            spName = "qz_user_player_overall_rank_top_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_top_rank", "p_cur_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MasterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_page_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = pageNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_top_no", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = mTopNo;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_top_rank", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_detail", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);

                        res = DataInitializer.Leaderboard.Leaderboard.InitializerGenPlayerLeaderboard(ds, ref retVal);

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

        public ResponseObject GetBadgesList(Int64 OptType, Int64 QzMId, ref HTTPMeta httpMeta)
        {
            ResponseObject res = new ResponseObject();
            Int64 retVal = -50;
            String spName = String.Empty;
            dynamic retJson = null;

            spName = "qz_quiz_badges_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaAdmin + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quizid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;


                        //NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        //mNpgsqlCmd.Parameters.Add(retJson);

                        //NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        //mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();
                        retJson = mNpgsqlCmd.ExecuteScalar();

                        //retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null)
                        {

                            res.Value = GenericFunctions.Deserialize<dynamic>(retJson.ToString());
                            res.FeedTime = GenericFunctions.GetFeedTime();
                            retVal = 1;
                        }

                        GenericFunctions.AssetMeta(retVal, ref httpMeta, spName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return res;
            }
        }

    }
}
