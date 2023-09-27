using Gaming.Quiz.Contracts.BGServices;
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

namespace Gaming.Quiz.DataAccess.BGService
{
    public class GamedayMapping : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public GamedayMapping(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public List<GamedayProccesQuizes> GetQuizesForGamedayMapping(int optType)
        {
            List<GamedayProccesQuizes> res = new List<GamedayProccesQuizes>();
            Int64 retVal = -50;
            String spName = String.Empty;

            spName = "qz_gdmap_quiz_get";

            
            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaAdmin + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = optType;

                        NpgsqlParameter retvalue = new NpgsqlParameter("p_ret_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retvalue);

                        NpgsqlParameter retJson = new NpgsqlParameter("p_json", NpgsqlDbType.Json) { Direction = ParameterDirection.Output };
                        mNpgsqlCmd.Parameters.Add(retJson);

                        if (connection.State != ConnectionState.Open) connection.Open();

                        mNpgsqlCmd.ExecuteScalar();

                        retVal = retvalue.Value != null && retvalue.Value.ToString().Trim() != "" ? Int64.Parse(retvalue.Value.ToString()) : retVal;


                        if (retJson != null && retVal == 1)
                        {
                            res = GenericFunctions.Deserialize<List<GamedayProccesQuizes>>(retJson.Value.ToString());                            
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

                return res;
            }
        }

        public Int32 ProcessGameday(int optType, long quizId, int day)
        {
            Int32 retVal = -50;
            String spName = String.Empty;

            spName = "qz_fant_quiz_ques_map_process";


            using (NpgsqlConnection connection = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_Schema + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input }).Value = optType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = quizId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_day", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = day;

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

    }
}
