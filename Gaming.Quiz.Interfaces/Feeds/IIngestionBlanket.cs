using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Interfaces.Feeds
{
    public interface IIngestionBlanket
    {
        Task<Tuple<int, string>> Languages();
        Task<Tuple<int, string>> Translations(string translations, String lang = "en");
    }
}
