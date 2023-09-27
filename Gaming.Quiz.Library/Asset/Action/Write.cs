using System;
using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using  Gaming.Quiz.Contracts.Common;
using System.IO;

namespace  Gaming.Quiz.Library.Asset.Action
{
    public class Write : Read
    {
        private readonly FileSystem.Broker _FileSystem;

        public Write(IAWS aws, IRedis redis, IOptions<Application> appSettings, ICookies cookies, IHttpContextAccessor httpContextAccessor)
            : base(aws, redis, appSettings, cookies, httpContextAccessor)
        {
            _FileSystem = new FileSystem.Broker();
        }

        public async Task<bool> SET(String key, Object content, bool serialize = true)
        {
            bool success = false;

            if (_UseRedis)
                success = await _Redis.Set(key, content, serialize);
            else
                success = await _AWS.Set(key, content, serialize);

            return success;
        }

        public async Task<bool> RedisSET(String key, Object content, bool serialize = true)
        {
            await _Redis.Set(key, content, serialize);

            return true;
        }

        public async Task<bool> SET(String bucket, String key, Object content, bool serialize = true)
        {
            return await _AWS.Set(bucket, key, content, serialize);
        }

        public async Task<bool> SETimage(String key, Stream content, bool downloadable = false)
        {
            bool success = false;

            if (_UseRedis)
                success = await _Redis.Set(key, content, false);
            else
                success = await _AWS.SetImage(key, content, downloadable);

            return success;
        }

        public async Task LOG(System.Reflection.MethodBase methodBase, String message)
        {
            HTTPLog log = Construct.Log(methodBase, message, _Cookies, _HttpContextAccessor);

            //await _FileSystem.Log(log, _AppSettings.Properties.LogFile);
            await _AWS.Log(log);
        }

        public async Task LOG(String message, Exception exception = null)
        {
            HTTPLog log = Construct.Log(message, exception);

            //await _FileSystem.Log(log, _AppSettings.Properties.LogFile);
            //await _AWS.Log(log);
        }

        public async Task DEBUG(String message)
        {
            await _AWS.Debug(message);
        }
    }
}
