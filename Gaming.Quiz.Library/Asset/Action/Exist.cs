using System;
using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace  Gaming.Quiz.Library.Asset.Action
{
    public class Exist: Delete
    {
        public Exist(IAWS aws, IRedis redis, IOptions<Application> appSettings, ICookies cookies, IHttpContextAccessor httpContextAccessor)
            : base(aws, redis, appSettings, cookies, httpContextAccessor)
        {
        }

        public async Task<bool> HAS(String key)
        {
            bool has = false;

            if (_UseRedis)
                has = _Redis.Has(key);
            else
                has = await _AWS.Has(key);

            return has;
        }
    }
}
