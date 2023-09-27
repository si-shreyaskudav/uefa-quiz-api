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

namespace Gaming.Quiz.API.Controllers
{
    [Route("services/ping")]
    [ApiController]
    public class PingController : BaseController
    {
        private readonly IHostingEnvironment _Env;
        private const String _Secret = "$uperm@r!0";

        public PingController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext, IHostingEnvironment env)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _Env = env;
        }

        /// <summary>
        /// API Status check
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        [ProducesResponseType(200)]
        public ActionResult Ping()
        {
            return Ok("Gaming.Quiz.API.V4");
        }

        /// <summary>
        /// Generates swagger documentation locally. Available only in Debug mode.
        /// </summary>
        /// <returns></returns>
        [HttpGet("swagger")]
        [ProducesResponseType(200)]
        public ActionResult Swagger()
        {
            if (_Env.IsDevelopment())
            {
                string jsonFile = "Gaming.Quiz.API.json";
                string jsonPath = System.IO.Path.Combine(@"D:\Swapnil\publish\Admin-Quiz\Nginx\", jsonFile);
                string swaggerUrl = "https://localhost:44399/swagger/v1/swagger.json";
                System.IO.File.WriteAllText(jsonPath, Library.Utility.GenericFunctions.GetWebData(swaggerUrl));
            }

            return Ok();
        }

    }
}
