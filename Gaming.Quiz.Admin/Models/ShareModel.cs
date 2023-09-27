using System;
using System.Collections.Generic;
using System.Linq;
using  Gaming.Quiz.Interfaces.Asset;

namespace  Gaming.Quiz.Admin.Models
{
    public class ShareModel
    {
        public ShareModel()
        {
            Config = "";
            Meta = "";
            LanguageId = "en";
            Language = new List<GenericDropdown>();
        }

        public String Config { get; set; }
        public String Meta { get; set; }

        public String LanguageId { get; set; }
        public List<GenericDropdown> Language { get; set; }
    }

    #region " Worker "

    public class ShareWorker
    {

    }

    #endregion
}
