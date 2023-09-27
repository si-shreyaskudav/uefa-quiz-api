using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Gaming.Quiz.DataInitializer.Common;
using System.Reflection;
using System.IO;
using Gaming.Quiz.Contracts.DataPopulation;
using Gaming.Quiz.Contracts.BGServices;
using Gaming.Quiz.Interfaces.GamedayMapping;

namespace Gaming.Quiz.Blanket.BGService
{
    public class GamedayMapping : Common.BaseBlanket, IGamedayMappingBGServiceBlanket
    {
        private readonly DataAccess.BGService.GamedayMapping _MappingContext;

        public GamedayMapping(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
        : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _MappingContext = new DataAccess.BGService.GamedayMapping(appSettings, postgre, cookies);
        }

        public List<GamedayProccesQuizes> GetQuizesForGamedayMapping()
        {
            Int32 optType = 1;

            List<GamedayProccesQuizes> usrPntPrcGet = new List<GamedayProccesQuizes>();
            try
            {
                usrPntPrcGet = _MappingContext.GetQuizesForGamedayMapping(optType);
            }
            catch (Exception ex)
            {
                throw new Exception("Blanket.BGService.GamedayMapping.GetQuizesForGamedayMapping: " + ex.Message);
            }

            return usrPntPrcGet;
        }

        public int ProcessGameday(long quizId)
        {
            Int32 optType = 1;
            Int32 retVal = -40;
            int day = 1;
            try
            {
                retVal = _MappingContext.ProcessGameday(optType, quizId, day);
            }
            catch (Exception ex)
            {
                throw new Exception("Blanket.BGService.GamedayMapping.ProcessGameday: " + ex.Message);
            }
            return retVal;
        }
    }
}
