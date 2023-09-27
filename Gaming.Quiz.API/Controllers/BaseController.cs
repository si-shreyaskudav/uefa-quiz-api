using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Admin;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Asset;
using  Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Contracts.Common;
using System.Threading.Tasks;
using System.Linq;

namespace  Gaming.Quiz.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IOptions<Application> _AppSettings;
        protected readonly IAWS _AWS;
        protected readonly IPostgre _Postgre;
        protected readonly ICookies _Cookies;
        protected readonly IRedis _Redis;
        protected readonly IAsset _Asset;
        protected readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _HttpContext;
        protected readonly Library.Dependency.Authentication _Authentication;

        public BaseController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _AppSettings = appSettings;
            _AWS = aws;
            _Postgre = postgre;
            _Cookies = cookies;
            _Redis = redis;
            _Asset = asset;
            _HttpContext = httpContext;
            _Authentication = new Library.Dependency.Authentication(appSettings, httpContext);
        }
    }
}
