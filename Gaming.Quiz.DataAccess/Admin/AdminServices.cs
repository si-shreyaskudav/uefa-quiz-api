using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.DataInitializer.Admin;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using Gaming.Quiz.Contracts.Admin;

namespace Gaming.Quiz.DataAccess.Admin
{
    public class AdminServices : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public AdminServices(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public Int32 AdminLLGet(Int64 OptType, Int64 QMId, ref List<vAdminLLget> CurOutput)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            ResponseObject QuestionSet = new ResponseObject();

            spName = "qz_admin_lifline_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_curr_question" };
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_lifeline", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        CurOutput = DataInitializer.Admin.AdminServices.InitializeAdminLLGet(ds, out retVal);
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

        public Int32 QuizMappingProcess(Int64 OptType, DateTime date, Int64 QzMId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_admin_quiz_ques_day_process";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = date;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;

                        NpgsqlParameter value = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(value);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = value.Value != null && value.Value.ToString().Trim() != "" ? Int32.Parse(value.Value.ToString()) : retVal;
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

        public Int32 GameDetailsGet(Int64 OptType, Int64 QzMId, ref List<vGamedayDetails> CurOutput)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            ResponseObject QuestionSet = new ResponseObject();

            spName = "qz_admin_gameday_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_curr_gameday_details" };
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_gameday_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        CurOutput = DataInitializer.Admin.AdminServices.InitializeGamedayDetails(ds, out retVal);

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

        public Int32 MonthDetailsGet(Int64 OptType, Int64 QzMId, ref List<vMonthDetails> CurOutput)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            ResponseObject QuestionSet = new ResponseObject();

            spName = "qz_admin_month_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_curr_month_details" };
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_month_details", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        CurOutput = DataInitializer.Admin.AdminServices.InitializeMonthDetails(ds, out retVal);

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
