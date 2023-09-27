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
using Gaming.Quiz.Contracts.Leaderboard;
using Gaming.Quiz.Contracts.Feeds;
using Gaming.Quiz.Interfaces.Leaderboard;

namespace Gaming.Quiz.Blanket.Leaderboard
{
    public class Leaderboard : Common.BaseBlanket, ILeaderboardBlanket
    {

        protected readonly Gaming.Quiz.DataAccess.Leaderboard.Leaderboard _Leaderboard;
        public Leaderboard(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _Leaderboard = new DataAccess.Leaderboard.Leaderboard(appSettings, postgre, cookies);
        }


        public async Task<HTTPResponse> GetMonth(bool offloadDB = true)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 OptType = 1;
            try
            {
                if (offloadDB)
                {
                    string mData = await _Asset.GET(_Asset.MonthFile(0));

                    data = GenericFunctions.Deserialize<ResponseObject>(mData);
                }
                else
                    data = _Leaderboard.GetMonth(OptType, _MasterId, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetMonth ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetWeekFile(bool offloadDB = true)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 OptType = 1;
            try
            {
                if (offloadDB)
                {
                    string mData = await _Asset.GET(_Asset.WeekFile(_MasterId));

                    data = GenericFunctions.Deserialize<ResponseObject>(mData);
                }
                else
                    data = _Leaderboard.GetWeekFile(OptType, _MasterId, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetWeekFile ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetGamedays(bool offloadDB = true)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 OptType = 1;
            try
            {
                if (offloadDB)
                {
                    string mData = await _Asset.GET(_Asset.GamedayFile(0));

                    data = GenericFunctions.Deserialize<ResponseObject>(mData);
                }
                else
                    data = _Leaderboard.GetGamedays(OptType, _MasterId, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetGamedays ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetFavPlayers(bool offloadDB = true)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 OptType = 1;
            try
            {
                if (offloadDB)
                {
                    string mData = await _Asset.GET(_Asset.FavPlayerFile(_MasterId));

                    data = GenericFunctions.Deserialize<ResponseObject>(mData);
                }
                else
                    data = _Leaderboard.GetFavPlayer(OptType, _MasterId, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetFavPlayers ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetBadges(bool offloadDB = true)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 OptType = 1;
            try
            {
                if (offloadDB)
                {
                    string mData = await _Asset.GET(_Asset.BadgesFile(_MasterId));

                    data = GenericFunctions.Deserialize<ResponseObject>(mData);
                }
                else
                    data = _Leaderboard.GetBadgesList(OptType, _MasterId, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetBadges ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetUserRank(UserRankInput userRank)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    UserCookie uc = _Cookies._GetUserCookies;

                    data = _Leaderboard.GetUserRank(userRank.OptType, uc.UserId, _SportsId, _CategoryId, userRank.QuizId, userRank.GamedayId, userRank.WeekId, userRank.MonthId, ref httpMeta);
                }
                else
                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Not Authorized");
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetUserRank ==> " + ex.Message + "QuizId=" + _Cookies._GetUserCookies.QzMasterID);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetTeamPlayerRank(UserPlayerRankInput userPlayerRank)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    UserCookie uc = _Cookies._GetUserCookies;

                    data = _Leaderboard.GetUserTeamRank(userPlayerRank.OptType, uc.UserId, _SportsId, _CategoryId, userPlayerRank.QuizId, userPlayerRank.PlayerId, ref httpMeta);
                }
                else
                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Not Authorized");
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetUserRank ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> GetBestScoreUserRank(UserRankInput userRank)
        {
            ResponseObject data = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {
                if (_Cookies._HasUserCookies)
                {
                    UserCookie uc = _Cookies._GetUserCookies;

                    data = _Leaderboard.GetBestScoreUserRank(userRank.OptType, uc.UserId, _SportsId, _CategoryId, uc.QzMasterID, userRank.GamedayId, userRank.WeekId, userRank.MonthId, ref httpMeta);
                }
                else
                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Not Authorized");
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.GetUserRank ==> " + ex.Message);
            }

            return OkResponse(data, httpMeta);
        }

        public async Task<HTTPResponse> BestScoreLeaderboard(Int32 OptType, Int64 MonthId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {

                if (offloadDB)
                {
                    String data = "";


                    data = await _Asset.GET(_Asset.BestScoreLeaderboard(OptType, MonthId));

                    if (data != "")
                    {
                        res = GenericFunctions.Deserialize<ResponseObject>(data);
                        httpResponse.Data = res;
                    }

                    retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");
                }
                else
                    res = _Leaderboard.BestScoreLeaderboard(OptType, GamedayId, WeekId, MonthId, _SportsId, _CategoryId, _MasterId, pageNo, mTopNo, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.BestScoreLeaderboard ==> " + ex.Message);
            }

            return OkResponse(res, httpMeta);
        }

        public async Task<HTTPResponse> MonthLeaderboard(Int32 OptType, Int64 MonthId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {

                if (offloadDB)
                {
                    String data = "";

                    if (OptType == 1)
                        data = await _Asset.GET(_Asset.MonthLeaderboard(MonthId, 0));
                    else
                        data = await _Asset.GET(_Asset.GamedayLeaderboard(GamedayId, 0));

                    if (data != "")
                    {
                        res = GenericFunctions.Deserialize<ResponseObject>(data);
                        httpResponse.Data = res;
                    }

                    retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");
                }
                else
                    res = _Leaderboard.MonthLeaderboard(OptType, GamedayId, WeekId, MonthId, _SportsId, _CategoryId, _MasterId, pageNo, mTopNo, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.MonthLeaderBoard ==> " + ex.Message);
            }

            return OkResponse(res, httpMeta);
        }


        public async Task<HTTPResponse> OverAllLeaderboard(Int32 OptType, Int64 pageNo = 1, Int64 mTopNo = 2000, bool offloadDB = true)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {

                if (offloadDB)
                {
                    String data = "";


                    data = await _Asset.GET(_Asset.OverallLeaderboard(OptType, 0));

                    if (data != "")
                    {
                        res = GenericFunctions.Deserialize<ResponseObject>(data);
                        httpResponse.Data = res;
                    }

                    retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");
                }
                else
                    res = _Leaderboard.OverallLeaderboard(OptType, _SportsId, _CategoryId, _MasterId, pageNo, mTopNo, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.OverallLeaderboard ==> " + ex.Message);
            }

            return OkResponse(res, httpMeta);
        }

        public async Task<HTTPResponse> TeamPlayerLeaderboard(Int32 OptType, Int64 PlayerId, Int64 pageNo = 1, Int64 mTopNo = 2000, Int64 GamedayId = 0, Int64 WeekId = 0, bool offloadDB = true)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            ResponseObject res = new ResponseObject();
            HTTPMeta httpMeta = new HTTPMeta();
            Int64 retVal = -40;
            try
            {

                if (offloadDB)
                {
                    String data = "";


                    data = await _Asset.GET(_Asset.PlayerLeaderboard(PlayerId,_MasterId));

                    if (data != "")
                    {
                        res = GenericFunctions.Deserialize<ResponseObject>(data);
                        httpResponse.Data = res;
                    }

                    retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                    GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");
                }
                else
                    res = _Leaderboard.TeamLeaderboard(OptType, PlayerId, GamedayId, WeekId, _SportsId, _CategoryId, _MasterId, pageNo, mTopNo, ref httpMeta);
            }
            catch (Exception ex)
            {
                return CatchResponse("Blanekt.Leaderboard.TeamPlayerLeaderboard ==> " + ex.Message);
            }

            return OkResponse(res, httpMeta);
        }

        public async Task<Tuple<Int32, string>> GenBestScoreLeaderboard(Int64 GamedayId, Int64 WeekId, Int64 MonthId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                Int32 pageNo = 1;
                Int32 mTopNo = 2000;
                Int32 mOptType = 1;

                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();


                httpResponse = await BestScoreLeaderboard(mOptType, MonthId, pageNo, mTopNo, GamedayId, WeekId, false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.BestScoreLeaderboard(mOptType, MonthId), httpResponse.Data);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }
        public async Task<Tuple<Int32, string>> GenMonthLeaderboard(Int64 GamedayId, Int64 WeekId, Int64 MonthId, Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                Int32 pageNo = 1;
                Int32 mTopNo = 2000;
                Int32 mOptType = 1;

                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                if (MonthId != 0)
                {
                    httpResponse = await MonthLeaderboard(mOptType, MonthId, pageNo, mTopNo, GamedayId, WeekId, false);

                    if (httpResponse.Meta.RetVal == 1)
                    {
                        success = await _Asset.SET(_Asset.MonthLeaderboard(MonthId, QuizId), httpResponse);

                        retVal = Convert.ToInt32(success);
                    }
                }
                if (GamedayId != 0)
                {
                    mOptType = 2;
                    httpResponse = await MonthLeaderboard(mOptType, MonthId, pageNo, mTopNo, GamedayId, WeekId, false);

                    if (httpResponse.Meta.RetVal == 1)
                    {
                        success = await _Asset.SET(_Asset.GamedayLeaderboard(GamedayId, QuizId), httpResponse);

                        retVal = Convert.ToInt32(success);
                    }
                }

                if (WeekId != 0)
                {
                    mOptType = 3;
                    httpResponse = await MonthLeaderboard(mOptType, MonthId, pageNo, mTopNo, GamedayId, WeekId, false);

                    if (httpResponse.Meta.RetVal == 1)
                    {
                        success = await _Asset.SET(_Asset.WeeklyLeaderboard(WeekId, QuizId), httpResponse);

                        retVal = Convert.ToInt32(success);
                    }
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> GenTeamLeaderboard(Int64 PlayerId, Int64 GamedayId, Int64 WeekId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                Int32 pageNo = 1;
                Int32 mTopNo = 2000;
                Int32 mOptType = 1;

                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();


                httpResponse = await TeamPlayerLeaderboard(mOptType, PlayerId, pageNo, mTopNo, GamedayId, WeekId, false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.PlayerLeaderboard(PlayerId,_MasterId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }


        public async Task<Tuple<Int32, string>> GenMonthFile(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                httpResponse = await GetMonth(false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.MonthFile(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> GenGamedayFile(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                httpResponse = await GetGamedays(false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.GamedayFile(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> GenWeekFile(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                httpResponse = await GetWeekFile(false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.WeekFile(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }


        public async Task<Tuple<Int32, string>> GenFavPlayerFile(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                httpResponse = await GetFavPlayers(false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.FavPlayerFile(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> GetMixApi(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();
                ResponseObject res = new ResponseObject();
                MixAPI mix = new MixAPI();

                string data = await _AWS.Get(_Asset.MixApi(QuizId));
                if (!String.IsNullOrEmpty(data))
                {
                    httpResponse = GenericFunctions.Deserialize<HTTPResponse>(data);
                    res = GenericFunctions.Deserialize<ResponseObject>(GenericFunctions.Serialize(httpResponse.Data));
                    mix = GenericFunctions.Deserialize<MixAPI>(GenericFunctions.Serialize(res.Value));
                    //mix = new MixAPI() { ldrbd = DateTime.UtcNow.ToString("yyyyMMddHHmmss") };
                    mix.ldrbd = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    res.Value = mix;
                    res.FeedTime = GenericFunctions.GetFeedTime();
                    GenericFunctions.AssetMeta(1, ref httpMeta);
                    httpResponse.Data = res;
                    httpResponse.Meta = httpMeta;
                }
                else
                {
                    mix = new MixAPI() { ldrbd = DateTime.UtcNow.ToString("yyyyMMddHHmmss") };
                    res.Value = mix;
                    res.FeedTime = GenericFunctions.GetFeedTime();
                    GenericFunctions.AssetMeta(1, ref httpMeta);
                    httpResponse.Data = res;
                    httpResponse.Meta = httpMeta;

                }



                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.MixApi(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }


        public async Task<Tuple<Int32, string>> GenOverallLeaderboard(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                Int32 pageNo = 1;
                Int32 mTopNo = 2000;
                Int32 mOptType = 1;

                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();


                httpResponse = await OverAllLeaderboard(mOptType, pageNo, mTopNo, false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.OverallLeaderboard(mOptType, QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, string>> GenBadgesFeed(Int64 QuizId)
        {
            Int32 retVal = -50;
            bool success = false;
            string error = string.Empty;

            try
            {
                HTTPResponse httpResponse = new HTTPResponse();
                HTTPMeta httpMeta = new HTTPMeta();

                httpResponse = await GetBadges(false);

                if (httpResponse.Meta.RetVal == 1)
                {
                    success = await _Asset.SET(_Asset.BadgesFile(QuizId), httpResponse);

                    retVal = Convert.ToInt32(success);
                }

                if (httpResponse.Meta.Success == false)
                {
                    error = httpResponse.Meta.Message;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

    }
}
