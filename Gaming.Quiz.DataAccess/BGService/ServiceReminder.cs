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

namespace Gaming.Quiz.DataAccess.BGService
{
   public class ServiceReminder : Common.BaseDataAccess
    {
        IPostgre _Postgre;
        public ServiceReminder(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies) : base(appSettings, postgre, cookies)
        {
            _Postgre = postgre;
        }

        public  List<string> GetPendingDays(Int32 OptType, Int64 DayNo, Int64 QzMId, ref Int32 retVal)
        {
            retVal = -50;
            String spName = String.Empty;
            NpgsqlTransaction transaction = null;

            List<string> pntPrcGets = new List<string>();

            spName = "qz_admin_service_pending_ques_bank_info_get";

            using (NpgsqlConnection connection = new NpgsqlConnection(_PointCalString))
            {
                try
                {
                    List<String> cursors = new List<string>() { "p_curr_day" };

                    using (NpgsqlCommand mNpgsqlCmd = new NpgsqlCommand(_SchemaRank + spName, connection))
                    {

                        mNpgsqlCmd.CommandType = CommandType.StoredProcedure;
                        mNpgsqlCmd.CommandTimeout = 0;

                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_opt_type", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = OptType;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_day_number", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = DayNo;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_qz_quiz_masterid", NpgsqlDbType.Numeric) { Direction = ParameterDirection.Input }).Value = QzMId;
                        mNpgsqlCmd.Parameters.Add(new NpgsqlParameter("p_curr_day", NpgsqlDbType.Refcursor)).Value = cursors[0];

                        if (connection.State != ConnectionState.Open) connection.Open();

                        DataSet ds = RunTransaction(ref transaction, connection, mNpgsqlCmd, cursors);
                        pntPrcGets = DataInitializer.PointCalculation.PointCalculation.InitializerGetPendingDays(ds, ref retVal);
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
    }
}
