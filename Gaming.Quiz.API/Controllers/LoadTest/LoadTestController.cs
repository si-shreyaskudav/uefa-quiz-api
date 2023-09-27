using UEFA.UCL.Classic.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UEFA.UCL.Classic.Interfaces.Storage;
using UEFA.UCL.Classic.Interfaces.Asset;
using UEFA.UCL.Classic.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using UEFA.UCL.Classic.Contracts.Common;
using System;
using UEFA.UCL.Classic.Contracts.Session;
using System.Threading.Tasks;
using System.Linq;
using UEFA.UCL.Classic.Contracts.Gameplay;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace UEFA.UCL.Classic.API.Controllers.LoadTest
{
    [Route("services/api/[controller]")]
    [ApiController]
    public class LoadTestController : BaseController
    {
        private readonly Blanket.LoadTest.UseCase _TestContext;

        public LoadTestController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext, IMemoryCache cache)
               : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _TestContext = new Blanket.LoadTest.UseCase(appSettings, aws, postgre, redis, cookies, asset, httpContext, cache);
        }

        /// <summary>
        /// API to generate a user's Fantasy game session
        /// </summary>
        /// <param name="details">Payload</param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public ActionResult<HTTPResponse> Login(LoginDetails details)
        {
            if (_Authentication.Validate(_AppSettings.Value.API.Authentication.Backdoor))
            {
                String SocialId = details.SocialId;
                details.ClientId = 1;
                details.CountryCode = "IND";
                details.EmailId = SocialId + "@gmail.com";
                details.FullName = "Full Name";
                details.GUID = "asdasds-asdsasda-adasdasd-asdad";
                details.optType = 1;
                details.platformId = 1;
                details.SocialId = SocialId;
                details.UefaSocialId = SocialId;

                HTTPResponse httpResponse = _TestContext.Login(details);

                return Ok(httpResponse);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Submits a user's team
        /// </summary>
        /// <param name="details">payload</param>
        /// <returns></returns>
        [Route("teamcreate")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> CreateTeam(LoginDetails details)
        {
            if (_Authentication.Validate(_AppSettings.Value.API.Authentication.Backdoor))
            {
                Team team = new Team();
                team.OptType = 1;
                team.Platform = 1;
                team.GamedayId = "1";
                team.PhaseId = 1;
                team.CaptainId = 250065455;
                team.IsWildcard = 0;

                String pIds = "250051237,250079261,250065455,70109,250024370,250066369,250070417,250000919,250043137,250081554,250007631,97923,1900737,1906016,104071";
                team.InPlayerId = pIds.Split(',').Select(Int32.Parse).ToList();

                String benches = "0,0,0,0,0,0,0,0,0,0,0,1,2,3,4";
                team.InPlayerBenchPosition = benches.Split(',').Select(Int32.Parse).ToList();

                team.OutPlayerId = new List<int>();
                team.OutPlayerBenchPosition = new List<int>();

                team.SubPlayerId = new List<int>();
                team.SubPlayerBenchPosition = new List<int>();

                team.GDCompId = 9;
                team.MDCompId = 7;
                team.ISMCPR = 0;
                team.IsNewsletter = 0;
                team.LangCode = "en";
                team.BadgeId = 0;
                team.FavTeamId = 0;
                team.TeamName = "LT_TEAM_" + details.SocialId;

                HTTPResponse httpResponse = await _TestContext.CreateTeam(team, Int64.Parse(details.SocialId));

                return Ok(httpResponse);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Get a user's team
        /// </summary>
        /// <param name="SocialId">SocialId</param>
        /// <returns></returns>
        [Route("teamget")]
        [HttpGet]
        public ActionResult<HTTPResponse> UserTeam(String SocialId)
        {
            if (_Authentication.Validate(_AppSettings.Value.API.Authentication.Backdoor))
            {
                HTTPResponse httpResponse = _TestContext.UserTeam(Convert.ToInt64(SocialId), 1, 1, 1, "en");

                return Ok(httpResponse);
            }
            else
                return Unauthorized();
        }
    }
}