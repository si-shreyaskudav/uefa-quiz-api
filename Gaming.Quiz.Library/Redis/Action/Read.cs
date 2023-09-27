using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using  Gaming.Quiz.Contracts.Configuration;

namespace  Gaming.Quiz.Library.Redis.Action
{
    public partial class Broker : Connection, Interfaces.Storage.IRedis
    {
        public Broker(IOptions<Application> appSettings) : base(appSettings)
        {
        }

        public async Task<String> Get(String key)
        {
            IDatabase mredisClient;
            String mData = String.Empty;

            try
            {
                mredisClient = _ClientManager.GetDatabase();
                RedisValue mRedData = await mredisClient.StringGetAsync(key);
                mData = mRedData.ToString() ?? "";

                /*
                DateTime mExpireOn = DateTime.UtcNow.AddYears(1);
                mRedisClient.KeyExpire(key, mExpireOn);

                RedisValue[] mRedData = mRedisClient.SetMembers(key);

                if (mRedData != null)
                    mData = mRedData[0].ToString();
                    */
            }
            catch (Exception ex)
            {
                mData = "";
                throw ex;
            }

            return mData;
        }
    }
}
