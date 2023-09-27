using System;
using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace  Gaming.Quiz.Library.Asset.Action
{
    public class Read : BaseAsset
    {
        public Read(IAWS aws, IRedis redis, IOptions<Application> appSettings, ICookies cookies, IHttpContextAccessor httpContextAccessor)
            : base(aws, redis, appSettings, cookies, httpContextAccessor)
        {
        }

        public async Task<String> GET(String key)
        {
            String content = "";

            if (_UseRedis)
                content = await _Redis.Get(key);
            else
                content = await _AWS.Get(key);

            return content;
        }
    }
}
