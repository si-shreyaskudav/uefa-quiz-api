using Gaming.Quiz.Contracts.Feeds;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Blanket.Validation
{
    public class Profanity
    {
        //public async Task<bool> Check(String searchText)
        //{
        //    bool valid = true;
        //    try
        //    {
        //        if (String.IsNullOrEmpty(searchText))
        //            return false;
        //        searchText = searchText.Trim().ToLower();
        //        ProfanityWords profaneWords = new ProfanityWords();
        //        #region " Fetch "
        //        String data = _MemCache.CacheTryGet(_MemCache._ProfanityToken);
        //        if (String.IsNullOrEmpty(data))
        //        {
        //            data = await IAsset.GET(_Asset.Profanity());
        //            _MemCache.CacheTrySet(_MemCache._ProfanityToken, data, TimeSpan.FromDays(1));
        //        }
        //        profaneWords = GenericFunctions.Deserialize<ProfanityWords>(data);
        //        #endregion
        //        #region " Logic "
        //        if (searchText.IndexOf(" ") > -1)
        //        {
        //            String[] texts = searchText.Split(" ");
        //            foreach (String t in texts)
        //            {
        //                if (profaneWords.words.Where(c => c.ToLower() == t).Count() > 0)
        //                {
        //                    valid = false;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (profaneWords.words.Where(c => c.ToLower() == searchText).Count() > 0)
        //            valid = false;
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        valid = true;
        //        throw ex;
        //    }
        //    return valid;
        //}
    }
}
