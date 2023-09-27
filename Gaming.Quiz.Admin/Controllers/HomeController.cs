using Microsoft.AspNetCore.Mvc;
using Gaming.Quiz.Admin.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using Gaming.Quiz.Admin.App_Code;
using Gaming.Quiz.Library.Utility;
using System.Collections.Generic;
using System.Text;
using Gaming.Quiz.Contracts.Feeds;
using System.IO;
using System.Data;
using Gaming.Quiz.Contracts.Admin;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Gaming.Quiz.Contracts.BGServices;
using Gaming.Quiz.Contracts.Leaderboard;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Gaming.Quiz.Contracts.GamedayMapping;
using Gaming.Quiz.Interfaces.DataPopulation;
//using Gaming.Quiz.DataInitializer.Common;

namespace Gaming.Quiz.Admin.Controllers
{
    public partial class HomeController : BaseController
    {
        #region " LOGIN "

        [HttpGet]
        [Route("/games/quiz/admin")]
        [Route("/games/quiz/admin/login")]
        public IActionResult Login(string enc)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            #region " Decryption "

            ViewBag.Enc = GenericFunctions.DecryptedValue(enc);

            #endregion

            if (_Session._HasAdminCookie)
                Response.Redirect(_AppSettings.Value.Admin.BasePath + _Session.Pages().FirstOrDefault().Replace(" ", ""));

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            if (ModelState.IsValid)
            {
                foreach (Contracts.Configuration.Authorization authority in _Admin.Authorization)
                {
                    if (model.Username.ToLower().Trim() == authority.User.ToLower().Trim() && model.Password == authority.Password)
                    {
                        bool status = _Session.SetAdminCookie(model.Username);

                        if (status)
                            Response.Redirect(_AppSettings.Value.Admin.BasePath + _Session.Pages(model.Username).FirstOrDefault().Replace(" ", ""));
                        else
                        {
                            ViewBag.MessageType = "_Error";
                            ViewBag.MessageText = "Session is Invalid.";
                        }
                    }
                    else
                    {
                        ViewBag.MessageType = "_Error";
                        ViewBag.MessageText = "Incorrect login credentials.";
                    }
                }
            }

            return View();
        }

        #endregion

        #region " LOG ME OUT "

        [HttpGet]
        public IActionResult Logout()
        {
            _Session.DeleteAdminCookie();
            Response.Redirect("/games/quiz/admin/" + "Login");
            return Content("");
        }

        #endregion

        #region "Data poulation"

        [HttpGet]
        public IActionResult DataPopulation(string enc)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            #region " Decryption "

            ViewBag.Enc = GenericFunctions.DecryptedValue(enc);

            #endregion

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DataPopulation(DataPopulationModel model, string process)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;
            DataSet ds = new DataSet();

            if (ModelState.IsValid)
            {
                switch (process)
                {
                    case "Upload":
                        tuple = _dataPopulationBlanket.InsertQuestion(model.file);
                        break;

                    case "verifyquedayget":
                        if (model.QMId != null && model.CatgId != null && model.SportId != null && !string.IsNullOrEmpty(model.Date))
                        {
                            tuple = _dataPopulationBlanket.VerifyQuestion(model.QMId.Value, model.CatgId.Value, model.SportId.Value, model.Date, out ds);

                            byte[] contents = Library.Documents.Excel.Export(ds);

                            if (tuple.Item1 == 1 && contents.Length > 0 && contents != null)
                            {
                                return File(fileContents: contents,
                                   contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                   fileDownloadName: $"Quiz_Question_Dump_{model.Date}.xlsx");
                            }
                        }
                        else
                            tuple = new Tuple<int, string>(-40, "Please input all paramters");
                        break;

                }

                Toast(tuple, process, dt);
            }

