using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Gaming.Quiz.Contracts.Common;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Leaderboard;
using Gaming.Quiz.Interfaces.Leaderboard;

namespace Gaming.Quiz.API.Controllers.Leaderboard
{
    [Route("services/[controller]")]
    public class LeaderboardController : BaseController
    {
        private readonly ILeaderboardBlanket _leaderboardBlanket;

        public LeaderboardController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
          IHttpContextAccessor httpContext, IHostingEnvironment env, ILeaderboardBlanket leaderboardBlanket)
          : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            this._leaderboardBlanket = leaderboardBlanket;
        }

        /// <summary>
        /// API to fetch User Rank on leaderboard
        /// </summary>
        /// <param name="userRank">Payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("{userguid}/userrank")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> GetUserRank(UserRankInput userRank, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.GetUserRank(userRank);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch User Player Rank on leaderboard
        /// </summary>
        /// <param name="userPlayerRank">Payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("{userguid}/userteamrank")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> GetUserRank(UserPlayerRankInput userPlayerRank, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.GetTeamPlayerRank(userPlayerRank);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch Best score User Rank on leaderboard
        /// </summary>
        /// <param name="userRank">Payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("{userguid}/bestscoreuserrank")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> GetBestScoreUserRank(UserRankInput userRank, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.GetBestScoreUserRank(userRank);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }


        /// <summary>
        /// API to fetch overall Leaderboard
        /// </summary>
        /// <param name="OptType">OptType - 1</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("overallleaderboard")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> OverallLeaderbaord(Int32 OptType=1, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.OverAllLeaderboard(OptType);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch Player overall Leaderboard
        /// </summary>
        /// <param name="PlayerId">MonthId</param>
        /// <param name="OptType">OptType - 1</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("teamleaderboard")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> TeamPlayerLeaderbaord(Int64 PlayerId, Int32 OptType = 1, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.TeamPlayerLeaderboard(OptType, PlayerId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch monthly Leaderboard
        /// </summary>
        /// <param name="MonthId">MonthId</param>
        /// <param name="GamedayId">MonthId</param>
        /// <param name="OptType">OptType - 1 for monthly || OptType - 2 for gamedaywise</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("monthleaderboard")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> MonthLeaderbaord(Int64 MonthId, Int64 GamedayId, Int32 OptType = 1, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.MonthLeaderboard(OptType, MonthId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch gloabl Leaderboard
        /// </summary>
        /// <param name="MonthId">MonthId</param>
        /// <param name="OptType">OptType - 1</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("bestscoreleaderboard")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> BestScoreLeaderbaord( Int64 MonthId, Int32 OptType =1, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.BestScoreLeaderboard(OptType, MonthId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to fetch User Rank on leaderboard
        /// </summary>
        /// <param name="userRank">Payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>User's session data</returns>
        [Route("getmonth")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> GetMonth(String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _leaderboardBlanket.GetMonth();

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }
    }
}