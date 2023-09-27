using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.PointCalculation;
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
using System.Threading.Tasks;

namespace Gaming.Quiz.DataAccess.PointCalculation
{
   public class PointCalculation : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public PointCalculation(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public  List<PntPrcGet> UserPointProcessGet(Int32 OptType, Int64 QzMId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            List<PntPrcGet> pntPrcGets = new List<PntPrcGet>();
            spName = "qz_point_process_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_details"};

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        pntPrcGets = DataInitializer.PointCalculation.PointCalculation.InitializerGetUserPointProcess(ds, ref retVal);
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

                return pntPrcGets;
            }
        }

        public List<PntPrcGet> UserEODSettlementGet(Int32 OptType, Int64 QzMId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            List<PntPrcGet> pntPrcGets = new List<PntPrcGet>();
            spName = "qz_end_day_settlement_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_cur_details" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        pntPrcGets = DataInitializer.PointCalculation.PointCalculation.InitializerGetUserPointProcess(ds, ref retVal);
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

                return pntPrcGets;
            }
        }

        public async Task<Int32> UserPointProcess(Int32 OptType, Int64 QzMId, Int32 MonthId, Int32 GamedayId, Int32 WeekId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_point_process";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int32.Parse(retvalue.Value.ToString()) : retVal;
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

                return retVal;
            }
        }

        public async Task<Int32> UserBestScoreUpd(Int32 OptType, Int64 UserId, Int32 MonthId, Int32 GamedayId, Int32 WeekId, Int64 QzMId, Int64 SportsId, Int64 CategoryId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_user_points_best_score_upd";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaRank + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = MonthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = GamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_week_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = WeekId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_userid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = UserId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;


                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int32.Parse(retvalue.Value.ToString()) : retVal;
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

                return retVal;
            }
        }

        public Int32 Settlement(Int32 OptType, Int64 QzMId, Int64 SportsId, Int64 CategoryId, ref SettlmentGet settlmentGet )
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            
            spName = "qz_user_settlement_end_day_process";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    //using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaRank + spName, connection))
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sport_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = SportsId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = CategoryId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        
                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);
                        NpgsqlParameter retJson = new NpgsqlParameter("p_ret_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        

                        if (connection.State != ConnectionState.Open) connection.Open();
                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int32.Parse(retvalue.Value.ToString()) : retVal;

                        if (retJson != null && retVal == 1)
                        {
                            settlmentGet = GenericFunctions.Deserialize<SettlmentGet>(retJson.Value.ToString());
                        }
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

                return retVal;
            }
        }
    }
}