            return View();
        }

        #endregion

        #region " Admin Services "

        [HttpGet]
        public IActionResult AdminServices()
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            ServicesModel model = new ServicesModelWorker().GetModel(null, null, null);

            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminServices(ServicesModel model, String process)
        {
            Int32 retVal = -40;
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;

            List<vGamedayDetails> vGdDet = new List<vGamedayDetails>();
            List<vMonthDetails> vMonthGet = new List<vMonthDetails>();
            List<vAdminLLget> vAdminLL = new List<vAdminLLget>();


            //if (ModelState.IsValid)
            {
                switch (process)
                {

                    case "adminllget":
                        string CursorOutput = string.Empty;

                        if (model.adQMid != null)
                        {
                            tuple = _adminServicesBlanket.AdminLLget(model.adQMid.Value, ref vAdminLL);
                        }
                        else
                            tuple = new Tuple<int, string>(retVal, "Parameter Missing");
                        break;


                    case "gddetailsget":
                        if (model.GDDetMId != null)
                        {
                            tuple = _adminServicesBlanket.GamedayDetailsGet(model.GDDetMId.Value, ref vGdDet);
                        }
                        else
                            tuple = new Tuple<int, string>(retVal, "Parameter Missing");
                        break;

                    case "monthdetailsget":
                        if (model.MnthDetMId != null)
                        {
                            tuple = _adminServicesBlanket.MonthDetailsGet(model.MnthDetMId.Value, ref vMonthGet);
                        }
                        else
                            tuple = new Tuple<int, string>(retVal, "Parameter Missing");
                        break;

                    case "quizmapping":
                        if (model.QzMappMId != null && !string.IsNullOrEmpty(model.QzMapDate))
                            tuple = _adminServicesBlanket.QuizMapping(model.QzMappMId.Value, model.QzMapDate);
                        else
                            tuple = new Tuple<int, string>(retVal, "Parameter Missing");
                        break;

                    case "checkmail":
                        tuple = await TestLogLeverInfoAsync();
                        break;
                }
                Toast(tuple, process, dt);
            }

            ServicesModel listmodel = new ServicesModelWorker().GetModel(vAdminLL, vGdDet, vMonthGet);
            return View(listmodel);
        }

        #endregion " Admin Services "

        #region " Template  Ingestion "

        [HttpGet]
        public async Task<IActionResult> Template(String LangCode)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            TemplateModel model = await new TemplateWorker().GetModel(_templateBlanket, _AppSettings.Value, LangCode);
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Template(TemplateModel model, String process)
        {
            bool success = false;
            Int32 retVal = 1;
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;
            String LangCode = model.LangCode;

            if (ModelState.IsValid)
            {
                switch (process)
                {
                    case "PreHeaderTemplate":
                        success = await _templateBlanket.UpdatePreHeaderTemplate(model.PreHeaderTemplate);
                        retVal = success ? 1 : -40;
                        break;

                    case "PostFooterTemplate":
                        success = await _templateBlanket.updatePostFooterTemplate(model.PostFooterTemplate);
                        retVal = success ? 1 : -40;
                        break;

                    case "PostUnavailableViewTemplate":
                        success = await _templateBlanket.updateUnavilablePageTemplate(LangCode);
                        retVal = success ? 1 : -40;
                        break;

                    case "yahootempletdownload":
                        success = await _templateBlanket.DownloadPageTemplate();
                        retVal = success ? 1 : -40;
                        break;

                    case "UpdateTemplates":
                        success = await _templateBlanket.UpdateJsCss(LangCode);
                        retVal = success ? 1 : -40;
                        break;

                    default:
                        retVal = -30;
                        break;
                }

                if (process == "PageTemplate")
                {
                    success = await _templateBlanket.updateWebTemplate(LangCode, true);
                    retVal = success ? 1 : -40;
                }
                else if (process == "PageTemplateMobile")
                {
                    success = await _templateBlanket.updateMobileTemplate(LangCode, true);
                    retVal = success ? 1 : -40;
                }
                else if (process == "PageTemplateWebView")
                {
                    success = await _templateBlanket.updateWebViewTemplate(LangCode, false);
                    retVal = success ? 1 : -40;
                }

                if (retVal != -40)
                {
                    if (retVal == -30)
                    {
                        ViewBag.MessageType = "_Info";
                        ViewBag.MessageText = "Please provide the input values.";
                    }
                    else
                    {
                        ViewBag.MessageType = (retVal == 1) ? "_Success" : "_Error";
                        ViewBag.MessageText = (retVal == 1) ? $"{process} ingested successfully."
                            : $"Error while ingesting {process}. RetVal = {retVal}";
                    }
                }
            }

            TemplateModel dataModel = await new TemplateWorker().GetModel(_templateBlanket, _AppSettings.Value, LangCode);
            return View(dataModel);
        }

        #endregion " TEMPLATE INGESTION "

        #region "Feed Ingestion"
        [HttpGet]
        public IActionResult Feed()
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;
            //model.isLiveLeaderboard = false;
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Feed(FeedIngestionModel model, String process)
        {
            Int32 retVal = -40;
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;

            if (ModelState.IsValid)
            {
                model.QuizId = _AppSettings.Value.Properties.QuizMasterId;
                switch (process)
                {
                    case "languages":
                        tuple = await _ingestionBlanket.Languages();
                        break;

                    case "monthingest":
                        if (model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenMonthFile(model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "weekingest":
                        if (model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenWeekFile(model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "gamedayingest":
                        if (model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenGamedayFile(model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "favplayeringest":
                        if (model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenFavPlayerFile(model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "badgeingest":
                        if (model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenBadgesFeed(model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;

                    case "bestscoreleaderboard":
                        if (model.BSMonthId != null)
                            tuple = await _leaderboardBlanket.GenBestScoreLeaderboard(model.BSGamedayId.Value, model.BSWeekId.Value, model.BSMonthId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;

                    case "monthleaderboard":
                        if (model.OVMonthId != null && model.QuizId != null)
                            tuple = await _leaderboardBlanket.GenMonthLeaderboard(model.OVGamedayId.Value, model.OVWeekId.Value, model.OVMonthId.Value, model.QuizId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "playerleaderboard":
                        if (model.PlayerId != null)
                            tuple = await _leaderboardBlanket.GenTeamLeaderboard(model.PlayerId.Value, model.PlrGamedayId.Value, model.PlrWeekId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;
                    case "overallleaderboard":
                        tuple = await _leaderboardBlanket.GenOverallLeaderboard(model.QuizId.Value);
                        break;
                }
                Toast(tuple, process, dt);
            }

            return View();
        }

        #endregion

        #region " Translations "

        [HttpGet]
        public async Task<IActionResult> Translations()
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            FeedIngestionModel model = new FeedIngestionModel();

            String mData = GenericFunctions.Serialize(await _feedsBlanket.GetTranslations("en"));
            model.Translations = JValue.Parse(mData).ToString(Formatting.Indented);

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Translations(FeedIngestionModel model, String process)
        {
            Int32 retVal = -40;
            Tuple<Int32, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;

            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            if (ModelState.IsValid)
            {
                switch (process)
                {
                    case "trans":
                        tuple = await _ingestionBlanket.Translations(model.Translations);
                        break;
                }

                Toast(tuple, process, dt);

            }

            return View();
        }

        #endregion " Translations "

        #region " Point Calculation "

        [HttpGet]
        public IActionResult PointCalculation()
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PointCalculation(PointCalculationModel model, String process)
        {
            Int32 retVal = -40;
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;

            if (ModelState.IsValid)
            {
                switch (process)
                {

                    case "userpointprocess":
                        if (model.MonthId != null)
                            tuple = await _pointCalculationBlanket.UserPointProcess(model.MonthId.Value, model.GamedayId.Value, model.WeekId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;

                    case "userbestscoreupd":
                        if (model.UserId != null && model.BsMonthId != null)
                            tuple = await _pointCalculationBlanket.UserBestScoreUpdate(model.UserId.Value, model.BsMonthId.Value, model.BsGamedayId.Value, model.BsWeekId.Value);
                        else
                            tuple = Extension.InsufficientInput();
                        break;

                    case "usersettlemeneodprocess":
                        tuple = await _pointCalculationBlanket.EODSettlement();
                        break;
                }
                Toast(tuple, process, dt);
            }

            return View();
        }

        #endregion

        #region " Helper "

        private void Toast(Tuple<int, string> tuple, String process, DateTime startTime)
        {
            if (tuple != null && tuple.Item1 != -30)
            {
                if (tuple.Item1 == -20)
                {
                    ViewBag.MessageType = "_Info";
                    ViewBag.MessageText = "Please provide input values.";
                }
                else
                {
                    String timeTaken = GenericFunctions.TimeDifference(startTime, DateTime.Now);

                    ViewBag.MessageType = (tuple.Item1 == 1) ? "_Success" : "_Error";
                    ViewBag.MessageText = (tuple.Item1 == 1) ? (process + $" process successful. [ Time taken: {timeTaken} ] " + ((!String.IsNullOrEmpty(tuple.Item2) && tuple.Item2.Trim() != "") ? tuple.Item2 : ""))
                        : $"{process} process error. [RetVal = {tuple.Item1}] [ Time taken: {timeTaken} ] {tuple.Item2}";
                }
            }
        }

        public async Task<Tuple<int, string>> TestLogLeverInfoAsync()
        {
            bool status = false;
            string error = string.Empty;
            try
            {
                status = await Notify("SES Check", "Test Mail");
                error = status.ToString();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new Tuple<int, string>(status == true ? 1 : 0, error);
        }

        #endregion

        #region " SEO META Management "

        [HttpGet]
        public async Task<IActionResult> SEOMeta(String LangCode)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            SEOModel model = await new TemplateWorker().GetSEOModel(_templateBlanket, _AppSettings.Value, LangCode);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SEOMeta(SEOModel model, String process)
        {
            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;
            Int32 retVal = -40;
            bool success = false;
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            String LangCode = model.LangCode;
            if (ModelState.IsValid)
            {
                switch (process)
                {
                    case "home":
                        success = await _templateBlanket.UpdateHomeMeta(model.metaHtmlHome);
                        retVal = success ? 1 : -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;

                    case "terms":
                        success = await _templateBlanket.UpdateTermsMeta(model.metaHtmlTerms);
                        retVal = success ? 1 : -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;

                    case "leaderbaord":
                        success = await _templateBlanket.UpdateLeaderboardMeta(model.metaHtmlLeaderboard);
                        retVal = success ? 1 : -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;

                    case "disclaimer":
                        success = await _templateBlanket.UpdateDisclaimerMeta(model.metaHtmlDisclaimer);
                        retVal = success ? 1 : -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;

                    case "rules":
                        success = await _templateBlanket.UpdateRulesMeta(model.metaHtmlRules);
                        retVal = success ? 1 : -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;

                    default:
                        retVal = -40;
                        tuple = new Tuple<int, string>(retVal, success.ToString());
                        break;
                }

                Toast(tuple, process, dt);
            }

            SEOModel mmodel = await new TemplateWorker().GetSEOModel(_templateBlanket, _AppSettings.Value, LangCode);
            return View(mmodel);
        }

        #endregion "SEO META Management "

        #region " Analytics "

        [HttpGet]
        public IActionResult Analytics(string enc)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            AnalyticsModel model = new AnalyticsModel();
            model.Analytics = null;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Analytics(AnalyticsModel model, string process)
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;
            AnalyticsModel analyticsModel = new AnalyticsModel();

            Tuple<int, string> tuple = Extension.DefaultInput();
            DateTime dt = DateTime.Now;
            DataSet ds = new DataSet();

            //Contracts.Analytics.Analytics analytics = new Contracts.Analytics.Analytics();
            Contracts.Analytics.AnalyticsNew analytics = new Contracts.Analytics.AnalyticsNew();
            Contracts.Analytics.QPLAnalytics qplanalytics = new Contracts.Analytics.QPLAnalytics();

            if (ModelState.IsValid)
            {
                switch (process)
                {
                    case "getanalytics":
                        if (model.fromdate != null && model.todate != null)
                        {
                            //tuple = _AnalyticsContext.GetAnalytics(model.fromdate, model.todate, ref analytics);
                            tuple = _analytics.GetAnalyticsNew(model.fromdate, model.todate, ref analytics);
                            tuple = _analytics.GetQPLAnalytics(model.fromdate, model.todate, ref qplanalytics);

                        }
                        else
                            tuple = Extension.InsufficientInput();
                        break;

                    
                }

                Toast(tuple, process, dt);
            }

            analyticsModel.Analytics = analytics;
            analyticsModel.QPLAnalytics = qplanalytics;

            return View(analyticsModel);
        }

        #endregion

        #region " Gameday Mapping "

        [HttpGet]
        public IActionResult GamedayMapping()
        {
            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            GamedayMappingModel model = new GamedayMappingModel();
            model = new GamedayMappingWorker().GetGamedayMappingModel(_gamedayMappingBlanket).Result;

            return View(model);
        }

        [HttpPost]
        public IActionResult GamedayMapping(GamedayMappingModel model, string process)
        {
            int retVal = -40;
            GamedayMappingModel newModel = new GamedayMappingModel();

            ViewBag.BasePath = _AppSettings.Value.Admin.BasePath;

            try
            {
                switch (process)
                {
                    case "mapping":
                        {
                            List<int> gamedayId = model.GamedayMapping.Where(x => x.Checked).Select(y => y.GamedayId).ToList();
                            List<string> tag = model.GamedayMapping.Where(x => x.Checked).Select(y => y.TagName).ToList();

                            if (gamedayId != null && tag != null && gamedayId.Count > 0 && tag.Count > 0 && tag.All(a => !String.IsNullOrEmpty(a)))
                            {
                                retVal = _gamedayMappingBlanket.UpdateGamedayMapping(gamedayId, tag);
                            }
                            else
                            {
                                retVal = -30;
                            }
                        }
                        break;
                }

                if(retVal != -40)
                {
                    if (retVal == -30)
                    {
                        ViewBag.MessageType = "_Info";
                        ViewBag.MessageText = "Please provide the input values.";
                    }
                    else
                    {
                        ViewBag.MessageType = (retVal == 1) ? "_Success" : "_Error";
                        ViewBag.MessageText = (retVal == 1) ? $"{process} done successfully."
                            : $"Error while doing {process}. RetVal = {retVal}";
                    }
                }
                newModel = new GamedayMappingWorker().GetGamedayMappingModel(_gamedayMappingBlanket).Result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return View(newModel);
        }

        #endregion
    }
}
