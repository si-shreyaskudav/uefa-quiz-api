using System;
using Gaming.Quiz.Contracts.Configuration;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Session;
using Microsoft.AspNetCore.Http;

namespace Gaming.Quiz.Library.Asset
{
    public class Constants : Action.Exist, Interfaces.Asset.IAsset
    {
        private readonly String _RedisBaseKey;

        public Constants(IAWS aws, IRedis redis, IOptions<Application> appSettings, ICookies cookies, IHttpContextAccessor httpContextAccessor)
            : base(aws, redis, appSettings, cookies, httpContextAccessor)
        {
            _RedisBaseKey = $"dream11:{_TourId}:";
        }


        #region " Leaderboards "

        public String GlobalLeaderboard(Int64 optType)
        {
            String key = "";
            if (optType == 2)
                key = $"/feeds/leaderboards/globalleaderboard_{_TourId}.json";
            else if (optType == 3)
                key = $"/feeds/leaderboards/globalleaderboard_phase_{_TourId}.json";
            else
                key = $"/feeds/leaderboards/globalleaderboard_{_TourId}.json";

            if (_UseRedis)
            {
                if (optType == 2)
                    key = $"{_RedisBaseKey}globalleaderboard:pgno";
                else if (optType == 3)
                    key = $"{_RedisBaseKey}globalleaderboard:phase::pgno:";
                else
                    key = $"{_RedisBaseKey}globalleaderboard:pgno:";
            }


            return key;
        }

        #endregion

        #region " Template "

        public String PreHeaderTemplate()
        {
            String key = $"/feeds/template/preheader_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:preheader";

            return key;
        }

        public String PageTemplate()
        {
            String key = $"/feeds/template/yahootemplate.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:preheader";

            return key;
        }
        public String MobileTemplate()
        {
            String key = $"/feeds/template/yahoomobiletemplate.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:preheader";

            return key;
        }

        public String PostFooterTemplate()
        {
            String key = $"/feeds/template/postfooter{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:postfooter";

            return key;
        }

        public String PageTemplate(String lang, Int32 IsWebView)
        {
            String key = $"/static-assets/templates/gameplay/pagetemplate_{_TourId}_{IsWebView}_{lang}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}pagetemplate:{IsWebView}:{lang}";

            return key;
        }

        public String PageTemplateMobile(String lang, Int32 IsWebView)
        {
            String key = $"/feeds/template/pagetemplate_mobile_{_TourId}_{IsWebView}_{lang}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-pagetemplate_mobile_-{IsWebView}-{lang}";

            return key;
        }

        public String GetLoginForm()
        {
            String key = $"/feeds/template/loginpagetemplate_1_1_en.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:loginpagetemplate:1:1:en.json";

            return key;
        }

        public String ScrapTemplate()
        {
            String key = $"/feeds/template/template-scrap_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:template:scrap";

            return key;
        }

        public String PageUnavailableTemplate(String lang)
        {
            String key = $"/feeds/template/unavailable-pagetemplate_{_TourId}_{lang}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:unavailable:pagetemplate:{lang}";

            return key;
        }

        public String YahooTemplate()
        {
            String key = $"/feeds/template/yahootemplate_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:unavailable:yahootemplate";

            return key;
        }

        public String YahooMobileTemplate()
        {
            String key = $"/feeds/template/yahoomobiletemplate_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:unavailable:yahoomobiletemplate";

            return key;
        }

        public String YahooWvTemplate()
        {
            String key = $"/feeds/template/yahoowvtemplate_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}:unavailable:yahoowvtemplate";

            return key;
        }

        #endregion

        public String Translation(String lang)
        {
            String key = $"/feeds/translations/trans_{_TourId}_{lang}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-translations-{lang}";

            return key;
        }

        public String Tutorial(String lang)
        {
            String key = $"/feeds/tutorial/sample_{_TourId}_{lang}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-tutorial-{lang}";

            return key;
        }


        public String Languages()
        {
            String key = $"/feeds/languages/languages_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-languages";

            return key;
        }

        public String Profanity()
        {
            String key = $"/feeds/validation/profanity_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-profanity";

            return key;
        }


