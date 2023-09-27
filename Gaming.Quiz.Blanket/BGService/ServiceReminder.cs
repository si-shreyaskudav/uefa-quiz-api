using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.PointCalculation;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.DataAccess.BGService;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.ServiceReminder;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Blanket.BGService
{
    public class ServiceReminder : Common.BaseBlanket, IServiceReminderBGService
    {

        protected readonly Gaming.Quiz.DataAccess.BGService.ServiceReminder _serviceReminder;

        public ServiceReminder(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _serviceReminder = new DataAccess.BGService.ServiceReminder(appSettings, postgre, cookies);
        }

        public List<string> GetPendingDays(Int64 DayNo,  ref Int32 retVal, ref string error)
        {
            Int32 OptType = 1;

            retVal = -40;

            List<string> pendingDays = new List<string>();
            try
            {
                pendingDays = _serviceReminder.GetPendingDays(OptType, DayNo, _MasterId, ref retVal);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return pendingDays;
        }
    }
}
