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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gaming.Quiz.Interfaces.Feeds;

namespace Gaming.Quiz.Blanket.Feeds
{
    public class Ingestion : Common.BaseBlanket, IIngestionBlanket
    {

        public Ingestion(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
        }

        public async Task<Tuple<int, String>> Languages()
        {
            Int32 retVal = -50;
            string error = string.Empty;
            try
            {
                ResponseObject response = new ResponseObject();
                response.Value = _Lang;
                response.FeedTime = GenericFunctions.GetFeedTime();

                bool success = await _Asset.SET(_Asset.Languages(), response);

                retVal = Convert.ToInt32(success);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(retVal, error);
        }

        public async Task<Tuple<Int32, String>> Translations(String vTrans, String lang = "en")
        {
            Int32 retVal = -50;
            string error = string.Empty;
            bool status = false;
            try
            {
                foreach (string appLangs in _Lang)
                {
                    Dictionary<string,string> appLangTrans = InsertTranslations(vTrans, lang);

                    status = await _Asset.SET(_Asset.Translation(appLangs), GenericFunctions.Serialize(appLangTrans));
                }

                retVal = Convert.ToInt32(status);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, String>(retVal, error); ;
        }

        private Dictionary<string, string> InsertTranslations(String data, string lang)
        {
            List<Dictionary<string, string>> newjson = new List<Dictionary<string, string>>();

            Int32 fetchindex = -1;

            Dictionary<string, string> langtransDic = new Dictionary<string, string>();
            try
            {
                if (!String.IsNullOrEmpty(data))
                {

                    newjson = GenericFunctions.Deserialize<List<Dictionary<String, String>>>(data);

                    for (int i = 0; i < newjson.Count; i++)
                    {
                        Dictionary<string, string> tran = newjson[i].ToDictionary(x => x.Key, x => x.Value);


                        var key = tran.Select(o => o.Key).ToList();
                        var val = tran.Select(o => o.Value).ToList();

                        fetchindex = key.FindIndex(o => o == lang);

                        if (fetchindex != -1)
                        {
                            langtransDic.Add(val[0], val[fetchindex]);
                            fetchindex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return langtransDic;
        }

    }
}
