using System;
using System.Collections.Generic;

namespace  Gaming.Quiz.Contracts.Configuration
{
    public class Application
    {
        public Properties Properties { get; set; }
        public Connection Connection { get; set; }
        public SMTP SMTP { get; set; }
        public Cookies Cookies { get; set; }
        public API API { get; set; }
        public Admin Admin { get; set; }
        public Redirect Redirect { get; set; }
        public Settings Settings { get; set; }

        public CustomSwaggerConfig CustomSwaggerConfig { get; set; }
    }

    #region "Children "
    public class CustomSwaggerConfig
    {
        public String BasePath { get; set; }
    }

    public class Settings
    {
        public FBOAuth Facebook { get; set; }
        public GoogleOAuth Google { get; set; }
        public String CallBackUrl { get; set; }
        public String RedirectUrl { get; set; }
    }

    public class FBOAuth
    {
        public String Id { get; set; }
        public String Secret { get; set; }
        public Auth Auth
        {
            get
            {
                return new Auth()
                {
                    Authorize = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope=email&state=fb",
                    Token = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                    Profile = "https://graph.facebook.com/v3.2/me?fields=id,name,email&access_token={0}"
                };
            }
        }
    }

    public class GoogleOAuth
    {
        public String Id { get; set; }
        public String Secret { get; set; }
        public Auth Auth
        {
            get
            {
                return new Auth()
                {
                    Authorize = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri={0}&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id={1}&state=google",
                    Token = "https://accounts.google.com/o/oauth2/token",
                    Profile = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}"
                };
            }
        }
    }
    public class Properties
    {
        public Int32 TourId { get; set; }
        public Int64 QuizMasterId { get; set; }
        public Int64 QuizSportsId { get; set; }
        public Int64 QuizCategoryId { get; set; }
        public String WAFProfileUrl { get; set; }
        public String WAFAuthKey { get; set; }
        public String WAFUserAgent { get;set; }
        public Int32 TnCVersion { get; set; }
        public Int32 PrivacyPolicy { get; set; }
        public String RecaptchaSecretKey { get; set; }

        public List<String> Languages { get; set; }
        public String SiteUrl { get; set; }
        public String ClientName { get; set; }

        public String LogFile { get; set; }
        public String StaticAssetBasePath { get; set; }
        public bool IsServer { get; set; }
        //Changes for timeStart and timeEnd
        public string timestart { get; set; }
        public string timeend { get; set; }
        public string ScheduleTime { get; set; }
        public int Bonus { get; set; }
    }
    public class Auth
    {
        public String Authorize { get; set; }
        public String Token { get; set; }
        public String Profile { get; set; }
    }

    public class Connection
    {
        public String Environment { get; set; }
        public AWSConfig AWS { get; set; }
        public Redis Redis { get; set; }
        public Postgre Postgre { get; set; }
    }

    public class AWSConfig
    {
        public SQS SQS { get; set; }
        public String S3Bucket { get; set; }
        public String S3FolderPath { get; set; }
        public bool Apply { get; set; }
        public bool UseCredentials { get; set; }
    }

    public class SQS
    {
        public String NotificationQueueUrl { get; set; }
        public String EventsQueueUrl { get; set; }
        public String TrackingQueueUrl { get; set; }
        public String ServiceUrl { get; set; }
    }

    public class Redis
    {
        public String Server { get; set; }
        public Int32 Port { get; set; }
        public bool Apply { get; set; }
    }

    public class Postgre
    {
        public String ConnectionString { get; set; }
        public String PointCalConn { get; set; }
        public bool Pooling { get; set; }
        public Int32 MinPoolSize { get; set; }
        public Int32 MaxPoolSize { get; set; }
        public String Schema { get; set; }
        public String SchemaAdmin { get; set; }
        public String SchemaRank { get; set; }
        public String SchemaAchievement { get; set; }
        public String SchemaService { get; set; }

    }

    public class SMTP
    {
        public String Host { get; set; }
        public Int32 Port { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }

    public class Redirect
    {
        public String PreLogin { get; set; }
        public String PostLogin { get; set; }
        public String ProfileIncomplete { get; set; }
    }
    public class Cookies
    {
        public Int32 ExpiryDays { get; set; }
        public String Domain { get; set; }
        public String WAFCookie { get; set; }
        public String WAFUSCCookie { get; set; }
    }

    #endregion
}
