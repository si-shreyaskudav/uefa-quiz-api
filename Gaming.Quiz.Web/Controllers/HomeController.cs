using System;
using Microsoft.AspNetCore.Mvc;
using Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Session;
using Gaming.Quiz.Contracts.Common;
using System.Text;
using Gaming.Quiz.Library.Social;
using Gaming.Quiz.Interfaces.Template;

namespace Gaming.Quiz.Web.Controllers
{
    public class HomeController : BaseController
    {
        public readonly String _StaticAssetsBasePath;
        //public readonly Blanket.Session.Session _Session;
        //private readonly Blanket.Template.Template _templateBlanket;
        protected readonly Settings _Settings;

        private string _CallBackUrl;
        private string _RedirectUrl;
        private string _PreLogin;
        private string _PostLogin;
        protected readonly Facebook _Facebook;
        protected readonly Google _Google;

        //private readonly Blanket.Session.Session _SessionContext;
        private readonly ITemplateBlanket _templateBlanket;
        private readonly ISessionBlanket _sessionBlanket;

        public HomeController(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
            IHttpContextAccessor httpContextAccessor,
            ITemplateBlanket templateBlanket) : base(appSettings, aws, postgre, redis, cookies, asset, httpContextAccessor)
        {

            _StaticAssetsBasePath = appSettings.Value.Properties.StaticAssetBasePath;
            _Settings = appSettings.Value.Settings;

            _CallBackUrl = appSettings.Value.Settings.CallBackUrl;
            _Facebook = new Facebook(appSettings);
            _Google = new Google(appSettings);
            _PreLogin = appSettings.Value.Redirect.PreLogin;
            _PostLogin = appSettings.Value.Redirect.PostLogin;

            this._templateBlanket = templateBlanket;
        }

        public async Task<IActionResult> Index(String webview, String ismobile)
        {
            String data = String.Empty;
            String meta = String.Empty;


            //if (ismobile == "true")
            //    data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            //else
            //    data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);

            if (!String.IsNullOrEmpty(webview))
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 0);
            else if (ismobile == "true")
                data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            else
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);


            meta = await _templateBlanket.GetHomeMeta();
            data = data.Replace("</head>", meta + "</head>");

            if (String.IsNullOrEmpty(data))
            {
                data = @"<html><body>This page works</body></html>";
            }

            ViewBag.StartMarkup = data;

            ViewData.Add("StaticAssetsBasePath", _StaticAssetsBasePath);
            return View("/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> Disclaimer(String webview, String ismobile)
        {
            String data = String.Empty;
            String meta = String.Empty;

            if (ismobile == "true")
                data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            else
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);

            meta = await _templateBlanket.GetDisclaimerMeta();
            data = data.Replace("</head>", meta + "</head>");
            ViewBag.StartMarkup = data;

            ViewData.Add("StaticAssetsBasePath", _StaticAssetsBasePath);
            return View("/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> Terms(String webview, String ismobile)
        {
            String data = String.Empty;
            String meta = String.Empty;


            if (ismobile == "true")
                data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            else
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);

            meta = await _templateBlanket.GetTermstMeta();
            data = data.Replace("</head>", meta + "</head>");
            ViewBag.StartMarkup = data;

            ViewData.Add("StaticAssetsBasePath", _StaticAssetsBasePath);
            return View("/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> Rules(String webview, String ismobile)
        {
            String data = String.Empty;
            String meta = String.Empty;

            if (ismobile == "true")
                data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            else
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);

            meta = await _templateBlanket.GetRulesMeta();
            data = data.Replace("</head>", meta + "</head>");
            ViewBag.StartMarkup = data;

            ViewData.Add("StaticAssetsBasePath", _StaticAssetsBasePath);
            return View("/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> Leaderboard(String webview, String ismobile)
        {
            String data = String.Empty;
            String meta = String.Empty;

            if (ismobile == "true")
                data = await _templateBlanket.GetPageTemplateMobile(_Lang[0], 1);
            else
                data = await _templateBlanket.GetPageTemplate(_Lang[0], 1);

            meta = await _templateBlanket.GetLeaderboardMeta();
            data = data.Replace("</head>", meta + "</head>");

            ViewBag.StartMarkup = data;

            ViewData.Add("StaticAssetsBasePath", _StaticAssetsBasePath);
            return View("/Views/Home/Index.cshtml");
        }

        public async Task<IActionResult> Unavailable()
        {
            String data = String.Empty;
            data = await _templateBlanket.GetUnavailablePageTemplate(_Lang[0]);
            ViewBag.HTML = data;

            return View();
        }
    }
}