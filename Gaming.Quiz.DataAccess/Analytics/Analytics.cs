using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Gaming.Quiz.DataAccess.Analytics
{
    public class Analytics : Common.BaseDataAccess
    {
        IPostgre _Postgre;

        public Analytics(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }


        public Contracts.Analytics.Analytics GetAnalytics(string fromDate, string toDate, ref Int32 retVal)
        {
            Int32 OptType = 1;
            retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            Contracts.Analytics.Analytics vAnalytics = new Contracts.Analytics.Analytics();

            spName = "qz_admin_dash_analytics_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_cur_ovearll_stats", "p_cur_days_stats" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cf_tourid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = _AppSettings.Value.Properties.TourId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_from_date", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = fromDate;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_to_date", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = toDate;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_ovearll_stats", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_days_stats", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        vAnalytics = DataInitializer.Analytics.Analytics.InitializeAnalyticsGet(ds, out retVal);
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

                return vAnalytics;
            }
        }

        public Contracts.Analytics.AnalyticsNew GetAnalyticsNew(string fromDate, string toDate, ref Int32 retVal)
        {
            Int32 OptType = 1;
            retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            Contracts.Analytics.AnalyticsNew vAnalytics = new Contracts.Analytics.AnalyticsNew();

            spName = "qz_quiz_analytics_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_gd_overall_analytic", "p_user_analytic" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaAdmin + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = _AppSettings.Value.Properties.QuizMasterId;
                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_sportz_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = _AppSettings.Value.Properties.QuizSportsId;
                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_category_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = _AppSettings.Value.Properties.QuizCategoryId;
                        //  mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_gameday_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = gamedayId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_from_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = fromDate.ToDateTimeValue();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_to_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = toDate.ToDateTimeValue();

                        //mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_month_id", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = monthId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_gd_overall_analytic", NpgsqlDbType.Refcursor)).Value = cursors[0];
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_user_analytic", NpgsqlDbType.Refcursor)).Value = cursors[1];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        vAnalytics = DataInitializer.Analytics.Analytics.InitializeAnalyticsGetNew(ds, out retVal);
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

                return vAnalytics;
            }
        }

        public Contracts.Analytics.QPLAnalytics GetQPLAnalytics(string fromDate, string toDate, ref Int32 retVal)
        {
            Int32 OptType = 1;
            retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            Contracts.Analytics.QPLAnalytics vAnalytics = new Contracts.Analytics.QPLAnalytics();

            spName = "qz_admin_qpl_analytics_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_cur_user_data" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaAdmin + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quizid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = _AppSettings.Value.Properties.QuizMasterId;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_from_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = DBNull.Value;//fromDate.ToDateTimeValue();
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_to_date", NpgsqlDbType.Date) { Direction = ParameterDirection.Input }).Value = DBNull.Value;//toDate.ToDateTimeValue();

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_cur_user_data", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        vAnalytics = DataInitializer.Analytics.Analytics.InitializeQPLAnalyticsGet(ds, out retVal);
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

                return vAnalytics;
            }
        }

    }
}
