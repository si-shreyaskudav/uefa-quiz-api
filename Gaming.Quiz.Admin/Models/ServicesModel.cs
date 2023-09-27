using Gaming.Quiz.Contracts.Admin;
using Gaming.Quiz.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class ServicesModel
    {

        #region quizmapping

        public string QzMapDate { get; set; }
        public Int64? QzMappMId { get; set; }

        #endregion
        
        #region pendingquiz

        public Int64? QzPndMId { get; set; }

        #endregion

        #region questiongdget

        public string QGdGetDate { get; set; }
        public Int64? QGdGetMId { get; set; }
        public Int64? QGdSprtId { get; set; }
        public Int64? QGdCtgId { get; set; }

        #endregion

        #region gddetailsget

        public Int64? GDDetMId { get; set; }

        #endregion

        #region gddetailsget

        public Int64? MnthDetMId { get; set; }

        #endregion

        #region adminllget
        public Int64? adQMid { get; set; }
        #endregion

        public List<vGamedayDetails> gdDetails { get; set; }
        public List<vMonthDetails> monthDetails { get; set; }
        public List<vAdminLLget> adminLL { get; set; }


    }

    #region " Worker "

    public class ServicesModelWorker
    {
        public ServicesModel GetModel(List<vAdminLLget> vAdminLL, List<vGamedayDetails> vGdDet, List<vMonthDetails> vMonthGet)
        {
            ServicesModel model = new ServicesModel();

            if (vGdDet != null)
            {
                model.gdDetails = vGdDet;
            }
            else
                model.gdDetails = null;


            if (vMonthGet != null)
            {
                model.monthDetails = vMonthGet;
            }
            else
                model.monthDetails = null;

            if (vAdminLL != null)
            {
                model.adminLL = vAdminLL;
            }
            else
                model.adminLL = null;

            return model;
        }
    }

    #endregion

}
