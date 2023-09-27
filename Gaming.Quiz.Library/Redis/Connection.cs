using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Linq;
using  Gaming.Quiz.Contracts.Configuration;

namespace  Gaming.Quiz.Library.Redis
{
    public class Connection
    {
        public static ConnectionMultiplexer _ClientManager;
        public static Contracts.Configuration.Redis _ConnectionEnvironment;

        public Connection(IOptions<Application> appSettings)
        {
            _ConnectionEnvironment = appSettings.Value.Connection.Redis;
        }

        public void Multiplexer()
        {
            if (_ClientManager == null || !_ClientManager.IsConnected)
                ConnectMultiplexer();
        }

        public void Disposer()
        {
            ConnectDisposer();
        }

        #region " Connection Managers "

        private static void ConnectMultiplexer()
        {
            EndPointCollection mEndPointCollection = new EndPointCollection();
            ConfigurationOptions mConfigurationOptions = new ConfigurationOptions();

            mConfigurationOptions = new ConfigurationOptions
            {
                //Ssl = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                AbortOnConnectFail = false,
                KeepAlive = 60, // 60 sec to ensure connection is alive
                ConnectTimeout = 15000, // 15 sec
                SyncTimeout = 10000, // 10 sec
            };

            String mRedisConn = _ConnectionEnvironment.Server;

            foreach (String mStr in mRedisConn.Split(',').ToList())
            {
                mConfigurationOptions.EndPoints.Add(mStr.Trim(), _ConnectionEnvironment.Port);
            }

            _ClientManager = ConnectionMultiplexer.Connect(mConfigurationOptions);
        }

        private static void ConnectDisposer()
        {
            _ClientManager.Dispose();
        }

        #endregion
    }
}
