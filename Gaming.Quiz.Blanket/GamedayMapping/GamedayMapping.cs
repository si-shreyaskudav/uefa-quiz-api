using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.GamedayMapping;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.GamedayMapping;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Blanket.GamedayMapping
{
    public class GamedayMapping : Common.BaseBlanket, IGamedayMappingBlanket
    {
        protected readonly DataAccess.GamedayMapping.GamedayMapping _GamedayMappingContext;
        Int64 OptType = 1;
        //int isBonus = 0;

        public GamedayMapping(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _GamedayMappingContext = new DataAccess.GamedayMapping.GamedayMapping(appSettings, postgre, cookies);
        }

        public List<Contracts.GamedayMapping.GamedayMapping> GetGamedayMappings()
        {
            Int32 retVal = -40;

            try
            {
                return _GamedayMappingContext.GetGamedayMappings(OptType, _MasterId, ref retVal);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Int32 UpdateGamedayMapping(List<int> gamedayId, List<string> tag)
        {
            Int32 retVal = -40;

            try
            {
                return _GamedayMappingContext.UpdateGamedayMapping(OptType, _MasterId, ref retVal, gamedayId, tag, _Bonus);
            }
            catch (Exception ex)
            {
                return retVal;
            }
        }
    }
}
