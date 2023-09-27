using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Gaming.Quiz.Interfaces.Template;

namespace Gaming.Quiz.Blanket.Template
{
    public class Template : Common.BaseBlanket, ITemplateBlanket
    {
        private readonly String _TemplateUri;
        private readonly String _TemplateUriMobile;
        private readonly String _WvTemplateUri;
        private readonly String _UnavailableUri;

        public Template(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset, IHttpContextAccessor httpContextAccessor)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContextAccessor)
        {
            _TemplateUri = appSettings.Value.Admin.TemplateUri;
            _WvTemplateUri = appSettings.Value.Admin.WvTemplateUri;
            _TemplateUriMobile = appSettings.Value.Admin.TemplateUriMobile;
            _UnavailableUri = appSettings.Value.Admin.UnavailableUri;
        }

        #region " Get Template "

        public async Task<String> GetPreHeaderTemplate()
        {
            return await _Asset.GET(_Asset.PreHeaderTemplate());
        }

        public async Task<String> GetPostFooterTemplate()
        {
            return await _Asset.GET(_Asset.PostFooterTemplate());
        }

        public async Task<String> GetPageTemplate(String lang, Int32 IsWebView)
        {
            return await _Asset.GET(_Asset.PageTemplate(lang, IsWebView));
        }

        public async Task<String> GetPageTemplateMobile(String lang, Int32 IsWebView)
        {
            return await _Asset.GET(_Asset.PageTemplateMobile(lang, IsWebView));
        }

        public async Task<String> GetUnavailablePageTemplate(String lang)
        {
            return await _Asset.GET(_Asset.PageUnavailableTemplate(lang));
        }


        #endregion " Get Template "

        #region " Ingest Template "

        public async Task<bool> UpdatePreHeaderTemplate(string data)
        {
            return await _Asset.SET(_Asset.PreHeaderTemplate(), data, false);
        }

        public async Task<bool> updatePostFooterTemplate(string data)
        {
            return await _Asset.SET(_Asset.PostFooterTemplate(), data, false);
        }

        public async Task<bool> updatePageTemplate(string lang, bool IsWebView)
        {
                String template = String.Empty;
                template = await _Asset.GET(_Asset.PageTemplate());
                template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
                template = template.Replace("</footer></div></body></html>", "</footer>" + await GetPostFooterTemplate()+ "</div></body></html>");
                template = template.Replace("</myapp>", "<div id=\"root\"></div></myapp>");
                template = template.Replace("\"swiper\":", "// \"swiper\":");
                template = template.Replace("\"nicescroll\":", "// \"nicescroll\":");
                //template = template.Replace("<div class=\"site\">", "<div class=\"site siteoverride>");


            return await _Asset.SET(_Asset.PageTemplate(lang, Convert.ToInt32(IsWebView)), template, false);
        }

        public async Task<bool> updatePageTemplateMobile(string lang, bool IsWebView)
        {
            String template = String.Empty;

            //if (!IsWebView)
            //{
                //web
                template = await _Asset.GET(_Asset.MobileTemplate());
                template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
                template = template.Replace("</footer></div></body></html>", "</footer>" + await GetPostFooterTemplate() + "</div></body></html>");
                template = template.Replace("</myapp>", "<div id=\"root\"></div></myapp>");
                template = template.Replace("\"swiper\":", "// \"swiper\":");
                template = template.Replace("\"nicescroll\":", "// \"nicescroll\":");

                return await _Asset.SET(_Asset.PageTemplateMobile(lang, Convert.ToInt32(IsWebView)), template, false);
            //}
            //else
            //{
            //    template = scrapeTemplate(_WvTemplateUri);
            //    template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
            //    template = template.Replace("</footer>", "</footer>" + await GetPostFooterTemplate());
            //    template = template.Replace("</myapp>", "<div id=\"root\"></div></myapp>");

            //    return await _Asset.SET(_Asset.PageTemplateMobile(lang, 1), template, false);
            //}
        }


