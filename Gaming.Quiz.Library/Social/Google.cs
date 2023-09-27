using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Contracts.Social;
using Gaming.Quiz.Library.Utility;
using Microsoft.Extensions.Options;
using System;
using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Interfaces.Storage;

namespace Gaming.Quiz.Library.Social
{
    public class Google : BaseOAuth
    {

        public Google(IOptions<Application> application) : base(application)
        {
        }

        public Credentials Callback(String code, String callbackUrl)
        {
            String data = "";
            GoogleUser mSocialUser = new GoogleUser();
            Credentials mUserCredentials = new Credentials();
            try
            {
                string tokenParam = string.Empty;
                string response = string.Empty;


                tokenParam = $"code={System.Web.HttpUtility.UrlEncode(code)}&client_id={System.Web.HttpUtility.UrlEncode(_Settings.Google.Id)}&client_secret={System.Web.HttpUtility.UrlEncode(_Settings.Google.Secret)}&redirect_uri={System.Web.HttpUtility.UrlEncode(callbackUrl)}&grant_type=authorization_code";
                response = GenericFunctions.PostWebData(_Settings.Google.Auth.Token, tokenParam);

                if (response != null && response.Length > 0)
                {

                    String token = "";
                    dynamic val = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);

                    if (val["access_token"] != null)
                        token = val["access_token"];

                    string profile = String.Format(_Settings.Google.Auth.Profile, token);
                    data = GenericFunctions.GetSocialWebData(profile, $"OAuth {token}");

                    if (!String.IsNullOrEmpty(data))
                    {
                        mSocialUser = GenericFunctions.Deserialize<GoogleUser>(data);
                    }
                    if (mSocialUser != null)
                    {
                        mUserCredentials = new Credentials()
                        {
                            ClientId = 2,
                            PlatformId = 1,
                            FullName = mSocialUser.name,
                            SocialId = mSocialUser.id,
                            EmailId = mSocialUser.email,
                            PhoneNo = mSocialUser.phoneno.ToString(),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Google Callback: " + ex.Message);
            }

            return mUserCredentials;
        }
    }
}
