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
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Gaming.Quiz.API.Controllers.Session
{
    [Route("services/[controller]")]
    [ApiController]
    public class SessionController : BaseController
    //public partial class GameplayController: BaseController
    {
        private readonly String _AuthKey;
        private readonly ISessionBlanket _sessionBlanket;

        public SessionController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContext, IMemoryCache cache, ISessionBlanket sessionBlanket)
               : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            
            _AuthKey = appSettings.Value.Properties.WAFAuthKey;
            this._sessionBlanket = sessionBlanket;
        }


       

        [Route("login")]
        [HttpPost]
        public ActionResult<HTTPResponse> Login(string SocialId, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = _sessionBlanket.Login(SocialId);

                    return Ok(response);
                }
                else
                    return Unauthorized();

            }
            else
                return BadRequest();
        }

        [Route("anonymouslogin")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> AnonymousLogin(Credentials credentials, String backdoor = null)
        {
            if (ModelState.IsValid)
            {
                if (_Authentication.Validate(backdoor))
                {
                    HTTPResponse response = await _sessionBlanket.AnonymousLogin(credentials);

                    return Ok(response);
                }
                else
                    return Unauthorized();

            }
            else
                return BadRequest();
        }


        [Route("user/login")]
        [HttpPost]
        public async Task<ActionResult<HTTPResponse>> UserLogin(String waf_guid)
        {

            HTTPResponse response;
            String mGUID = String.Empty;
            Credentials mCredentials = new Credentials();
            Credentials credentials = new Credentials();
            ResponseObject rObj = new ResponseObject();
            Dictionary<String, String> mAuthHeaders = new Dictionary<string, string>();
            HTTPMeta meta = new HTTPMeta();

            String mUserYahooGuid = String.Empty; //credentials.SocialId;
            String _USCCookie = String.Empty; //credentials.SocialId;
            String _URCCookie = String.Empty; //credentials.SocialId;

            String mProfileUrl = String.Empty;
            String msg = String.Empty;
            #region " Validate WAF-USC Cookie "

            //if (_HttpContext.HttpContext.Request.Headers["uscwebview"].Any() /*&& _HttpContext.HttpContext.Request.Headers["user_socialid"].Any()*/)
            //{
            //    _USCCookie = HttpContext.Request.Headers["uscwebview"].ToString();
            //    mGUID = Guid.NewGuid().ToString();//_HttpContext.HttpContext.Request.Headers["user_socialid"].ToString();
            //    mProfileUrl = _AppSettings.Value.Properties.WAFProfileUrl + "?token=" + mGUID+ "&is_app=1"; ;
            //}
            //else
            {
                if (_Cookies._HasWAFUSCCookies)
                {

                    //WAFCookie wAFCookie = _Cookies._GetWAFURCCookies;

                    //mUserYahooGuid = _Cookies._GetWAFUSCCookies;
                    _USCCookie = _Cookies._GetWAFUSCCookies;
                    if (_USCCookie != null)
                    {
                        mGUID = String.IsNullOrEmpty(waf_guid) ? Guid.NewGuid().ToString() : waf_guid;
                        //mProfileUrl = _AppSettings.Value.Properties.WAFProfileUrl + "?token=" + mGUID;
                        mProfileUrl = _AppSettings.Value.Properties.WAFProfileUrl + "?token=" + mGUID + "&is_app=1";
                    }
                    else
                    {
                        bool success = _Cookies.DELETE();
                        response = new HTTPResponse();
                        rObj.Value = new object();
                        rObj.FeedTime = GenericFunctions.GetFeedTime();
                        response.Data = rObj;
                        GenericFunctions.AssetMeta(-995, ref meta);
                        response.Meta = meta;
                        return Ok(response);
                        //return Unauthorized();
                        //response.Meta.Message += "_HasWAFUSCCookies null";
                        //return Ok(response);
                    }


                }
                else
                {
                    bool success = _Cookies.DELETE();
                    response = new HTTPResponse();
                    rObj.Value = new object();
                    rObj.FeedTime = GenericFunctions.GetFeedTime();
                    response.Data = rObj;
                    GenericFunctions.AssetMeta(-997, ref meta);
                    response.Meta= meta;
                    return Ok(response);
                    //return Unauthorized();
                    //response.Meta.Message += "_HasWAFUSCCookies null";
                    //return Ok(response);
                }
            }

            //mAuthHeaders.Add("user_guid", mUserYahooGuid);
            //mAuthHeaders.Add("auth", _AppSettings.Value.Properties.WAFAuthKey);
            //mAuthHeaders.Add("User-Agent", _AppSettings.Value.Properties.WAFUserAgent);

            mAuthHeaders.Add("usertoken", _USCCookie);
            mAuthHeaders.Add("auth", _AppSettings.Value.Properties.WAFAuthKey);

            #endregion

            #region " User Authentication "


            response = Library.Session.Session.ValidateUser(mProfileUrl, mAuthHeaders, mUserYahooGuid);

            if (response.Meta.RetVal == -999)
            {
                return Ok(response);
            }
            else if (response.Meta.RetVal == 2)
            {
                return Ok(response);
                //return new RedirectResult(_AppSettings.Value.Redirect.ProfileIncomplete);
            }

            #endregion

            //mGUID = response.Meta.Message;

            mCredentials = (Credentials)response.Data;

            if (mCredentials == null)
            {
                bool success = _Cookies.DELETE();
                response = new HTTPResponse();
                rObj.Value = new object();
                rObj.FeedTime = GenericFunctions.GetFeedTime();
                response.Data = rObj;
                GenericFunctions.AssetMeta(-996, ref meta);
                response.Meta = meta;
                return Ok(response);
                //return Unauthorized();
                //response.Meta.Message += "Credentials null"+GenericFunctions.Serialize(mCredentials);
                //return Ok(response);
            }
            else
            {
                credentials.SocialId = mCredentials.SocialId;
                credentials.FullName = mCredentials.FullName;
                credentials.EmailId = mCredentials.EmailId;
                credentials.ClientId = 1;
                credentials.OptType = 1;
                credentials.PlatformId = 3;
                credentials.UserId = 0;
                credentials.DOB = mCredentials.DOB;
                credentials.PhoneNo = mCredentials.PhoneNo;
                credentials.userCreatedDate = mCredentials.userCreatedDate;

                #region " Validate SI Login "

                response = await _sessionBlanket.WafLogin(credentials);
                ////response.Meta.Message = mUserYahooGuid;
                //if (response.Meta.RetVal == 1)
                //    return new RedirectResult(_AppSettings.Value.Redirect.PostLogin);
                //else
                //    return new RedirectResult(_AppSettings.Value.Redirect.PreLogin);
                ////return Ok(response);

                return Ok(response);

                #endregion
            }

            
        }

    }
}