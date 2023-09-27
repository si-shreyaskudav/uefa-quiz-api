using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Asset;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace  Gaming.Quiz.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOptions<Application> _AppSettings;
        protected readonly IAWS _AWS;
        protected readonly IPostgre _Postgre;
        protected readonly ICookies _Cookies;
        protected readonly IRedis _Redis;
        protected readonly IAsset _Asset;
        protected readonly IHttpContextAccessor _HttpContextAccessor;
        public readonly List<String> _Lang;

        public BaseController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContextAccessor)
        {
            _AppSettings = appSettings;
            _AWS = aws;
            _Postgre = postgre;
            _Cookies = cookies;
            _Redis = redis;
            _Asset = asset;
            _HttpContextAccessor = httpContextAccessor;
            _Lang = appSettings.Value.Properties.Languages;
        }
    }
}
