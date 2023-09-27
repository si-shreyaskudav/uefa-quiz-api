using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Admin
{
    public class vGamedayDetails
    {

        #region "Display Data"
        public string vGameday { get; set; }
        public string vGamedayNo { get; set; }
        public string vStartDate { get; set; }
        public string vEndDate { get; set; }
        public string vQuestionReady { get; set; }
        public string vTotCtg { get; set; }
        public string vTotSports { get; set; }
        public string vQUpdDate { get; set; }
        public string vQBnkPrsnt { get; set; }
        public string vQsnCnt { get; set; }
        public string vTotAttempt { get; set; }

        #endregion
    }

    public class vMonthDetails
    {

        #region "Display Data"

        public string vMonthId { get; set; }
        public string vMonthNo { get; set; }
        public string vMnStartDate { get; set; }
        public string vMnEndDate { get; set; }
        public string vIsCurrent { get; set; }
        public string vIsLocked { get; set; }
        public string vIsLeaderbordShow { get; set; }

        #endregion
    }

    public class vAdminLLget
    {
        #region "Display Data"

        public string vLifelineMid { get; set; }
        public string vMid { get; set; }
        public string vDesc { get; set; }
        public string vShortDesc { get; set; }
        public string vIsActive { get; set; }


        #endregion
    }


}
