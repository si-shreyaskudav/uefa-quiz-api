using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.PointCalculation
{
   public class PntPrcGet
    {
        public Int32 QzMId { get; set; }
        public Int32 MonthId { get; set; }
        public Int32 WeekId { get; set; }
        public Int32 GamedayId { get; set; }
        public string SettleProcessFlag { get; set; }
        public Int32 retVal { get; set; }
    }
    public class SettlmentGet
    {
        public Int32 QuMasterid { get; set; }
        public Int32? QuMonthid { get; set; }
        public Int32? QuWeekid { get; set; }
        public Int32? QuGamedayid { get; set; }
    }
}