        public async Task<bool> updateUnavilablePageTemplate(string lang)
        {
            String template = String.Empty;
            try
            {
                template = scrapeTemplate(_UnavailableUri);
            }
            catch (Exception ex)
            {
                template = ex.Message;
            }

            return await _Asset.SET(_Asset.PageUnavailableTemplate(lang), template, false);
        }



        //My new Template Functions

        public async Task<bool> DownloadPageTemplate()
        {
            String template = String.Empty;
            bool status = false;
            try
            {
                template = scrapeTemplate(_WvTemplateUri);

                status = await _Asset.SET(_Asset.YahooWvTemplate(), template, false);

                template = scrapeTemplate(_TemplateUri);

                status = await _Asset.SET(_Asset.YahooTemplate(), template, false);

                template = scrapeTemplate(_TemplateUriMobile);

                status = await _Asset.SET(_Asset.YahooMobileTemplate(), template, false);

            }
            catch (Exception ex)
            {
                template = ex.Message;
            }

            return status; 
        }

        public async Task<bool> UpdateJsCss(string lang)
        {
            String template = String.Empty;

            bool status = false;

            //status = await updateWebTemplate(lang,true);
            //status = await updateMobileTemplate(lang,false);
            status = await updateWebViewTemplate(lang,false);

            return status;
        }

        public async Task<bool> updateWebTemplate(string lang, bool IsWebView)
        {
            String template = String.Empty;
            bool status = false;

            template = await _Asset.GET(_Asset.YahooTemplate());
            template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
            template = template.Replace("</footer></div></body></html>", "</footer>" + await GetPostFooterTemplate() + "</div></body></html>");
            template = template.Replace("</myapp>", "<div id=\"quizcontainer\"></div></myapp>");
            template = template.Replace("\"swiper\":", "// \"swiper\":");
            template = template.Replace("\"nicescroll\":", "// \"nicescroll\":");
            template = template.Replace("<title>404</title>", "<title>Take the Ultimate Cricket Quiz | Yahoo! Cricket</title>");
            template = template.Replace("content=\"404\"", "content=\"Take the Ultimate Cricket Quiz | Yahoo! Cricket\"");
            template = template.Replace("content=\"Yahoo Daily Quiz\"", "content=\"Yahoo Cricket Quiz\"");
            template = template.Replace("content=\"Yahoo! Cricket\"", "content=\"Yahoo Cricket Quiz\"");
            template = template.Replace("Yahoo Daily Quiz", "Yahoo Cricket Quiz");
            //template = template.Replace("https://beta.cricket.yahoo.sportz.io/static-assets/images/cssimages/logo.png", "https://beta.cricket.yahoo.sportz.io/quiz/static-assets/newlogohigh.jpg");
            //template = template.Replace("https://beta.cricket.yahoo.sportz.io/404", "https://beta.cricket.yahoo.sportz.io/quiz/home");
            template = template.Replace("https://cricket.yahoo.net/static-assets/images/cssimages/logo.png", "https://cricket.yahoo.net/quiz/static-assets/newlogohigh.jpg");
            template = template.Replace("https://cricket.yahoo.net/404", "https://cricket.yahoo.net/quiz/home");

            status = await _Asset.SET(_Asset.PageTemplate(lang, Convert.ToInt32(IsWebView)), template, false);

            return status;
        }

        public async Task<bool> updateWebViewTemplate(string lang, bool IsWebView)
        {
            String template = String.Empty;
            bool status = false;


            template = await _Asset.GET(_Asset.YahooWvTemplate());
            template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
            template = template.Replace("</footer>", "</footer>" + await GetPostFooterTemplate());
            template = template.Replace("</myapp>", "<div id=\"quizcontainer\"></div></myapp>");

            status = await _Asset.SET(_Asset.PageTemplate(lang, Convert.ToInt32(IsWebView)), template, false);

            return status;
        }

