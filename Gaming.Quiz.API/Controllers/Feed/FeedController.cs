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
using Gaming.Quiz.Contracts.Gameplay;
using Gaming.Quiz.Blanket.Feeds;
using Gaming.Quiz.Interfaces.Feeds;

namespace Gaming.Quiz.API.Controllers.Gameplay
{
    [Route("services/[controller]")]
    public class FeedController : BaseController
    {
        private readonly IFeedsBlanket _feedsBlanket;

        public FeedController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
           IHttpContextAccessor httpContext, IHostingEnvironment env, IFeedsBlanket feedsBlanket)
           : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            this._feedsBlanket = feedsBlanket;
        }


        /// <summary>
        /// Returns all the available languages
        /// </summary>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [HttpGet("languages")]
        public async Task<ActionResult<HTTPResponse>> Languages(String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = await _feedsBlanket.GetLanguages();

                    return Ok(response);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Returns translations
        /// </summary>
        /// <param name="langCode">langauge code</param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [HttpGet("translations")]
        public async Task<ActionResult<HTTPResponse>> Translations(String langCode = "en", String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = await _feedsBlanket.GetTranslations(langCode);

                    return Ok(response);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Returns tutorial questions
        /// </summary>
        /// <param name="langCode">langauge code</param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [HttpGet("tutorial")]
        public async Task<ActionResult<HTTPResponse>> Tutorial(String langCode = "en", String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = await _feedsBlanket.GetTutorial(langCode);

                    return Ok(response);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }
    }
}