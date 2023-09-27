using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Gaming.Quiz.DataInitializer.GamedayMapping
{
    public class GamedayMapping
    {
        public static List<Contracts.GamedayMapping.GamedayMapping> InitializeGamedayMapping(DataSet ds, out Int32 retVal)
        {
            retVal = -70;
            List<Contracts.GamedayMapping.GamedayMapping> vGamedayMappings = new List<Contracts.GamedayMapping.GamedayMapping>();

            try
            {
                if(ds != null)
                {
                    if(ds.Tables != null && ds.Tables.Count > 0)
                    {
                        vGamedayMappings = (from a in ds.Tables[0].AsEnumerable()
                                            select new Contracts.GamedayMapping.GamedayMapping
                                            {
                                                Checked = false,
                                                QuizId = Convert.IsDBNull(a["quiz_id"]) ? 0 : Convert.ToInt32(a["quiz_id"]),
                                                Date = Convert.IsDBNull(a["date"]) ? "" : a["date"].ToString(),
                                                GamedayId = Convert.IsDBNull(a["gameday"]) ? 0 : Convert.ToInt32(a["gameday"]),
                                                IsMapped = Convert.IsDBNull(a["is_mapped"]) ? "" : a["is_mapped"].ToString(),
                                                TagName = Convert.IsDBNull(a["tag"]) ? "" : a["tag"].ToString(),
                                                IsMatchday = Convert.IsDBNull(a["is_matchday"]) ? 0 : Convert.ToInt32(a["is_matchday"]),
                                                QuestionsLeft = Convert.IsDBNull(a["qtn_left_in_tag"]) ? 0 : Convert.ToInt32(a["qtn_left_in_tag"]),
                                            }).ToList();
                    }
                }

                if (vGamedayMappings != null) retVal = 1;
            }
            catch (Exception ex)
            {
                retVal = -70;
                throw new Exception("Inside Dataint ==> retval ==> " + retVal + " error message" + ex.Message + "== DB output===> Table 1 " + GenericFunctions.DebugTable(ds.Tables[0]));
            }
            return vGamedayMappings;
        }
    }
}
