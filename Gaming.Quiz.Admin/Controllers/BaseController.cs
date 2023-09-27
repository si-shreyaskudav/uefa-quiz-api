using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Admin;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Gaming.Quiz.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOptions<Application> _AppSettings;
        protected readonly Contracts.Configuration.Admin _Admin;
        protected readonly ISession _Session;
        protected readonly IAWS _AWS;
        protected readonly IPostgre _Postgre;
        protected readonly ICookies _Cookies;
        protected readonly IRedis _Redis;
        protected readonly IAsset _Asset;
        protected readonly Int32 _TourId;
        protected readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _HttpContext;
        protected readonly String _Environment;
        protected readonly IHostingEnvironment _HostEnv;

        public BaseController(IOptions<Application> appSettings, ISession session, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
             Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IHostingEnvironment hostingEnv)
        {
            _AppSettings = appSettings;
            _Session = session;
            _AWS = aws;
            _Postgre = postgre;
            _Cookies = cookies;
            _Redis = redis;
            _Asset = asset;
            _TourId = appSettings.Value.Properties.TourId;
            _HttpContext = httpContext;
            //_Admin = new App_Code.Helper(asset).Config();
            _Admin = appSettings.Value.Admin;
            _Environment = appSettings.Value.Connection.Environment;
            _HostEnv = hostingEnv;
        }

        public async Task<bool> Notify(string subject, string body)
        {
            bool status = false;
            try
            {
                String sender = _AppSettings.Value.Admin.Notification.Sender;
                String recipient = _AppSettings.Value.Admin.Notification.Recipient;
                //List<ServiceConfig> mServiceConfig = _gameplay.GetServiceConfig().Result.ToList();

                //if (mServiceConfig.Any(c => c.ServiceName == _Service && c.SendEmail == 1))
                {
                    status = await _AWS.SendSNSAlert($"Yahoo Cricket Quiz [{_Environment.ToUpper()}] " + subject, body);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
    }
}
