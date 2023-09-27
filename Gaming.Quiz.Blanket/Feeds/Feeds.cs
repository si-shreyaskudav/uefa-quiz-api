using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System;
using Gaming.Quiz.Library.Utility;
using System.Linq;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Feeds;
using Gaming.Quiz.Interfaces.Feeds;

namespace Gaming.Quiz.Blanket.Feeds
{
    public class Feeds : Common.BaseBlanket, IFeedsBlanket
    {

        public Feeds(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
        }

        public async Task<HTTPResponse> GetTranslations(String langCode)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();
            Int32 retVal = -40;
            try
            {
                String data = await _Asset.GET(_Asset.Translation(langCode));
                ResponseObject mResponse = new ResponseObject();

                httpResponse = GenericFunctions.Deserialize<HTTPResponse>(data);

                //mResponse.Value = GenericFunctions.Deserialize<Object>(GenericFunctions.Deserialize<dynamic>(data));
                //mResponse.FeedTime = GenericFunctions.GetFeedTime();
                ////httpResponse.Data = GenericFunctions.Deserialize<ResponseObject>(data);
                //httpResponse.Data = mResponse;

                retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");

            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Feeds.GetTranslations", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }

        public async Task<HTTPResponse> GetTutorial(String langCode)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();
            ResponseObject mResponse = new ResponseObject();

            Int32 retVal = -40;

            try
            {
                String data = await _Asset.GET(_Asset.Tutorial(langCode));


                mResponse.Value = GenericFunctions.Deserialize<Tutoral>(data);
                mResponse.FeedTime = GenericFunctions.GetFeedTime();

                httpResponse.Data = mResponse;
                retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");
            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Feeds.GetTranslations", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }


        public async Task<HTTPResponse> GetLanguages()
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();

            try
            {
                String data = await _Asset.GET(_Asset.Languages());

                httpResponse.Data = GenericFunctions.Deserialize<ResponseObject>(data);

                Int32 retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");

            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Feeds.GetLanguages", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }

        public async Task<HTTPResponse> GetFavPlayers()
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();

            try
            {
                String data = await _Asset.GET(_Asset.FavPlayerFile(_MasterId));

                httpResponse.Data = GenericFunctions.Deserialize<ResponseObject>(data);

                Int32 retVal = httpResponse.Data != null && httpResponse.Data.ToString() != "" ? 1 : -40;

                GenericFunctions.AssetMeta(retVal, ref httpMeta, "Success");

            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Feeds.GetFavPlayers", ex.Message);
                _AWS.Log(httpLog);
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }
    }
}
