using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gaming.Quiz.DataAccess.GamedayMapping
{
    public class GamedayMapping : Common.BaseDataAccess
    {
        IPostgre _Postgre;

        public GamedayMapping(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public List<Contracts.GamedayMapping.GamedayMapping> GetGamedayMappings(Int64 OptType, Int64 quizMaterId, ref Int32 retVal)
        {
            retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;
            List<Contracts.GamedayMapping.GamedayMapping> vGamedayMappings = new List<Contracts.GamedayMapping.GamedayMapping>();

            spName = "qz_gameday_tag_details_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    List<String> cursors = new List<String>() { "p_curr_gd_tag_detail" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = quizMaterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_gd_tag_detail", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        vGamedayMappings = DataInitializer.GamedayMapping.GamedayMapping.InitializeGamedayMapping(ds, out retVal);
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

                return vGamedayMappings;
            }
        }

        public Int32 UpdateGamedayMapping(Int64 OptType, Int64 quizMaterId, ref Int32 retVal, List<int> gamedayId, List<string> tag, int isBonus)
        {
            retVal = -50;
            String spName = String.Empty;
            List<int> pIsBonus = new List<int>();
            for(int i = 0; i < gamedayId.Count; i++)
            {
                pIsBonus.Add(isBonus);
            }

            spName = "qz_quiz_gameday_tag_upd";

            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaAdmin + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = quizMaterId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_gameday_id", NpgsqlDbType.Array | NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = gamedayId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_is_bonus_matchday", NpgsqlDbType.Array | NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = pIsBonus;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_gd_qtn_tag", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input }).Value = tag;

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = 1;
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
            }

            return retVal;
        }
    }
}
