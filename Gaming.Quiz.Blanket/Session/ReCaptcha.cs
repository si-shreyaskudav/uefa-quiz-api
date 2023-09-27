using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Blanket.Session
{
    public class ReCaptcha : Common.BaseBlanket, IReCaptchaBlanket
    {
        static string _VerifyUrl { get { return "https://www.google.com/recaptcha/api/siteverify"; } }
        private readonly String _RecaptchaSecretKey;

        public ReCaptcha(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset,httpContext)
        {
            _RecaptchaSecretKey = appSettings.Value.Properties.RecaptchaSecretKey;
        }

        public bool Verify(String response)
        {
            bool valid = false;

            try
            {
                if (!String.IsNullOrEmpty(response))
                {
                    var postData = string.Format("&secret={0}&response={1}", _RecaptchaSecretKey, response);

                    String data = GenericFunctions.PostWebData(_VerifyUrl, postData);

                    if (data != null)
                    {
                        dynamic resJson = GenericFunctions.Deserialize<dynamic>(data);

                        valid = (resJson["success"] == true);
                    }
                }
            }
            catch (Exception ex)
            {
                valid = false;
            }

            return valid;
        }
    }
}
