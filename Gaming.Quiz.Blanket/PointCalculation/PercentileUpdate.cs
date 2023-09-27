using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.PointCalculation;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.PercentileUpdate;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Blanket.PointCalculation
{

    public class PercentileUpdate : Common.BaseBlanket, IPercentileUpdate
    {

        protected readonly Gaming.Quiz.DataAccess.PointCalculation.PercentileUpdate _PercentileUpdContext;

        public PercentileUpdate(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _PercentileUpdContext = new DataAccess.PointCalculation.PercentileUpdate(appSettings, postgre, cookies);
        }

        public Tuple<Int32, Exception> UpdatePercentile()
        {
            Int32 retVal = -40;
            Int32 optType = 1;
            Exception error = null;
            try
            {
                retVal = _PercentileUpdContext.UpdatePercentile(optType, _MasterId);

            }
            catch (Exception ex)
            {
                error = ex;
            }

            return new Tuple<int, Exception>(retVal, error);
        }

    }
}
