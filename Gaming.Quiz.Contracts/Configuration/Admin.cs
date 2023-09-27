using System;
using System.Collections.Generic;

namespace  Gaming.Quiz.Contracts.Configuration
{
    public class Admin
    {
        public List<Authorization> Authorization { get; set; }
        public Feed Feed { get; set; }
        public String TemplateUri { get; set; }
        public String TemplateUriMobile { get; set; }
        public String WvTemplateUri { get; set; }
        public String UnavailableUri { get; set; }
        public String BasePath { get; set; }
        public Translation Translation { get; set; }
        public String MultiSportzFeed { get; set; }
        public Notification Notification { get; set; }
    }

    public class Authorization
    {
        public String User { get; set; }
        public String Password { get; set; }
        public List<String> Pages { get; set; }
    }

    #region " Translation "

    public class Translation
    {
        public String ProjectGroupId { get; set; }
        public PlatformId PlatformId { get; set; }
        public List<String> ClientId { get; set; }
    }

    public class PlatformId
    {
        public String Web { get; set; }
        public String iOS { get; set; }
        public String Android { get; set; }
    }

    #endregion

    public class Feed
    {
        public String API { get; set; }
        public String LineupAPIUrl { get; set; }
        public String Client { get; set; }
    }
}