        public async Task<bool> updateMobileTemplate(string lang, bool IsWebView)
        {
            String template = String.Empty;
            bool status = false;

            template = await _Asset.GET(_Asset.YahooMobileTemplate());
            template = template.Replace("</head>", await GetPreHeaderTemplate() + "</head>");
            template = template.Replace("</footer></div></body></html>", "</footer>" + await GetPostFooterTemplate() + "</div></body></html>");
            template = template.Replace("</myapp>", "<div id=\"quizcontainer\"></div></myapp>");
            template = template.Replace("\"swiper\":", "// \"swiper\":");
            template = template.Replace("\"nicescroll\":", "// \"nicescroll\":");
            template = template.Replace("<title>404</title>", "<title>Take the Ultimate Cricket Quiz | Yahoo! Cricket</title>");
            template = template.Replace("content=\"404\"", "content=\"Take the Ultimate Cricket Quiz | Yahoo! Cricket\"");
            template = template.Replace("content=\"Yahoo Daily Quiz\"", "content=\"Yahoo Cricket Quiz\"");
            template = template.Replace("content=\"Yahoo! Cricket\"", "content=\"Yahoo Cricket Quiz\"");
            template = template.Replace("daily fantasy", "Yahoo Cricket Quiz");
            //template = template.Replace("https://beta.cricket.yahoo.sportz.io/static-assets/images/cssimages/logo.png", "https://beta.cricket.yahoo.sportz.io/quiz/static-assets/newlogohigh.jpg");
            //template = template.Replace("https://beta.cricket.yahoo.sportz.io/404", "https://beta.cricket.yahoo.sportz.io/quiz/home");
            template = template.Replace("https://cricket.yahoo.net/static-assets/images/cssimages/logo.png", "https://cricket.yahoo.net/quiz/static-assets/newlogohigh.jpg");
            template = template.Replace("https://cricket.yahoo.net/404", "https://cricket.yahoo.net/quiz/home");

            status = await _Asset.SET(_Asset.PageTemplateMobile(lang, Convert.ToInt32(IsWebView)), template, false);

            return status;
        }

        #endregion " Ingest Template "

        #region " Scrape Template  "

        private String scrapeTemplate(String templateURI)
        {
            String data = String.Empty;

            data = Library.Utility.GenericFunctions.GetYahooTemplateData(templateURI);

            return data;
        }

        #endregion " Scrape Template  "

        #region " Ingest SEO Meta "

        #region " Update "

        public async Task<bool> UpdateHomeMeta(string data)
        {
            return await _Asset.SET(_Asset.SEOHome(), data, false);
        }


        public async Task<bool> UpdateLeaderboardMeta(string data)
        {
            return await _Asset.SET(_Asset.SEOLeaderboard(), data, false);
        }


        public async Task<bool> UpdateRulesMeta(string data)
        {
            return await _Asset.SET(_Asset.SEORules(), data, false);
        }

        public async Task<bool> UpdateTermsMeta(string data)
        {
            return await _Asset.SET(_Asset.SEOTerms(), data, false);
        }

        public async Task<bool> UpdateDisclaimerMeta(string data)
        {
            return await _Asset.SET(_Asset.SEODisclaimer(), data, false);
        }

        #endregion

        #region " Get "

        public async Task<String> GetHomeMeta()
        {
            return await _Asset.GET(_Asset.SEOHome());
        }

        public async Task<String> GetLeaderboardMeta()
        {
            return await _Asset.GET(_Asset.SEOLeaderboard());
        }

        public async Task<String> GetRulesMeta()
        {
            return await _Asset.GET(_Asset.SEORules());
        }

        public async Task<String> GetTermstMeta()
        {
            return await _Asset.GET(_Asset.SEOTerms());
        }

        public async Task<String> GetDisclaimerMeta()
        {
            return await _Asset.GET(_Asset.SEODisclaimer());
        }

        #endregion

        #endregion
    }
}
