using  Gaming.Quiz.Interfaces.Storage;
using System;
using  Gaming.Quiz.Interfaces.Session;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using  Gaming.Quiz.DataInitializer.Common;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Contracts.Configuration;

namespace  Gaming.Quiz.DataAccess.Common
{
    public class BaseDataAccess
    {
        protected readonly IOptions<Application> _AppSettings;
        protected readonly String _ConnectionString;
        protected readonly String _PointCalString;
        protected readonly String _Schema;
        protected readonly String _SchemaAdmin;
        protected readonly String _SchemaSimulation = "dcfsimu.";
        protected readonly String _SchemaRank;
        protected readonly String _SchemaService;
        protected readonly String _SchemaLive= "dcflive.";

        protected readonly String _SchemaAchievement;

        //protected readonly NpgsqlConnection _Connection;
        protected readonly ICookies _Cookies;
        protected readonly Int32 _HostCount;

        #region " Public Functions "

        protected DataSet RunTransaction(ref NpgsqlTransaction transaction, NpgsqlConnection connection, NpgsqlCommand mNpgsqlCmd, List<String> cursors)
        {
            transaction = connection.BeginTransaction();
            mNpgsqlCmd.ExecuteNonQuery();
            DataSet dataSet = Utility.GetDataSet(mNpgsqlCmd, cursors);
            transaction.Commit();
            transaction.Dispose();

            return dataSet;
        }


        protected Int64 GetMod(Int64 userId = 0, Int64 socialId = 0, Int64 teamId = 0)
        {
            Int64 mod = default(Int64);
            Int64 source = userId != 0 ? userId : socialId;

            source = source != 0 ? source : teamId;

            if (source != 0)
            {
                mod = ((source % _HostCount) + 1);
            }

            return mod;
        }


        protected Int64 GetTeamMod(Int64 userId = 0, Int64 socialId = 0, Int64 teamId = 0)
        {
            Int64 mod = default(Int64);
            Int64 source = userId != 0 ? userId : socialId;

            source = source != 0 ? source : teamId;

            if (source != 0)
            {
                mod = ((source % _HostCount) + 1);
            }

            return mod;
        }
        #endregion

        public BaseDataAccess(IOptions<Application> appSettings, IPostgre postgre, ICookies cookies)
        {
            _AppSettings = appSettings;
            _ConnectionString = postgre.ConnectionString;
            _PointCalString = postgre.PointCalConnectionString;
            _Schema = postgre.Schema;
            _SchemaRank = postgre.SchemaRank;
            _SchemaService = postgre.SchemaService;
            _SchemaAchievement = postgre.SchemaAchievement;
            //_Connection = postgre.Connection;
            _Cookies = cookies;
            _SchemaAdmin = postgre.SchemaAdmin;
        }
    }
}
