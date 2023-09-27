using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Gaming.Quiz.Library.Utility;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Feeds;
using System.Text.RegularExpressions;

namespace Gaming.Quiz.Blanket.Session
{
    public class Session : Common.BaseBlanket, ISessionBlanket
    {
        private readonly DataAccess.Session.Session _DBContext;
        private readonly MemCache _MemCache;

        private readonly Int32 _TnCVersion;
        private readonly Int32 _PrivacyPolicyVersion;


        public Session(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext, IMemoryCache cache)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _DBContext = new DataAccess.Session.Session(appSettings, postgre, cookies);
            _TnCVersion = appSettings.Value.Properties.TnCVersion;
            _PrivacyPolicyVersion = appSettings.Value.Properties.PrivacyPolicy;
            _MemCache = new MemCache(cache);
        }

        public HTTPResponse Login(Credentials credentials)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();

            HTTPMeta httpMeta = new HTTPMeta();
            Int64 mUserId = 0;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    mUserId = _Cookies._GetUserCookies.UserId;
                }

                if (_Cookies._HasUserCookies )
                {
                    credentials.OptType = 2;
                }

                //if (_Cookies._HasWAFUSCCookies)
                //{
                //    HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Session.User.Login", _Cookies._HasWAFUSCCookies.ToString());
                //    _AWS.Log(httpLog);
                //}

                //if (_Cookies._HasWAFUSCCookies)
                //{
                //    var c = _Cookies._GetWAFUSCCookies;
                //    HTTPLog httpLog = _Cookies.PopulateLog("UserGuid", c.user_guid);
                //    _AWS.Log(httpLog);
                //}

                UserDetails details = _DBContext.Login(credentials.OptType, credentials.PlatformId, _TourId, _TnCVersion.ToString(), mUserId, credentials.SocialId, credentials.ClientId,
                    credentials.FullName, credentials.EmailId, Convert.ToInt64(credentials.PhoneNo), credentials.CountryCode, ref httpMeta);


                if (httpMeta.RetVal == 1)
                {
                    if (details != null && details.User != null && details.Game != null)
                    {
                        bool success = _Cookies.SetUserCookies(details.User);
                        success = _Cookies.SetGameCookies(details.Game);

                        Int32 retVal = (success) ? 1 : -100;

                        res.Value = retVal;
                        res.FeedTime = GenericFunctions.GetFeedTime();
                        httpResponse.Data = res;

                        GenericFunctions.AssetMeta(retVal, ref httpMeta);
                    }
                    else
                        GenericFunctions.AssetMeta(-40, ref httpMeta, "Details from database is NULL.");
                }
                else
                    GenericFunctions.AssetMeta(httpMeta.RetVal, ref httpMeta, "Error while fetching user details from database.");
            }
            catch (Exception ex)
            {
                httpMeta.Message = ex.Message;
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Session.User.Login", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }

        public HTTPResponse Login(string SocialId)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();
            UserDetails login = new UserDetails();
            ResponseObject res = new ResponseObject();

            try
            {
                login = _DBContext.Login(SocialId, ref httpMeta);

                if (httpMeta.RetVal == 1)
                {
                    if (!string.IsNullOrEmpty(login.User.UserId.ToString()) )
                    {
                        bool success = _Cookies.SetUserCookies(login.User);
                        success = _Cookies.SetGameCookies(login.Game);

                        Int32 retVal = (success) ? 1 : -100;

                        res.Value = retVal;
                        res.FeedTime = GenericFunctions.GetFeedTime();
                        httpResponse.Data = res;

                        GenericFunctions.AssetMeta(retVal, ref httpMeta);

                    }
                    else
                        GenericFunctions.AssetMeta(-40, ref httpMeta, "Details from database is NULL.");
                }
                else
                    GenericFunctions.AssetMeta(httpMeta.RetVal, ref httpMeta, "Error while fetching user details from database.");
            }
            catch (Exception ex)
            {
                httpMeta.Message = ex.Message;
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Session.User.Login", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }
        public async Task<HTTPResponse> AnonymousLogin(Credentials credentials)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();
            UserDetails login = new UserDetails();
            ResponseObject res = new ResponseObject();
            bool IsCaptchVerified = false;

            try
            {
                //if (credentials.CaptchaCode != "")
                //{
                //    IsCaptchVerified = _ReCaptchaContext.Verify(credentials.CaptchaCode);
                //}

                if (IsCaptchVerified)
                {
                    //if ( !string.IsNullOrEmpty(credentials.FullName) ? SpecialCheck(credentials.FullName) : false)
                    {
                        //if (await Check(credentials.FullName))
                        {
                            login = _DBContext.AnonymousLogin(credentials.OptType, _MasterId, credentials.UserId, credentials.SocialId, credentials.FullName, credentials.EmailId, Convert.ToInt64(credentials.PhoneNo), credentials.CountryCode, credentials.ClientId, credentials.PlatformId,_TnCVersion.ToString(), 0, ref httpMeta);

                            if (httpMeta.RetVal == 1)
                            {
                                if (!string.IsNullOrEmpty(login.User.UserId.ToString()))
                                {
                                    bool success = _Cookies.SetUserCookies(login.User);
                                    success = _Cookies.SetGameCookies(login.Game);

                                    Int32 retVal = (success) ? 1 : -100;

                                    res.Value = retVal;
                                    res.FeedTime = GenericFunctions.GetFeedTime();
                                    httpResponse.Data = res;

                                    GenericFunctions.AssetMeta(retVal, ref httpMeta);

                                }
                                else
                                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Details from database is NULL.");
                            }
                            else
                                GenericFunctions.AssetMeta(httpMeta.RetVal, ref httpMeta, "Error while fetching user details from database.");
                        }
                        //else
                        //    GenericFunctions.AssetMeta(-90, ref httpMeta, "User Name profanity check failed");
                    }
                    //else
                    //    GenericFunctions.AssetMeta(-100, ref httpMeta, "User Name Have special characters");
                }
                else
                    GenericFunctions.AssetMeta(-30, ref httpMeta, "recaptcha verification fail.");
            }
            catch (Exception ex)
            {
                httpMeta.Message = ex.Message;
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Session.User.Login", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }

        public async Task<HTTPResponse> WafLogin(Credentials credentials)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 mUserId = 0;

            bool success = false;

            GameCookie gameCookie = new GameCookie();
            UserCookie userCookie = new UserCookie();

            try
            {
                if (credentials.OptType == 2)
                {
                    if (_Cookies._HasUserCookies)
                    {
                        mUserId = _Cookies._GetUserCookies.UserId;
                    }
                }

                if (credentials.OptType == 1 && credentials.EmailId == null)
                {
                    credentials.EmailId = String.Empty;
                }

                UserLoginDBResp details = _DBContext.WafLogin(credentials.OptType, credentials.PlatformId, _TourId, mUserId, credentials.SocialId, credentials.ClientId,
                    credentials.FullName, credentials.EmailId, credentials.PhoneNo, credentials.CountryCode, credentials.ProfilePicture, credentials.DOB,
                    credentials.userCreatedDate, _TnCVersion, _PrivacyPolicyVersion, ref httpMeta);

                if (httpMeta.RetVal == 1)
                {
                    gameCookie = new GameCookie()
                    {
                        GUID = details.Usrguid,
                        WAF_GUID = details.Usrguid,
                        ClientId = details.Usrclnid.ToString(),
                        CoinTotal = details.coinTotal == null ? 0 : details.coinTotal.Value,
                        SocialId = BareEncryption.BaseEncrypt(credentials.SocialId)
                    };
                    userCookie = new UserCookie()
                    {
                        SocialId = credentials.SocialId,
                        UserId = details.Usrid == null ? 0 : details.Usrid.Value,
                        FullName = details.Usrname,
                        TeamId = details.Teamid == null ? 0 : details.Teamid.Value,
                        //QzMasterID=details.qu
                        
                    };


                    success = _Cookies.SetGameCookies(gameCookie);
                    success = _Cookies.SetUserCookies(userCookie);
                }
                else if (httpMeta.RetVal == 3)
                {
                    GenericFunctions.AssetMeta(httpMeta.RetVal, ref httpMeta, "Email id already exists.");
                }
                else
                    GenericFunctions.AssetMeta(httpMeta.RetVal, ref httpMeta, "Error while fetching user details from database.");

                return OkResponse(gameCookie, httpMeta);

            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Session.User.Login", ex.Message);
                _AWS.Log(httpLog);

                return CatchResponse(ex.Message);
            }
        }

        #region "Helper"

        public async Task<bool> Check(String searchText)
        {
            bool valid = true;
            try
            {
                if (String.IsNullOrEmpty(searchText))
                    return false;

                searchText = searchText.Trim().ToLower();
                ProfanityWords profaneWords = new ProfanityWords();
                #region " Fetch "
                String data = _MemCache.CacheTryGet(_MemCache._ProfanityToken);
                if (String.IsNullOrEmpty(data))
                {
                    data = await _Asset.GET(_Asset.Profanity());
                    _MemCache.CacheTrySet(_MemCache._ProfanityToken, data, TimeSpan.FromDays(1));
                }
                profaneWords = GenericFunctions.Deserialize<ProfanityWords>(data);
                #endregion
                #region " Logic "
                if (searchText.IndexOf(" ") > -1)
                {
                    String[] texts = searchText.Split(" ");
                    foreach (String t in texts)
                    {
                        if (profaneWords.words.Where(c => c.ToLower() == t).Count() > 0)
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                else if (profaneWords.words.Where(c => c.ToLower() == searchText).Count() > 0)
                    valid = false;
                #endregion
            }
            catch (Exception ex)
            {
                valid = true;
                throw ex;
            }
            return valid;
        }

        public bool SpecialCheck(string userName)
        {
            bool success = false;
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (regexItem.IsMatch(userName))
            {
                success = true;
            }

            return success;
        }
        #endregion
    }
}