        #region "Leaderbaord"

        public String MonthLeaderboard(Int64 MonthId, Int64 QuizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{QuizId}/month/overall_{MonthId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-month-overall-{_TourId}-{MonthId}";

            return key;
        }

        public String GamedayLeaderboard(Int64 GamedayId, Int64 QuizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{QuizId}/gameday/gameday_{GamedayId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-gameday-gameday-{_TourId}-{GamedayId}";

            return key;
        }

        public String WeeklyLeaderboard(Int64 WeekId, Int64 QuizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{QuizId}/week/week_{WeekId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-gameday-gameday-{_TourId}-{WeekId}";

            return key;
        }
        public String PlayerLeaderboard(Int64 PlayerId, Int64 QuizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{QuizId}/team/player_{_TourId}_{PlayerId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-team-player-{_TourId}-{PlayerId}";

            return key;
        }
        public String OverallLeaderboard(Int64 optType, Int64 QuizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{QuizId}/overall/overall.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-overall-{_TourId}-{optType}";

            return key;
        }



        public String BestScoreLeaderboard(Int32 optType, Int64 MonthId)
        {
            String key = "";

            key = $"/feeds/leaderboards/bestscore_{_TourId}_{optType}_{MonthId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-bestscore-{_TourId}-{optType}-{MonthId}";

            return key;
        }

        public String MonthFile(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{quizId}/filter/month.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-month";

            return key;
        }

        public String WeekFile(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{quizId}/filter/week.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-week";

            return key;
        }

        public String GamedayFile(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{quizId}/filter/gameday.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-gameday";

            return key;
        }

        public String FavPlayerFile(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/leaderboards/{quizId}/filter/favplayers.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-leaderboards-favplayers";

            return key;
        }

        public String BadgesFile(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/badges/{quizId}/badges.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-badges-badges";

            return key;
        }

        public String MixApi(Int64 quizId)
        {
            String key = "";

            key = $"/feeds/live/{quizId}/mixapi.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-feeds-live-mixapi.json";

            return key;
        }
        #endregion


        #region " Share Template "

        public String ShareTemplate()
        {
            String key = $"/feeds/image-share/sharetemplate/share_image_template.txt";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-template-sharetemplate";

            return key;
        }

        public String ShareGraphics(String fileName)
        {
            String key = $"/feeds/share/graphic/images/share/{fileName}";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-share-graphic-{fileName}";

            return key;
        }

        public String MetaPath()
        {
            String key = $"/feeds/share/template/meta/meta_tag_template.txt";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-share-template-meta-meta_tag_template";

            return key;
        }

        public String LanguagePath()
        {
            String key = $"/feeds/share/template/languages/language_details.txt";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-share-template-languages-language_details";

            return key;
        }

        public String UserMetaPath(String lang, String guid)
        {
            String key = $"/feeds/share/graphic/meta/tags_{guid}.html";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-share-graphic-meta-tags_{guid}_{lang}";

            return key;
        }

        #endregion


        #region " SEO Templates "

        public String SEOHome()
        {
            String key = $"/feeds/template/meta/home_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:meta:home";

            return key;
        }

        public String SEOLeaderboard()
        {
            String key = $"/feeds/template/meta/leaderboard_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:meta:leaderboard";

            return key;
        }

        public String SEORules()
        {
            String key = $"/feeds/template/meta/rules_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:meta:rules";

            return key;
        }

        public String SEOTerms()
        {
            String key = $"/feeds/template/meta/terms_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:meta:terms";

            return key;
        }

        public String SEODisclaimer()
        {
            String key = $"/feeds/template/meta/disclaimer_{_TourId}.json";

            if (_UseRedis)
                key = $"{_RedisBaseKey}template:meta:disclaimer";

            return key;
        }

        #endregion

        #region "Analytics Report"

        public String AnalyticsReport(String reportDate)
        {
            String key = $"/feeds/analytics/{reportDate}.html";

            if (_UseRedis)
                key = $"{_RedisBaseKey}-{reportDate}";

            return key;
        }


        #endregion


    }
}
