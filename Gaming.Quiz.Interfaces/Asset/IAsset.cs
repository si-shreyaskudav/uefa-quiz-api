using System;
using System.Collections.Generic;

namespace  Gaming.Quiz.Interfaces.Asset
{
    public interface IAsset : IAction
    {
        String GlobalLeaderboard(Int64 optTypee);

        #region " Template "


        String PreHeaderTemplate();
        String PageTemplate();
        String MobileTemplate();


        String PostFooterTemplate();

        String PageTemplate(String lang, Int32 IsWebView);
        String PageTemplateMobile(String lang, Int32 IsWebView);
        String GetLoginForm();
        String ScrapTemplate();

        String PageUnavailableTemplate(String lang);

        String YahooTemplate();
        String YahooMobileTemplate();
        String YahooWvTemplate();

        #endregion

        String Translation(String lang);
        String Tutorial(String lang);


        String Languages();

        #region "Leaderboard"
        String MonthLeaderboard( Int64 MonthId, Int64 QuizId);
        String GamedayLeaderboard(Int64 GamedayId, Int64 QuizId);
        String WeeklyLeaderboard(Int64 WeekId, Int64 QuizId);
        String OverallLeaderboard(Int64 optType,Int64 QuizId);

        String PlayerLeaderboard(Int64 PlayerId, Int64 QuizId);

        String BestScoreLeaderboard(Int32 optType, Int64 MonthId);

        String MonthFile(Int64 quizId);
        String WeekFile(Int64 quizId);
        String GamedayFile(Int64 quizId);
        String FavPlayerFile(Int64 quizId);
        String BadgesFile(Int64 quizId);
        String MixApi(Int64 quizId);

        #endregion

        #region " Social Sharing"

        String ShareTemplate();

        String ShareGraphics(String fileName);

        String MetaPath();

        String LanguagePath();

        String UserMetaPath(String guid, String  lang);

        #endregion

        #region " SEO Template "

        String SEOHome();
        String SEOLeaderboard();
        String SEOTerms();
        String SEODisclaimer();
        String SEORules();

        #endregion

        String Profanity();

        String AnalyticsReport(String reportDate);
    }
}
