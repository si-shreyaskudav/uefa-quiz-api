using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaming.Quiz.Admin.Models
{
    public class FeedIngestionModel
    {
        public FeedIngestionModel()
        {
            BSGamedayId = 0;
            BSWeekId = 0;
            OVGamedayId = 0;
            OVWeekId = 0;
        }

        public String Translations { get; set; }
        public Int64? BSGamedayId { get; set; }
        public Int64? BSWeekId { get; set; }
        public Int64? BSMonthId { get; set; }

        public Int64? OVGamedayId { get; set; }
        public Int64? OVWeekId { get; set; }
        public Int64? OVMonthId { get; set; }
        public Int64? PlayerId { get; set; }
        public Int64? QuizId { get; set; }

        public Int64? PlrGamedayId { get; set; }
        public Int64? PlrWeekId { get; set; }
    }
}
