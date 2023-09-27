using Gaming.Quiz.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Feeds
{
    public interface IFeedsBlanket
    {
        Task<HTTPResponse> GetLanguages();
        Task<HTTPResponse> GetTranslations(string langCode);
        Task<HTTPResponse> GetTutorial(string langCode);
    }
}
