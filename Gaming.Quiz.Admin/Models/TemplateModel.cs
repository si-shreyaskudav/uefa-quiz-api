using Gaming.Quiz.Interfaces.Template;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class TemplateModel
    {

        [Display(Name = "PreHeaderTemplate")]
        public string PreHeaderTemplate { get; set; }

        [Display(Name = "PostFooterTemplate")]
        public string PostFooterTemplate { get; set; }

        [Display(Name = "PostUnavailableTemplate")]
        public string PostUnavailableTemplate { get; set; }


        public string LangCode { get; set; }

        public List<LangList> LangList { get; set; }
    }

    public class LangList
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    #region " Worker "

    public class TemplateWorker
    {
        public async Task<TemplateModel> GetModel(ITemplateBlanket templateContext,
                                         Contracts.Configuration.Application applicationContext, System.String LangCode)
        {
            TemplateModel model = new TemplateModel();


            model.PreHeaderTemplate = await templateContext.GetPreHeaderTemplate();
            model.PostFooterTemplate = await templateContext.GetPostFooterTemplate();

            model.LangList = applicationContext.Properties.Languages.Select(o => new Models.LangList
            {
                Id = o.ToString(),
                Name = o.ToString()
            }).ToList();

            return model;
        }

        public async Task<SEOModel> GetSEOModel(ITemplateBlanket templateContext,
                                     Contracts.Configuration.Application applicationContext, System.String LangCode)
        {
            SEOModel model = new SEOModel();
            model.metaHtmlHome = await templateContext.GetHomeMeta();
            model.metaHtmlLeaderboard = await templateContext.GetLeaderboardMeta();
            model.metaHtmlTerms = await templateContext.GetTermstMeta();
            model.metaHtmlDisclaimer = await templateContext.GetDisclaimerMeta();
            model.metaHtmlRules = await templateContext.GetRulesMeta();

            return model;
        }
    }

    #endregion

    public class SEOModel
    {
        public string LangCode { get; set; }
        public string metaHtmlRules { get; set; }
        public string metaHtmlHome { get; set; }
        public string metaHtmlTerms { get; set; }
        public string metaHtmlLeaderboard { get; set; }
        public string metaHtmlDisclaimer { get; set; }
    }

}