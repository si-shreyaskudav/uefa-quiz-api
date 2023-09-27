using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Asset;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using  Gaming.Quiz.Contracts.Session;
using  Gaming.Quiz.Library.Utility;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Feeds;

namespace  Gaming.Quiz.Blanket.Common
{
    public class BaseBlanket: ResponseResult
    {
        protected readonly IOptions<Application> _AppSettings;
        protected readonly IAWS _AWS;
        protected readonly IPostgre _Postgre;
        protected readonly IRedis _Redis;
        protected readonly ICookies _Cookies;
        protected readonly IAsset _Asset;
        protected readonly IHttpContextAccessor _HttpContext;



        #region " Public properties "

        public readonly List<String> _Lang;
        protected readonly Int32 _TourId;

        protected readonly Int64 _SportsId;
        protected readonly Int64 _CategoryId;
        protected readonly Int64 _MasterId;
        protected readonly int _Bonus;

        #endregion

        #region " Public Functions "

        public String GetIPAdress()
        {
            String ipAddress = Convert.ToString(_HttpContext.HttpContext.Request.HttpContext.Connection.RemoteIpAddress);
            if (string.IsNullOrEmpty(ipAddress))
            {
                if (_HttpContext.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    ipAddress = _HttpContext.HttpContext.Request.Headers["X-Forwarded-For"];

            }
            return ipAddress;
        }

        #endregion

        public BaseBlanket(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
        {
            _AppSettings = appSettings;
            _AWS = aws;
            _Postgre = postgre;
            _Redis = redis;
            _Cookies = cookies;
            _Asset = asset;
            _Lang = appSettings.Value.Properties.Languages;
            _TourId = appSettings.Value.Properties.TourId;
            _HttpContext = httpContext;
            _SportsId = appSettings.Value.Properties.QuizSportsId;
            _CategoryId = appSettings.Value.Properties.QuizCategoryId;
            _MasterId = appSettings.Value.Properties.QuizMasterId;
            _Bonus = appSettings.Value.Properties.Bonus;
        }
    }
}
