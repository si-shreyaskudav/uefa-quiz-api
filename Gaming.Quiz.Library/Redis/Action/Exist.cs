using StackExchange.Redis;
using System;

namespace  Gaming.Quiz.Library.Redis.Action
{
    public partial class Broker
    {
        public bool Has(String key)
        {
            IDatabase client;

            client = _ClientManager.GetDatabase();

            return client.KeyExists(key);
        }
    }
}
