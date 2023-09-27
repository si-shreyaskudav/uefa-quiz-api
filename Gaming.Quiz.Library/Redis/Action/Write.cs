using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using  Gaming.Quiz.Library.Utility;

namespace  Gaming.Quiz.Library.Redis.Action
{
    public partial class Broker
    {
        public async Task<bool> Set(String key, Object content, bool serialize)
        {
            IDatabase redisClient;
            bool mSuccess = false;

            String data = serialize ? GenericFunctions.Serialize(content) : content.ToString();

            try
            {
                redisClient = _ClientManager.GetDatabase();
                redisClient.StringSetAsync(key, data);
                mSuccess = true;

                /*
                if (redisClient.KeyExists(key))
                    redisClient.KeyDelete(key);

                mSuccess = redisClient.SetAdd(key, data);

                DateTime mExpireOn = DateTime.UtcNow.AddYears(1);
                redisClient.KeyExpire(key, mExpireOn);
                */
            }
            catch (Exception ex)
            {
                mSuccess = false;
                throw ex;
            }

            return mSuccess;
        }
    }
}
