using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using Gaming.Quiz.Contracts.Common;
using System;
using Gaming.Quiz.Contracts.Session;
using System.Linq;
using Gaming.Quiz.Library.Utility;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Sharing;
using Microsoft.AspNetCore.Hosting;
using Gaming.Quiz.Interfaces.Sharing;

namespace Gaming.Quiz.API.Controllers.SocialShare
{
    [Route("services/[controller]")]
    [ApiController]
    public class ShareController : BaseController
    //public partial class GameplayController: BaseController
    {
        private readonly String _AuthKey;
        private readonly IImageGenerationBlanket _imageGenerationBlanket;

        public ShareController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext, IHostingEnvironment env, IImageGenerationBlanket imageGenerationBlanket)
               : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _AuthKey = appSettings.Value.Properties.WAFAuthKey;
            this._imageGenerationBlanket = imageGenerationBlanket;
        }

        /// <summary>
        /// Generates Share Image.
        /// </summary>
        /// <param name="sharemodel">The GUID of the user</param>
        /// <param name="backdoor">backdoor</param>
        /// <returns></returns>
        [HttpPost("generate")]
        public async Task<ActionResult<HTTPResponse>> Generate(ShareModel sharemodel, string backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = await _imageGenerationBlanket.GenerateImage(sharemodel.GUID,"",sharemodel.RightAns ,sharemodel.TotalQtn, sharemodel.StreakPts, sharemodel.TotPoints,sharemodel.Answers, null, sharemodel.Message,sharemodel.PlatformId, "", 0, sharemodel.GamedayId, sharemodel.AnsPoints, sharemodel.TimePoints);
                    return Ok(response);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}