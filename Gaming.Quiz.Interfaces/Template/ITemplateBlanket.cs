using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Template
{
    public interface ITemplateBlanket
    {
        Task<bool> DownloadPageTemplate();
        Task<string> GetDisclaimerMeta();
        Task<string> GetHomeMeta();
        Task<string> GetLeaderboardMeta();
        Task<string> GetPageTemplate(string v1, int v2);
        Task<string> GetPageTemplateMobile(string v1, int v2);
        Task<string> GetPostFooterTemplate();
        Task<string> GetPreHeaderTemplate();
        Task<string> GetRulesMeta();
        Task<string> GetTermstMeta();
        Task<string> GetUnavailablePageTemplate(string v);
        Task<bool> UpdateDisclaimerMeta(string metaHtmlDisclaimer);
        Task<bool> UpdateHomeMeta(string metaHtmlHome);
        Task<bool> UpdateJsCss(string langCode);
        Task<bool> UpdateLeaderboardMeta(string metaHtmlLeaderboard);
        Task<bool> updateMobileTemplate(string langCode, bool v);
        Task<bool> updatePostFooterTemplate(string postFooterTemplate);
        Task<bool> UpdatePreHeaderTemplate(string preHeaderTemplate);
        Task<bool> UpdateRulesMeta(string metaHtmlRules);
        Task<bool> UpdateTermsMeta(string metaHtmlTerms);
        Task<bool> updateUnavilablePageTemplate(string langCode);
        Task<bool> updateWebTemplate(string langCode, bool v);
        Task<bool> updateWebViewTemplate(string langCode, bool v);
    }
}
