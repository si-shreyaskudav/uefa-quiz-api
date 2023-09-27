using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Contracts.Social;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.Extensions.Options;
using System;

namespace Gaming.Quiz.Library.Social
{
    public class Facebook : BaseOAuth
    {
        public Facebook(IOptions<Application> application) : base(application)
        {
        }

        public Credentials Callback(String code, String callbackUrl)
        {
            HTTPLog logger = new HTTPLog();

            String data = "";
            SocialUser mSocialUser = new SocialUser();
            Credentials mUserCredentials = new Credentials();
            try
            {
                string accessTokenUrl = String.Format(_Settings.Facebook.Auth.Token, _Settings.Facebook.Id, callbackUrl, _Settings.Facebook.Secret, code);
                string response = GenericFunctions.GetWebData(accessTokenUrl);

                if (response != null && response.Length > 0)
                {
                    String token = "";
                    dynamic val = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);

                    if (val["access_token"] != null)
                        token = val["access_token"];

                    string profile = String.Format(_Settings.Facebook.Auth.Profile, token);

                        data = GenericFunctions.GetWebData(profile);
                    

                    if (!String.IsNullOrEmpty(data))
                    {
                            mSocialUser = GenericFunctions.Deserialize<SocialUser>(data);
                    }
                    if (mSocialUser != null)
                    {
                        mUserCredentials = new Credentials()
                        {
                            ClientId = 1,
                            PlatformId = 1,
                            FullName = mSocialUser.name,
                            SocialId = mSocialUser.id,
                            EmailId = mSocialUser.email,
                            PhoneNo = mSocialUser.phoneno.ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Facebook Callback: " + ex.Message);
            }

            return mUserCredentials;
        }
    }
}
