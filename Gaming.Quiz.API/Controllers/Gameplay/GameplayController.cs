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
using Gaming.Quiz.Interfaces.Gameplay;

namespace Gaming.Quiz.API.Controllers.Gameplay
{
    /// <summary>
    /// 
    /// </summary>
    [Route("services/[controller]")]
    public class GameplayController : BaseController
    {

        private readonly IGameplayBlanket _gameplayBlanket;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="aws"></param>
        /// <param name="postgre"></param>
        /// <param name="redis"></param>
        /// <param name="cookies"></param>
        /// <param name="asset"></param>
        /// <param name="httpContext"></param>
        /// <param name="env"></param>
        public GameplayController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
           IHttpContextAccessor httpContext, IHostingEnvironment env, IGameplayBlanket gameplayBlanket)
           : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            this._gameplayBlanket = gameplayBlanket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/userdetails")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> GetUserDetails(Int64 quizId, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = await _gameplayBlanket.GetUserDetails(quizId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="playAttempt"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/attempt")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> AttemptStart(Int64 quizId, PlayAttempt playAttempt, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    playAttempt.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.PlayAttempt(playAttempt);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="lifeline"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/fifty")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> LLFiftyFifty(Int64 quizId, Lifeline lifeline, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    lifeline.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.LLFiftyFifty(lifeline);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="OptType"></param>
        /// <param name="lifeline"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/sneakpeak")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> LLSneakpeak(Int64 quizId, Int64 OptType, Lifeline lifeline, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    lifeline.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.LLSneakPeak(lifeline, OptType);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="lifeline"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/switch")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> LLSwitch(Int64 quizId, Lifeline lifeline, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    lifeline.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.LLSwitch(lifeline);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="lifeline"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/fetchhint")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> GetHint(Int64 quizId, Lifeline lifeline, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    lifeline.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.GetHint(lifeline);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="question"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/registerquestion")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> RegisterQuestion(Int64 quizId, Question question, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    question.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.RegisterQuestion(question);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="card"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{quizId}/scorecard")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> ScoreCard(Int64 quizId, ScoreCard card, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    card.QuizId = quizId;
                    httpResponse = await _gameplayBlanket.ScoreCard(card);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="GamedayId"></param>
       /// <param name="AttemptId"></param>
       /// <param name="PlatformId"></param>
       /// <param name="QuizId"></param>
       /// <param name="backdoor"></param>
       /// <returns></returns>
        [Route("{userguid}/{QuizId}/settlement")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> Settlement(Int64 GamedayId, Int64 AttemptId, Int64 PlatformId, Int64 QuizId, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();

                    httpResponse = await _gameplayBlanket.Settlement(GamedayId, AttemptId, PlatformId, QuizId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptType"></param>
        /// <param name="QuizId"></param>
        /// <param name="MonthId"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{QuizId}/stats")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> UserStats(Int64 OptType, Int64 QuizId,Int64 MonthId=0, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();
                    
                    httpResponse = await _gameplayBlanket.GetUserStats(OptType, QuizId, MonthId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptType"></param>
        /// <param name="QuizId"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{QuizId}/userbadges")]
        [HttpGet]
        public async Task<ActionResult<HTTPResponse>> UserBadges(Int64 OptType, Int64 QuizId, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();

                    httpResponse = await _gameplayBlanket.GetUserBadges(OptType, QuizId);

                    return Ok(httpResponse);
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptType"></param>
        /// <param name="MonthId"></param>
        /// <param name="QuizId"></param>
        /// <param name="backdoor"></param>
        /// <returns></returns>
        [Route("{userguid}/{QuizId}/badgenotify")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> UpdateBadgeNotify(Int64 OptType, Int64 MonthId, Int64 QuizId, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {

                    HTTPResponse httpResponse = new HTTPResponse();

                    httpResponse = await _gameplayBlanket.UpdateBadgeNotify(OptType, MonthId, QuizId);

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