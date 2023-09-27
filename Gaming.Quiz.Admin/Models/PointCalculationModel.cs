using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class PointCalculationModel
    {
        public Int64? UserId { get; set; }
        public Int32? MonthId { get; set; }
        public Int32? WeekId { get; set; }
        public Int32? GamedayId { get; set; }

        public Int32? BsMonthId { get; set; }
        public Int32? BsWeekId { get; set; }
        public Int32? BsGamedayId { get; set; }
    }

    #region " Worker "

    public class PointCalculationModelWorker
    {
        //public ServicesModel GetModel(List<vAdminLLget> vAdminLL, List<vGamedayDetails> vGdDet, List<vMonthDetails> vMonthGet)
        //{
            

        //    return model;
        //}
    }

    #endregion
}
