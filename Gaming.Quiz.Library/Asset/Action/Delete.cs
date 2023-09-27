using System;
using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace  Gaming.Quiz.Library.Asset.Action
{
    public class Delete : Write
    {
        public Delete(IAWS aws, IRedis redis, IOptions<Application> appSettings, ICookies cookies, IHttpContextAccessor httpContextAccessor)
            : base(aws, redis, appSettings, cookies, httpContextAccessor)
        {
        }

        public async Task<bool> REMOVE(String key)
        {
            bool success = false;

            if (_UseRedis)
                success = _Redis.Remove(key);
            else
                success = await _AWS.Remove(key);

            return success;
        }
    }
}
