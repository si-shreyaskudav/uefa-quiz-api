using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using  Gaming.Quiz.Interfaces.Admin;
using  Gaming.Quiz.Interfaces.Storage;
using  Gaming.Quiz.Interfaces.Asset;
using  Gaming.Quiz.Interfaces.Session;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Gaming.Quiz.Interfaces.DataPopulation;
using Gaming.Quiz.Interfaces.Template;
using Gaming.Quiz.Interfaces.Feeds;
using Gaming.Quiz.Interfaces.Leaderboard;
using Gaming.Quiz.Interfaces.PointCalculation;
using Gaming.Quiz.Interfaces.GamedayMapping;
using Gaming.Quiz.Interfaces.Analytics;

namespace Gaming.Quiz.Admin.Controllers
{
    public partial class HomeController
    {

        private readonly IDataPopulationBlanket _dataPopulationBlanket;
        private readonly IAdminServicesBlanket _adminServicesBlanket;
        private readonly IIngestionBlanket _ingestionBlanket;
        private readonly IFeedsBlanket _feedsBlanket;
        private readonly ILeaderboardBlanket _leaderboardBlanket;
        private readonly IPointCalculationBlanket _pointCalculationBlanket;
        private readonly IGamedayMappingBlanket _gamedayMappingBlanket;
        private readonly ITemplateBlanket _templateBlanket;
        private readonly IAnalyticsBlanket _analytics;

        public HomeController(IOptions<Application> appSettings, ISession session, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMemoryCache cache, IHostingEnvironment hostingEnv,
            IDataPopulationBlanket dataPopulationBlanket,
            IAdminServicesBlanket adminServicesBlanket,
            IIngestionBlanket ingestionBlanket,
            IFeedsBlanket feedsBlanket,
            ILeaderboardBlanket leaderboardBlanket,
            IPointCalculationBlanket pointCalculationBlanket,
            IGamedayMappingBlanket gamedayMappingBlanket,
            ITemplateBlanket templateBlanket,
            IAnalyticsBlanket analytics)
               : base(appSettings, session, aws, postgre, redis, cookies, asset, httpContext, hostingEnv)
        {
            this._dataPopulationBlanket = dataPopulationBlanket;
            this._adminServicesBlanket = adminServicesBlanket;
            this._ingestionBlanket = ingestionBlanket;
            this._feedsBlanket = feedsBlanket;
            this._leaderboardBlanket = leaderboardBlanket;
            this._pointCalculationBlanket = pointCalculationBlanket;
            this._gamedayMappingBlanket = gamedayMappingBlanket;
            this._templateBlanket = templateBlanket;
            this._analytics = analytics;
        }
    }
}
