using UEFA.UCL.Classic.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UEFA.UCL.Classic.Interfaces.Storage;
using UEFA.UCL.Classic.Interfaces.Asset;
using UEFA.UCL.Classic.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using UEFA.UCL.Classic.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace UEFA.UCL.Classic.API.Controllers.Share
{
    [Route("services/api/[controller]")]
    [ApiController]
    public class ShareController : BaseController
    {
        private readonly Blanket.Share.Graphic _GraphicContext;

        public ShareController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext)
               : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _GraphicContext = new Blanket.Share.Graphic(appSettings, aws, postgre, redis, cookies, asset, httpContext);
        }

        /// <summary>
        /// Generates a share graphic for the user
        /// </summary>
        /// <param name="guid">User GUID from Cookie</param>
        /// <param name="phaseId">Id of the phase for which the image is to be shared</param>
        /// <param name="gamedayId">Id of the gameday for which the image is to be shared</param>
        /// <param name="myPoints">Flag identifying whether MyPoints or MyTeam. 1 = MyPoints | 0 = MyTeam</param>
        /// <param name="platform">The social platform. 1 = Facebook | 2 = Twitter</param>
        /// <param name="language">EN,ES,FR,DE,RU,IT,PT | Default EN</param>
        /// <param name="backdoor">Debug param</param>
        /// <returns>An url to be shared on Facebook and Twitter</returns>
        [Route("{guid}/{phaseId}/{gamedayId}/{myPoints}/{platform}/{language}/generate")]
        [ActionName("generate")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> Generate(String guid, Int32 phaseId, Int32 gamedayId, Int32 myPoints, Int32 platform, String language, String backdoor = null)
        {
            _HttpContext.HttpContext.Response.Headers.Add("Edge-control", "cache-maxage=0s");

            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse httpResponse = await _GraphicContext.Generate(guid, phaseId, gamedayId, myPoints, language);

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