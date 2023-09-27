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
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Gameplay;
using Gaming.Quiz.Interfaces.Gameplay;

namespace Gaming.Quiz.Blanket.Gameplay
{
    public class Gameplay : Common.BaseBlanket, IGameplayBlanket
    {

        protected readonly Gaming.Quiz.DataAccess.Gameplay.Gameplay _Gameplay;
        public Gameplay(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _Gameplay = new DataAccess.Gameplay.Gameplay(appSettings, postgre, cookies);
        }

        public async Task<HTTPResponse> PlayAttempt(PlayAttempt attempt)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (attempt.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.PlayAttempt(uc.UserId, attempt.GamedayId, _SportsId, _CategoryId, attempt.QuizId, attempt.AttemptNo, attempt.Lang, GetIPAdress(), attempt.PlatformId, ref httpMeta);

                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.PlayAttempt ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> LLFiftyFifty(Lifeline lifeline)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            Int64 OptType = 1;
            try
            {

                if (_Cookies._HasUserCookies)
                {
                    if (lifeline.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.LLFiftyFifty(OptType, uc.UserId, lifeline.QuizId, _SportsId, _CategoryId, lifeline.UsrAttemptId, lifeline.QzQstMid, lifeline.GamedayId, lifeline.Lang, lifeline.QuizId, ref httpMeta);
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.LLFiftyFifty ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> LLSneakPeak(Lifeline lifeline, Int64 OptType)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (lifeline.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.LLSneakPeak(OptType, uc.UserId, lifeline.QuizId, _SportsId,
                            _CategoryId, lifeline.UsrAttemptId, lifeline.QzQstMid, lifeline.SelectedAns,
                            lifeline.GamedayId, lifeline.Lang, lifeline.QuizId, ref httpMeta);

                        if (httpMeta.RetVal == 11)
                        {
                            httpMeta.Success = true;
                        }
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");
                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.LLSneakPeak ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> LLSwitch(Lifeline lifeline)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            Int64 OptType = 1;

            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (lifeline.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.LLSwitch(OptType, uc.UserId, lifeline.QuizId, _SportsId, _CategoryId,
                            lifeline.UsrAttemptId, lifeline.QzQstMid, lifeline.GamedayId, lifeline.Lang, lifeline.QuizId, ref httpMeta);
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");
                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.LLSneakPeak ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> GetHint(Lifeline lifeline)
        {
            Int64 Optype = 1;
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (lifeline.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.GetHint(Optype, uc.UserId, lifeline.QuizId, _SportsId,
                            _CategoryId, lifeline.UsrAttemptId, lifeline.QzQstMid, lifeline.GamedayId, lifeline.QuizId, ref httpMeta);

                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.LLSneakPeak ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> RegisterQuestion(Question question)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (question.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.RegisterQuestion(uc.UserId, question.GamedayId, _SportsId, _CategoryId, question.QuizId, question.QzAtmpId, question.Lang,
                                question.QstMId, GetIPAdress(), question.SltdAnsOptn, question.PlatformId, question.TimeSpent, question.AtmptStatus,
                                question.ResumeAtmp, question.HintCnt, question.QuizId, ref httpMeta);

                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                if (httpMeta.RetVal == 2)
                    httpMeta.Success = true;

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.RegisterQuestion ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> GetUserDetails(Int64 quizId)
        {
            Int64 Optype = 1;
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasWAFUSCCookies && _Cookies._GetWAFUSCCookies != null)
                {
                    if (_Cookies._HasUserCookies)
                    {
                        if (quizId > 0)
                        {
                            UserCookie uc = _Cookies._GetUserCookies;
                            String mFavPlayerId = "";
                            data = _Gameplay.GetUserDetails(Optype, uc.UserId, quizId, _SportsId, _CategoryId, ref mFavPlayerId, ref httpMeta);

                            if (!String.IsNullOrEmpty(mFavPlayerId))
                            {
                                if (_Cookies._HasGameCookies)
                                {
                                    //GameCookie g= _Cookies._GetGameCookies;

                                    GameCookie gameDetails = new GameCookie()
                                    {
                                        FavPlayer = mFavPlayerId
                                    };
                                    bool status = _Cookies.UpdateGameCookies(gameDetails);
                                }
                            }

                        }
                        else
                        {
                            GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                        }
                    }
                    else
                        GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");
                }
                else
                {
                    _Cookies.DeleteGameCookies();
                    GenericFunctions.AssetMeta(-95, ref httpMeta, "Not Authorized");
                }
                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.GetUserDetails ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> ScoreCard(ScoreCard card)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            Int64 OptType = 1;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (card.QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.GetScoreCard(OptType, uc.UserId, card.QuizId, card.GamedayId, _SportsId, _CategoryId, card.AttemptId, ref httpMeta);
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.ScoreCard ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> Settlement(Int64 GamedayId, Int64 AttemptId, Int64 PlatformID, Int64 QuizId)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        retVal = _Gameplay.Settlement(uc.UserId, QuizId, _SportsId, _CategoryId, GamedayId, AttemptId, PlatformID, GetIPAdress(), ref httpMeta);
                        data.Value = retVal;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }

                }
                else
                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.Settlement ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> GetUserStats(Int64 OptType, Int64 QuizId, Int64 MonthId)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;

            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        data = _Gameplay.GetUserStats(OptType, _SportsId, _CategoryId, QuizId, uc.UserId, MonthId, ref httpMeta);
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }
                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.GetUserStats ==> " + ex.Message);
            }
        }
        public async Task<HTTPResponse> GetUserBadges(Int64 OptType, Int64 QuizId)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;

            try
            {
                if (_Cookies._HasUserCookies)
                {

                    UserCookie uc = _Cookies._GetUserCookies;

                    data = _Gameplay.GetUserBadges(OptType, uc.UserId, QuizId, ref httpMeta);

                }
                else
                    GenericFunctions.AssetMeta(-40, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.GetUserBadges ==> " + ex.Message);
            }
        }

        public async Task<HTTPResponse> UpdateBadgeNotify(Int64 OptType, Int64 MonthId, Int64 QuizId)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    if (QuizId > 0)
                    {
                        UserCookie uc = _Cookies._GetUserCookies;

                        retVal = _Gameplay.UpdateBadgeNotify(OptType,uc.UserId, MonthId, QuizId, ref httpMeta);
                        data.Value = retVal;
                        data.FeedTime = GenericFunctions.GetFeedTime();
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-41, ref httpMeta, "Invalid QuizId");
                    }

                }
                else
                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Not Authorized");

                return OkResponse(data, httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Gameplay.UpdateBadgeNotify ==> " + ex.Message);
            }
        }

    }
}
