using UEFA.UCL.Classic.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UEFA.UCL.Classic.Interfaces.Storage;
using UEFA.UCL.Classic.Interfaces.Asset;
using UEFA.UCL.Classic.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using UEFA.UCL.Classic.Contracts.Common;
using System;
using UEFA.UCL.Classic.Contracts.Gameplay;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace UEFA.UCL.Classic.API.Controllers.Settings
{
    [Route("services/api/[controller]")]
    [ApiController]
    public partial class SettingsController : BaseController
    {
        private readonly Blanket.Settings.Helper _HelperContext;
        private readonly Blanket.Settings.Newsletter _NewsletterContext;

        public SettingsController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext, IMemoryCache cache)
               : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _HelperContext = new Blanket.Settings.Helper(appSettings, aws, postgre, redis, cookies, asset, httpContext, cache);
            _NewsletterContext = new Blanket.Settings.Newsletter(appSettings, aws, postgre, redis, cookies, asset, httpContext);
        }

        /// <summary>
        /// Deletes the fantasy team for a user
        /// </summary>
        /// <param name="OptType">Operation type</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns></returns>
        [Route("user/delete-fantasy-team")]
        [HttpPost]
        public ActionResult<HTTPResponse> DeleteFantasyTeam(Int64 OptType, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse httpResponse = _HelperContext.DeleteFantasyTeam(OptType);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// API to update a user's team name
        /// </summary>
        /// <param name="mTeamNameInfo">Payload</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns></returns>
        [Route("user/update-team-name")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> UpdateTeamName(TeamNameInfo mTeamNameInfo, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse httpResponse = await _HelperContext.UpdateTeamName(mTeamNameInfo);

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