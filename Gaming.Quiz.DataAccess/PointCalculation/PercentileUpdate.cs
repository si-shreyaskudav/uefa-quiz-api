using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.PointCalculation;
using Gaming.Quiz.DataInitializer.Common;
using Gaming.Quiz.Interfaces.PercentileUpdate;
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
    public class PercentileUpdate : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public PercentileUpdate(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public Int32 UpdatePercentile(Int32 OptType, Int64 QzMId)
        {
            Int32 retVal = -50;
            String spName = String.Empty;
            spName = "qz_admin_percentile_bracket_upd_service";

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
