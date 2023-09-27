using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.PointCalculation;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.PointCalculation;
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
    public class PointCalculation : Common.BaseBlanket, IPointCalculationBlanket
    {

        protected readonly Gaming.Quiz.DataAccess.PointCalculation.PointCalculation _PointCalContext;

        public PointCalculation(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _PointCalContext = new DataAccess.PointCalculation.PointCalculation(appSettings, postgre, cookies);
        }

        public List<PntPrcGet> UserPointProcessGet( ref string error)
        {
            Int32 OptType = 1;

            Int32 retVal = -40;
            

            List<PntPrcGet> usrPntPrcGet = new List<PntPrcGet>();
            try
            {

                usrPntPrcGet = _PointCalContext.UserPointProcessGet(OptType, _MasterId);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return usrPntPrcGet;
        }

        public List<PntPrcGet> UserEODSettlementGet(ref string error)
        {
            Int32 OptType = 1;

            Int32 retVal = -40;


            List<PntPrcGet> usrPntPrcGet = new List<PntPrcGet>();
            try
            {

                usrPntPrcGet = _PointCalContext.UserEODSettlementGet(OptType, _MasterId);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return usrPntPrcGet;
        }

        public async Task<Tuple<Int32, string>> UserPointProcess(Int32 Monthid, Int32 GamedayId=0, Int32 WeekId=0)
        {
            Int32 retVal = -40;
            Int32 optType = 1;
            string error = string.Empty;
            try
            {
                retVal = await _PointCalContext.UserPointProcess(optType, _MasterId, Monthid, GamedayId, WeekId);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, Exception>> ServiceUserPointProcess(Int32 Monthid, Int32 GamedayId = 0, Int32 WeekId = 0)
        {
            Int32 retVal = -40;
            Int32 optType = 1;
            Exception error = null;
            try
            {
                retVal = await _PointCalContext.UserPointProcess(optType, _MasterId, Monthid, GamedayId, WeekId);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            return new Tuple<int, Exception>(retVal, error);
        }


        public async Task<Tuple<Int32, string>> UserBestScoreUpdate(Int64 UserId,Int32 Monthid, Int32 GamedayId=0, Int32 WeekId=0)
        {
            Int32 retVal = -40;
            Int32 optType = 1;

            string error = string.Empty;
            try
            {
              retVal =  await  _PointCalContext.UserBestScoreUpd(optType, UserId, Monthid, GamedayId, WeekId, _MasterId, _SportsId, _CategoryId);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> EODSettlement()
        {
            Int32 retVal = -40;
            Int32 optType = 1;
            string error = string.Empty;
            try
            {

                SettlmentGet settlmentGet = new SettlmentGet();
                retVal  =  _PointCalContext.Settlement(optType, _MasterId,_SportsId,_CategoryId, ref settlmentGet);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public Tuple<Int32, Exception> ServiceEODSettlement( ref SettlmentGet settlmentGet)
        {
            Int32 retVal = -40;
            Int32 optType = 1;
            Exception error = null;
            try
            {

                retVal =  _PointCalContext.Settlement(optType, _MasterId, _SportsId, _CategoryId, ref settlmentGet);

            }
            catch (Exception ex)
            {
                error = ex;
            }

            return new Tuple<int, Exception>(retVal, error);
        }

    }
}
