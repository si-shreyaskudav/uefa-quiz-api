using StackExchange.Redis;
using System;
using System.Net;

namespace  Gaming.Quiz.Library.Redis.Action
{
    public partial class Broker
    {
        public bool RemoveAllLiveLBKeys()
        {
            IDatabase client;
            bool success = false;
            client = _ClientManager.GetDatabase();
            EndPoint endPoint = _ClientManager.GetEndPoints()[0];
            // get the target server
            var server = _ClientManager.GetServer(endPoint);
            // show all keys in database 0 that include "live" in their name
            foreach (var vkey in server.Keys(pattern: "*dream11*live*vrsn*"))
            {
                if (client.KeyExists(vkey))
                {
                    try
                    {
                        client.KeyDelete(vkey);
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                }
                else
                {
                    success = true;
                }
            }
            return success;
        }

        public bool RemoveKeys(String pattern)
        {
            IDatabase client;
            bool success = false;
            client = _ClientManager.GetDatabase();
            EndPoint endPoint = _ClientManager.GetEndPoints()[0];
            // get the target server
            var server = _ClientManager.GetServer(endPoint);
            // show all keys in database 0 that include "live" in their name
            foreach (var vkey in server.Keys(pattern: pattern))
            {
                if (client.KeyExists(vkey))
                {
                    try
                    {
                        client.KeyDelete(vkey);
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                }
                else
                {
                    success = true;
                }
            }
            return success;
        }
        public bool Remove(String key)
        {
            IDatabase client;
            bool success = false;

            client = _ClientManager.GetDatabase();

            if (client.KeyExists(key))
            {
                try
                {
                    client.KeyDelete(key);
                    success = true;
                }
                catch
                {
                    success = false;
                }
            }
            else
            {
                success = true;
            }

            return success;
        }
    }
}
