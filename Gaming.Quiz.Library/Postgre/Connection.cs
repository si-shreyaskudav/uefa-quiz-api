using Microsoft.Extensions.Options;
using Npgsql;
using System;
using  Gaming.Quiz.Contracts.Configuration;

namespace  Gaming.Quiz.Library.Postgre
{
    public class Connection : Interfaces.Storage.IPostgre
    {
        private Contracts.Configuration.Postgre _ConSettings;
        private NpgsqlConnection connection;

        public Connection(IOptions<Application> appSettings)
        {
            _ConSettings = appSettings.Value.Connection.Postgre;
        }

        public String Schema { get { return _ConSettings.Schema; } }
        public String SchemaAdmin { get { return _ConSettings.SchemaAdmin; } }
        public String SchemaService { get { return _ConSettings.SchemaService; } }

        public String SchemaRank { get { return _ConSettings.SchemaRank; } }
        public String SchemaAchievement { get { return _ConSettings.SchemaAchievement; } }

        public NpgsqlConnection _Connection
        {
            get
            {
                return connection;
            }
        }

        public String ConnectionString
        {
            get
            {
                String p = "";
                String connection = _ConSettings.ConnectionString;
                bool pooling = _ConSettings.Pooling;
                int minPool = _ConSettings.MinPoolSize;
                int maxPool = _ConSettings.MaxPoolSize;

                if (pooling)
                    p = "Pooling=true;MinPoolSize=" + minPool + ";MaxPoolSize=" + maxPool + ";";
                else
                    p = "Pooling=false;";

                return String.Concat(connection, p);
                //return "this is the probleml";
            }
        }

        public String PointCalConnectionString
        {
            get
            {
                String p = "";
                String connection = _ConSettings.PointCalConn;
                bool pooling = _ConSettings.Pooling;
                int minPool = _ConSettings.MinPoolSize;
                int maxPool = _ConSettings.MaxPoolSize;

                if (pooling)
                    p = "Pooling=true;MinPoolSize=" + minPool + ";MaxPoolSize=" + maxPool + ";";
                else
                    p = "Pooling=false;";

                return String.Concat(connection, p);
                //return "this is the probleml";
            }
        }
        

        //public String RepConnectionString
        //{
        //    get
        //    {
        //        String p = "";
        //        String connection = _ConSettings.RepHost;
        //        bool pooling = _ConSettings.Pooling;
        //        int minPool = _ConSettings.MinPoolSize;
        //        int maxPool = _ConSettings.MaxPoolSize;

        //        if (pooling)
        //            p = "Pooling=true;MinPoolSize=" + minPool + ";MaxPoolSize=" + maxPool + ";";
        //        else
        //            p = "Pooling=false;";

        //        return String.Concat(connection, p);
        //    }
        //}

        //public String MasterConnectionString
        //{
        //    get
        //    {
        //        String p = "";
        //        String connection = _ConSettings.MasterHost;
        //        bool pooling = _ConSettings.Pooling;
        //        int minPool = _ConSettings.MinPoolSize;
        //        int maxPool = _ConSettings.MaxPoolSize;

        //        if (pooling)
        //            p = "Pooling=true;MinPoolSize=" + minPool + ";MaxPoolSize=" + maxPool + ";";
        //        else
        //            p = "Pooling=false;";

        //        return String.Concat(connection, p);
        //    }
        //}

        public void Establish()
        {
            connection = new NpgsqlConnection(ConnectionString);

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }

        public void Dispose()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();

            connection.Dispose();
        }
    }
}
